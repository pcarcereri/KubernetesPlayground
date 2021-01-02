using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileParser
{
    class Program
    {
        private static string processedFileNameEnding = "_processed.txt";

        // NOTE: below code is not state of the art and it's only as PoC 
        static void Main(string[] args)
        {
            Console.WriteLine("Parsing file..");

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

            var existingProcessedFiles = Directory.GetFiles(inputFolder).Where(file => file.EndsWith(processedFileNameEnding));
            foreach (var existingProcessedFile in existingProcessedFiles)
            {
                File.Delete(existingProcessedFile);
            }

            var availableFiles = Directory.GetFiles(inputFolder).Where(file => file.EndsWith(".txt")).OrderBy(file => file);

            if (!availableFiles.Any())
            {
                Console.WriteLine($"No files available in folder '{inputFolder}'");
                return;
            }

            foreach (var availableFile in availableFiles)
            {
                ParseFile(inputFolder, availableFile);
                Thread.Sleep(200);
            }

            Console.WriteLine($"{availableFiles.Count()} files processed successfully");
        }

        private static void ParseFile(string inputFolder, string inputFilePath)
        {
            var inputFileName = Path.GetFileName(inputFilePath);
          
            var outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + processedFileNameEnding;
            Console.WriteLine($"Generating file '{outputFileName}'..");

            var outputFile = Path.Combine(inputFolder, outputFileName);
            if (File.Exists(outputFile))
            {
                Console.WriteLine($"Removing existing file '{outputFileName}'..");
                File.Delete(outputFile);
            }

            var lines = Enumerable.Empty<string>();
            try
            {
                Console.WriteLine($"Reading file '{inputFileName}'..");
                lines = File.ReadAllLines(inputFilePath).Reverse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot open input file '{inputFilePath}': {ex.Message}");
                return;
            }

            using (var outputStream = File.CreateText(outputFile))
            {
                foreach (var inputLine in lines)
                {
                    var outputLine = inputLine.Reverse().ToArray();
                    outputStream.WriteLine(outputLine);
                }
            }

            Console.WriteLine($"File {inputFileName}' processed successfully into file {outputFileName}'");
        }
    }
}