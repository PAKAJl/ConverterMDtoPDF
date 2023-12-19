using System.Xml.Linq;

public class FileConverter
{
    public string outputPath {get; private set;}
    GroupDocs.Conversion.Converter converter;
    public FileConverter()
    {
        FileFinder ff = new FileFinder();
        outputPath = ff.GetOutputPathDir();
    }

    public void Convert(XDocument xdoc)
    {
        Directory.CreateDirectory("htmlTemp");
        Directory.CreateDirectory(outputPath);
        foreach(XElement dir in xdoc.Root.Elements())
        {
            string dirName = dir.Attribute("name").Value.Split($"/").Last<string>();
            Directory.CreateDirectory(@$"{outputPath}/{dirName}");
            foreach(XElement file in dir.Elements())
            {
                string nameFile = file.Attribute("name").Value.Split(@"/").Last<string>().Split(".")[0];
                converter = new GroupDocs.Conversion.Converter(file.Attribute("name").Value);

                var convertOptions = converter.GetPossibleConversions()["html"].ConvertOptions; 
                converter.Convert($"htmlTemp/{nameFile}.html", convertOptions);     
                ConvertMdToPdf($"htmlTemp/{nameFile}.html", $@"{outputPath}/{dirName}/{nameFile}.pdf");
                
            }
        }
    }

    private void ConvertMdToPdf(string htmlPath, string savePath)
    {   
        var renderer = new ChromePdfRenderer();
        PdfDocument pdf;
        string deleteText = "<span class=\"awspan awtext001\" style=\"color:#ff0000; left:0pt; top:0.51pt; line-height:13.29pt;\">Created with evaluation version of GroupDocs.Conversion © Aspose Pty Ltd 2001-2023. All </span><span class=\"awspan awtext001\" style=\"color:#ff0000; left:0pt; top:14.31pt; line-height:13.29pt;\">Rights Reserved.</span>";
        using (StreamReader reader = new StreamReader(htmlPath))
        {
            string text = reader.ReadToEnd();
            text = text.Replace(deleteText, "");
           
            pdf = renderer.RenderHtmlAsPdf(text);
              
        }
        
        pdf.SaveAs(savePath);    
    }
}


    