using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileParser
{
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
                Console.WriteLine($"Cannot read input folder path '{inputFolder??string.Empty}': {ex.Message}");
                return;
            }

            for (int fileIndex = 0; fileIndex < 10; fileIndex++)
            {
                var outputFileName = $"File_{fileIndex}.txt";
                var outputFilePath = Path.Combine(inputFolder, outputFileName);

                Console.WriteLine($"Generating file '{outputFileName}'..");

                if (File.Exists(outputFilePath))
                {
                    Console.WriteLine($"Removing existing file '{outputFileName}'..");
                    File.Delete(outputFilePath);
                }
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

            Console.WriteLine("File generated successfully");
        }
    }
}