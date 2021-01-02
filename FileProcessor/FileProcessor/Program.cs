using System;
using System.Collections.Generic;
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
            Console.WriteLine("Processing file..");

            var inputFolder = args.ElementAtOrDefault(0);
            if (IsFolderPathInvalid(inputFolder))
            {
                Console.WriteLine($"Cannot read input folder path '{inputFolder ?? string.Empty}'");
            }

            while(TryGetFileToProcess(inputFolder, out string fileToProcess))
            {
                ProcessFile(inputFolder, fileToProcess);
                File.Delete(fileToProcess);

                Thread.Sleep(500);
            }

            Console.WriteLine($"Files processed successfully");
        }

        private static bool TryGetFileToProcess(string inputFolder, out string fileToProcess)
        {
            var availableCandidateFiles = Directory.GetFiles(inputFolder).Where(file => file.EndsWith(".txt")).ToList();
            fileToProcess = string.Empty;

            while(availableCandidateFiles.Any() && string.IsNullOrWhiteSpace(fileToProcess))
            {
                fileToProcess = availableCandidateFiles.First();
                availableCandidateFiles.Remove(fileToProcess);

               if(IsFileLocked(fileToProcess))
                {
                    return true;
                }
            }

            return false; 
        }

        // code from: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        private static bool IsFileLocked(string file)
        {
            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        // TODO: refactor below method
        private static void ProcessFile(string inputFolder, string inputFilePath)
        {
            var inputFileName = Path.GetFileName(inputFilePath);
            var lines = Enumerable.Empty<string>();
            try
            {
                Console.WriteLine($"Reading file '{inputFileName}'..");
                lines = File.ReadAllLines(inputFilePath).Reverse();
                Console.WriteLine($"File '{inputFileName}' deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot open input file '{inputFilePath}': {ex.Message}");
                return;
            }

            var outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + processedFileNameEnding;
            var outputFile = Path.Combine(inputFolder, outputFileName);
            Console.WriteLine($"Generating file '{outputFileName}'..");

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

        // code from https://stackoverflow.com/questions/3137097/check-if-a-string-is-a-valid-windows-directory-folder-path
        private static bool IsFolderPathInvalid(string inputFolder)
        {
            try
            {
                Path.GetFullPath(inputFolder);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}