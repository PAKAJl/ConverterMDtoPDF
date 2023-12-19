Console.WriteLine("Начало Работы...");
FileFinder fileFinder = new FileFinder();
FileConverter fileConverter = new FileConverter();

fileConverter.Convert(fileFinder.ScanDirectory());
Console.WriteLine("Файлы Конвертированы");