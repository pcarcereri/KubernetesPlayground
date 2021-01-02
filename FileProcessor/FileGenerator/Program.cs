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
            try
            {
                Path.GetFullPath(inputFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read input folder path '{inputFolder ?? string.Empty}': {ex.Message}");
                return;
            }

            RemoveAllFilesInFolder(inputFolder);

            GenerateFiles(inputFolder);

            Console.WriteLine("File generated successfully");
        }

        private static void GenerateFiles(string inputFolder)
        {
            for (int fileIndex = 0; fileIndex < 10; fileIndex++)
            {
                var outputFileName = $"File_{fileIndex}.txt";
                var outputFilePath = Path.Combine(inputFolder, outputFileName);

                Console.WriteLine($"Generating file '{outputFileName}'..");
                using (var outputStream = File.CreateText(outputFilePath))
                {
                    for (int lineNumber = 0; lineNumber < fileIndex; lineNumber++)
                    {
                        var outputLine = $"Line {lineNumber}";
                        outputStream.WriteLine(outputLine);
                    }
                }

                Console.WriteLine($"Files generated correctly");

                Thread.Sleep(200);
            }
        }

        // source: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
        private static void RemoveAllFilesInFolder(string inputFolder)
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