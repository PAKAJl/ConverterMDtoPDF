using System.Xml;
using System.Xml.Linq;
public class FileFinder
{
    public XmlDocument configFile {get; set;}
    public XmlElement? xRoot {get; set;}
    public string sourcePath {get; private set;}
    public FileFinder()
    {
        configFile = new XmlDocument();
        configFile.Load(Environment.CurrentDirectory+"/config.xml");
        xRoot = configFile.DocumentElement;
        sourcePath = GetSourcePathDir();
    }

    public FileFinder(string configPath)
    {
        configFile = new XmlDocument();
        configFile.Load(configPath);
        xRoot = configFile.DocumentElement;
        sourcePath = GetSourcePathDir();
    }

    public string GetSourcePathDir()
    {
        if(xRoot != null)
        {
            foreach(XmlElement item in xRoot)
            {
                if (item.Name == "sourceDir")
                {
                    return item.InnerText;
                }
            } 
        }

        return "";
    }

    public string GetOutputPathDir()
    {
        if(xRoot != null)
        {
            foreach(XmlElement item in xRoot)
            {
                if (item.Name == "saveDir")
                {
                    return item.InnerText;
                }
            } 
        }

        return "";
    }

    public XDocument ScanDirectory()
    {
        XDocument result = new XDocument();
        XElement root = new XElement("directories");
        result.Add(root);
        string[] catalogs = Directory.GetDirectories(sourcePath);

        foreach (string item in catalogs)
        {
            XElement dir = new XElement("directory");
            XAttribute nameAttr = new XAttribute("name", item);
            dir.Add(nameAttr);
            root.Add(dir);
            string[] files = Directory.GetFiles(item);

            foreach(string file in files)
            {
                XElement fileNode = new XElement("file");
                XAttribute nameFileAttr = new XAttribute("name", file);
                fileNode.Add(nameFileAttr);
                dir.Add(fileNode);
            }
        }

        return result;
    }

}