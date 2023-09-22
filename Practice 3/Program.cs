using System;
using System.IO;

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
    string dictionaryPath;
    string fileName;
    string filePath;
    DirectoryInfo dirInfo;
    FileInfo fileInfo;

    public void Update(string fileName)
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
  class Program
  {
    static void printTasks()
    {
      Console.WriteLine("1. Вывести информацию в консоль о логических дисках, именах, метке тома, размере и типе файловой системы.");
      Console.WriteLine("2. Работа с файлами");
    }
    static void printFileTasks()
    {
      Console.WriteLine("1. Создать файл");
      Console.WriteLine("2. Записать в файл строку, введённую пользователем");
      Console.WriteLine("3. Прочитать файл в консоль");
      Console.WriteLine("4. Удалить файл");
      Console.WriteLine("5. Назад");
    }
    static void Main(string[] args)
    {    
      PCInfo pcInfo = new PCInfo();
      FileManager fileManager = new FileManager();
      printTasks();
      string firstTask, secondTask;
      string fileName;
      while (true)
      {
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
                      if (fileManager.FileExist())
                      {
                        fileManager.PrintFile();
                      }
                      else
                      {
                        Console.WriteLine("Файл не найден ...");
                      }
                    }
                    break;

                  case "4":
                    {
                      Console.WriteLine("Введите название файла с расширенем");
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
                    }
                    break;
                  default:
                    fileCaseFlag = false;
                    break;
                }

              }
            }
            break;
            
            
        }
      }

      Console.WriteLine("Hello World!");
    }
  }
}
