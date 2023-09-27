using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Xml.Serialization;

namespace Practice_3
{



  class PCInfo
  {
    public void diskInfo()
    {
      DriveInfo[] drives = DriveInfo.GetDrives();

      foreach (DriveInfo drive in drives)
      {
        Console.WriteLine($"Название: {drive.Name}");
        Console.WriteLine($"Тип: {drive.DriveType}");
        if (drive.IsReady)
        {
          Console.WriteLine($"Объем диска: {drive.TotalSize}");
          Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
          Console.WriteLine($"Метка: {drive.VolumeLabel}");
        }
        Console.WriteLine();
      }
    }
  }


  class FileManager
  {
    public FileManager()
    {
      dictionaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CreatedFiles";
      dirInfo = new DirectoryInfo(dictionaryPath);
    }
    public FileManager(string fileName)
    {
      this.fileName = fileName;
      dictionaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CreatedFiles";
      dirInfo = new DirectoryInfo(dictionaryPath);
      filePath = dictionaryPath + @"\" + fileName;
      fileInfo = new FileInfo(filePath);
    }
    protected string dictionaryPath;
    protected string fileName;
    protected string filePath;
    protected DirectoryInfo dirInfo;
    protected FileInfo fileInfo;

    virtual public void Update(string fileName)
    {
      this.fileName = fileName;
      filePath = dictionaryPath + @"\" + fileName;
      fileInfo = new FileInfo(filePath);
    }
    public bool FileExist()
    {
      return fileInfo.Exists;
    }
    public void CreateFile()
    {
      if (!dirInfo.Exists)
      {
        dirInfo.Create();
      }
      if (!fileInfo.Exists)
        fileInfo.Create().Close();
    }
    public void InputFile(string text)
    {
      try
      {
        using (StreamWriter writer = fileInfo.AppendText())
        {
          writer.WriteLine(text);
        }
        Console.WriteLine("Запись выполнена");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
    public void PrintFile(string fileName)
    {
      try
      {
        using (StreamReader reader = new StreamReader(dictionaryPath + @"\" + fileName))
        {
          string text =  reader.ReadToEnd();
          Console.WriteLine(text);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    public void PrintFile()
    {
      try
      {
        using (StreamReader reader = fileInfo.OpenText())
        {
          string text = reader.ReadToEnd();
          Console.WriteLine(text);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    public void Delete()
    {
      fileInfo.Delete();
    }
    

  }

  class JsonManager : FileManager
  {
    public JsonManager() : base()
    {

    }
    public JsonManager(string fileName)
    {
      this.fileName = fileName + ".json";
      dictionaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CreatedFiles";
      dirInfo = new DirectoryInfo(dictionaryPath);
      filePath = dictionaryPath + @"\" + this.fileName;
      fileInfo = new FileInfo(filePath);
    }
    public override void Update(string fileName)
    {
      this.fileName = fileName + ".json";
      filePath = dictionaryPath + @"\" + this.fileName;
      fileInfo = new FileInfo(filePath);
    }
    public void InputFile(object obj)
    {
      try
      {
        using (StreamWriter writer = fileInfo.CreateText())
        {
          string jsonString = JsonSerializer.Serialize(obj);
          writer.Write(jsonString);

        }
        Console.WriteLine("Запись выполнена");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
  }

  class XmlManager : FileManager
  {
    public XmlManager() : base()
    {

    }
    public XmlManager(string fileName)
    {
      this.fileName = fileName + ".xml";
      dictionaryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CreatedFiles";
      dirInfo = new DirectoryInfo(dictionaryPath);
      filePath = dictionaryPath + @"\" + this.fileName;
      fileInfo = new FileInfo(filePath);
    }
    public override void Update(string fileName)
    {
      this.fileName = fileName + ".xml";
      filePath = dictionaryPath + @"\" + this.fileName;
      fileInfo = new FileInfo(filePath);
    }
    public void InputFile<T>(T obj)
    {
      try
      {
        using (StreamWriter writer = fileInfo.CreateText())
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
          xmlSerializer.Serialize(writer, obj);

        }
        Console.WriteLine("Запись выполнена");
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
  }
  class ZipManager : FileManager
  {
    public ZipManager() : base()
    {

    }
    string zipName;
    string zipPath;
    public void UpdateZip(string zipName)
    {
      this.zipName = zipName;
      zipPath = dictionaryPath + @"\" + zipName + ".zip";
    }
    public void CreateZip()
    {
      try
      {
        using (ZipArchive zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create)) { }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    public void ToZip()
    {
      try
      {
        using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
        {
          zipArchive.CreateEntryFromFile(filePath, fileName);
          fileInfo.Delete();
          Console.WriteLine("В архив добавлена запись!");
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
    public void ExtractZip()
    {
      List<string> fileNames = new List<string>();
      using (ZipArchive zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
      {        
        foreach (ZipArchiveEntry entry in zipArchive.Entries)
        {
          string fileName = entry.FullName;
          fileNames.Add(fileName);
        }

      }
      ZipFile.ExtractToDirectory(zipPath, dictionaryPath);
      using (ZipArchive zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
      {
        while (zipArchive.Entries.Count > 0)
        {
          zipArchive.Entries[0].Delete();
        }
      }
      foreach (string item in fileNames)
      {
        PrintFile(item);
      }
    }
  }
  public class SerializableClass
  {
    public SerializableClass()
    {

    }
    public SerializableClass (string name,string age)
    {
      this.name = name;
      this.age = age;
    }
    public string name;
    public string age;
  }
  class Program
  {
    static void printTasks()
    {
      Console.WriteLine("1. Вывести информацию в консоль о логических дисках, именах, метке тома, размере и типе файловой системы.");
      Console.WriteLine("2. Работа с файлами");
      Console.WriteLine("3. Работа с JSON файлами");
      Console.WriteLine("4. Работа с XML файлами");
      Console.WriteLine("5. Работа с ZIP файлами");
      Console.WriteLine("6. Удалить файл");
    }
    static void printFileTasks()
    {
      Console.WriteLine("1. Создать файл");
      Console.WriteLine("2. Записать в файл строку, введённую пользователем");
      Console.WriteLine("3. Прочитать файл в консоль");
      Console.WriteLine("4. Назад");
    }
    static void printJsonTasks()
    {
      Console.WriteLine("1. Создать файл");
      Console.WriteLine("2. Записать в файл объект");
      Console.WriteLine("3. Прочитать файл в консоль");
      Console.WriteLine("4. Назад");
    }
    static void printXmlTasks()
    {
      Console.WriteLine("1. Создать файл");
      Console.WriteLine("2. Записать в файл объект");
      Console.WriteLine("3. Прочитать файл в консоль");
      Console.WriteLine("4. Назад");
    }
    static void printZipTasks()
    {
      Console.WriteLine("1. Создать архив");
      Console.WriteLine("2. Добавить файл в архив");
      Console.WriteLine("3. Разархивировать файл и вывести данные");
      Console.WriteLine("4. Назад");
    }
    static void Main(string[] args)
    {
      PCInfo pcInfo = new PCInfo();
      FileManager fileManager = new FileManager();
      JsonManager jsonManager = new JsonManager();
      XmlManager xmlManager = new XmlManager();
      ZipManager zipManager = new ZipManager();
      string firstTask, secondTask;
      string fileName;
      while (true)
      {
        printTasks();
        firstTask = Console.ReadLine();
        switch (firstTask)
        {
          case "1":
            pcInfo.diskInfo();
            break;
          case "2":
            {
              bool fileCaseFlag = true;
              while (fileCaseFlag)
              {
                printFileTasks();
                secondTask = Console.ReadLine();
                switch (secondTask)
                {
                  case "1":
                    {
                      Console.WriteLine("Введите имя файла с расширением");
                      fileName = Console.ReadLine();
                      fileManager.Update(fileName);
                      fileManager.CreateFile();
                    }
                    break;
                  case "2":
                    {
                      string text;
                      Console.WriteLine("Введите название файла с расширенем");
                      fileName = Console.ReadLine();
                      fileManager.Update(fileName);
                      if (fileManager.FileExist())
                      {
                        Console.WriteLine("Введите текст");
                        text = Console.ReadLine();
                        fileManager.InputFile(text);
                      }
                      else
                      {
                        Console.WriteLine("Файл не найден ...");
                      }
                    }
                    break;

                  case "3":
                    {
                      Console.WriteLine("Введите название файла с расширенем");
                      fileName = Console.ReadLine();
                      fileManager.Update(fileName);
                      fileManager.PrintFile();
                    }
                    break;

                  default:
                    fileCaseFlag = false;
                    break;
                }

              }
            }
            break;

          case "3":
            {
              bool jsonCaseFlag = true;
              while (jsonCaseFlag)
              {
                printJsonTasks();
                secondTask = Console.ReadLine();
                switch (secondTask)
                {
                  case "1":
                    Console.WriteLine("Введите имя файла без расширения");
                    fileName = Console.ReadLine();
                    jsonManager.Update(fileName);
                    jsonManager.CreateFile();
                    break;
                  case "2":
                    Console.WriteLine("Введите название файла без расширения");
                    fileName = Console.ReadLine();
                    jsonManager.Update(fileName);
                    jsonManager.InputFile(new { name = "Maksim", age = 21 });
                    break;
                  case "3":
                    Console.WriteLine("Введите название файла без расширения");
                    fileName = Console.ReadLine();
                    jsonManager.Update(fileName);
                    jsonManager.PrintFile();
                    break;

                  default:
                    jsonCaseFlag = false;
                    break;
                }
              }

            }
            break;

          case "4":
            {
              bool xmlCaseFlag = true;
              while (xmlCaseFlag)
              {
                printXmlTasks();
                secondTask = Console.ReadLine();
                switch (secondTask)
                {
                  case "1":
                    Console.WriteLine("Введите имя файла без расширения");
                    fileName = Console.ReadLine();
                    xmlManager.Update(fileName);
                    xmlManager.CreateFile();                    
                    break;
                  case "2":
                    Console.WriteLine("Введите название файла без расширения");
                    fileName = Console.ReadLine();
                    xmlManager.Update(fileName);
                    Console.WriteLine("Введите имя для записи");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите возраст для записи");
                    string age = Console.ReadLine();                    
                    xmlManager.InputFile(new SerializableClass(name,age));
                    break;
                  case "3":
                    Console.WriteLine("Введите название файла без расширения");
                    fileName = Console.ReadLine();
                    xmlManager.Update(fileName);
                    xmlManager.PrintFile();
                    break;
                  default:
                    xmlCaseFlag = false;
                    break;
                }
              }
            }
            break;
          case "5":
            {
              bool zipCaseFlag = true;
              while (zipCaseFlag)
              {
                printZipTasks();
                secondTask = Console.ReadLine();
                string zipName;
                switch (secondTask)
                {
                  case "1":
                    Console.WriteLine("Введите имя архива без расширения");
                    zipName = Console.ReadLine();
                    zipManager.UpdateZip(zipName);
                    zipManager.CreateZip();
                    break;
                  case "2":
                    Console.WriteLine("Введите название архива без расширения");
                    zipName = Console.ReadLine();
                    Console.WriteLine("Введите название файла");
                    fileName = Console.ReadLine();
                    zipManager.Update(fileName);
                    zipManager.UpdateZip(zipName);
                    zipManager.ToZip();
                    break;
                  case "3":
                    Console.WriteLine("Введите название архива без расширения");
                    zipName = Console.ReadLine();
                    zipManager.UpdateZip(zipName);
                    zipManager.ExtractZip();
                    break;
                  default:
                    zipCaseFlag = false;
                    break;
                }
              }
            }            
            break;
          case "6":
            Console.WriteLine("Введите название файла c расширенем");
            fileName = Console.ReadLine();
            fileManager.Update(fileName);
            if (fileManager.FileExist())
            {
              fileManager.Delete();
            }
            else
            {
              Console.WriteLine("Файл не найден ...");
            }
            break;
        }
      }
    }
  }
}
