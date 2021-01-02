using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileParser
{
    // NOTE: below code is not state of the art and it's only as PoC 
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating file..");

            var inputFolder = args.ElementAtOrDefault(0);
            if(IsFolderPathInvalid(inputFolder))
            {
                Console.WriteLine($"Cannot read input folder path '{inputFolder ?? string.Empty}'");
                return;
            }

            DeleteAllFilesInFolder(inputFolder);

            while(true)
            {
                GenerateFile(inputFolder);
                Thread.Sleep(500);
            }
        }

        // code from https://stackoverflow.com/questions/3137097/check-if-a-string-is-a-valid-windows-directory-folder-path
        private static bool IsFolderPathInvalid(string inputFolder)
        {
            var isFolderPathValid = false;
            try
            {
                Path.GetFullPath(inputFolder);
                isFolderPathValid = true;
            }
            catch
            {
            }
            return !isFolderPathValid;
        }

        private static void GenerateFile(string inputFolder)
        {
            var outputFileName = $"File_{DateTime.Now.ToLongTimeString()}.txt";
            var outputFilePath = Path.Combine(inputFolder, outputFileName);

            Console.WriteLine($"Generating file '{outputFileName}'..");
                using (var outputStream = File.CreateText(outputFilePath))
                {
                    for (int lineNumber = 0; lineNumber < 10; lineNumber++)
                    {
                        var outputLine = $"Line {lineNumber}";
                        outputStream.WriteLine(outputLine);
                    }
                }

                Console.WriteLine($"Files generated correctly");
        }

        // source: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
        private static void DeleteAllFilesInFolder(string inputFolder)
        {
            DirectoryInfo di = new DirectoryInfo(inputFolder);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}