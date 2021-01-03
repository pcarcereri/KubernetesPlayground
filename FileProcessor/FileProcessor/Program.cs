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
            var inputFolder = args.ElementAtOrDefault(0) ?? string.Empty;
            if (IsFolderPathInvalid(inputFolder))
            {
                Console.WriteLine($"Cannot read input folder path '{inputFolder}'");
                return;
            }

            Console.WriteLine("Processing file..");
            while (TryGetAndLockFileToProcess(inputFolder, out FileStream fileToProcess))
            {
                ProcessAndDeleteFile(inputFolder, fileToProcess);
                Thread.Sleep(1000);
            }

            Console.WriteLine($"All available files have been processed successfully");
        }

        private static bool TryGetAndLockFileToProcess(string inputFolder, out FileStream fileStream)
        {
            var availableCandidateFiles = GetTextFilesInFolder(inputFolder);
            Console.WriteLine($"Retrieved {availableCandidateFiles.Count()} candidate files for processing..");

            bool processingAvailableCandidateFiles = true;
            fileStream = null;

            while (availableCandidateFiles.Any() && processingAvailableCandidateFiles)
            {
                var candidatefFileToProcess = availableCandidateFiles.First();
                availableCandidateFiles.Remove(candidatefFileToProcess);

                if (TryLockFile(candidatefFileToProcess, out FileStream stream))
                {
                    processingAvailableCandidateFiles = false;
                    fileStream = stream;
                    Console.WriteLine($"Candidate file '{candidatefFileToProcess}' has been successfully scheduled for processing..");
                }
                else
                {
                    Console.WriteLine($"Candidate file '{candidatefFileToProcess}' cannot be locked and scheduled for processing..");
                }
            }

            return fileStream != null;
        }

        private static List<string> GetTextFilesInFolder(string inputFolder)
        {
            return Directory.GetFiles(inputFolder).Where(file => file.EndsWith(".txt") && !file.EndsWith(processedFileNameEnding))
                .Distinct()
                .OrderBy(file => file)
                .ToList();
        }

        // code from: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        private static bool TryLockFile(string file, out FileStream fileStream)
        {
            try
            {
                fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                return true;
            }
            catch (IOException)
            {
                fileStream = null;
                return false;
            }
        }

        // TODO: refactor below long method in submethods 
        private static void ProcessAndDeleteFile(string inputFolder, FileStream inputFileStream)
        {
            var inputFilePath = inputFileStream.Name;
            var inputFileName = Path.GetFileName(inputFilePath);
            var inputFileContent = string.Empty;
            try
            {
                Console.WriteLine($"Reading file '{inputFileName}'..");
                using (inputFileStream)
                using (var streamReader = new StreamReader(inputFileStream))
                {
                    inputFileContent = streamReader.ReadToEnd();
                }
                File.Delete(inputFilePath);
                Console.WriteLine($"File '{inputFileName}' successfully imported and deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot open locked file '{inputFilePath}': {ex.Message}");
                return;
            }

            var outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + processedFileNameEnding;
            var outputFile = Path.Combine(inputFolder, outputFileName);
            Console.WriteLine($"Generating file '{outputFileName}'..");

            using (var outputStream = File.CreateText(outputFile))
            {
                var reversedContent = inputFileContent.Reverse();
                outputStream.WriteLine(reversedContent);
            }

            Console.WriteLine($"File {inputFileName}' processed successfully into file {outputFileName}'");
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
    }
}