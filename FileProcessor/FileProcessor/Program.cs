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
            var inputFolder = args.ElementAtOrDefault(0);
            if (IsFolderPathInvalid(inputFolder))
            {
                Console.WriteLine($"Cannot read input folder path '{inputFolder ?? string.Empty}'");
                return;
            }

            // wait for the file generator to generate some file
            Console.WriteLine("Waiting for the file generator to start..");
            Thread.Sleep(5000);

            Console.WriteLine("Processing file..");
            while (TryGetAndLockFileToProcess(inputFolder, out FileStream fileToProcess))
            {
                ProcessAndDeleteFile(inputFolder, fileToProcess);
                Thread.Sleep(500);
            }

            Console.WriteLine($"Files processed successfully");
        }

        private static bool TryGetAndLockFileToProcess(string inputFolder, out FileStream fileStream)
        {
            var availableCandidateFiles = Directory.GetFiles(inputFolder).Where(file => file.EndsWith(".txt")).ToList();
            Console.WriteLine($"Retrieved {availableCandidateFiles.Count()} candidate files for processing..");

            bool processingAvailableCandidateFiles = true;
            fileStream = null;

            while (availableCandidateFiles.Any() && processingAvailableCandidateFiles)
            {
                var candidatefFileToProcess = availableCandidateFiles.First();
                availableCandidateFiles.Remove(candidatefFileToProcess);

                Console.WriteLine($"Picked candidate file '{candidatefFileToProcess}' for processing..");
                if (IsFileLocked(candidatefFileToProcess, out FileStream stream))
                {
                    processingAvailableCandidateFiles = false;
                    fileStream = stream;
                    Console.WriteLine($"Candidate file '{candidatefFileToProcess}' has been locked and scheduled for processing..");
                }
            }

            return fileStream != null; 
        }

        // code from: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        private static bool IsFileLocked(string file, out FileStream fileStream)
        {
            try
            {
                fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                fileStream = null;
                return true;
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