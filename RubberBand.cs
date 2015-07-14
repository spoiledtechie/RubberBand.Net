using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Reflection;

namespace RubberBandNet
{
    public static class RubberBandService
    {
        public static string RubberBandExecutableFilePath;

        private const int MaximumBuffers = 25;

        public static Queue<string> PreviousBuffers = new Queue<string>();

        static RubberBandService()
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;

            RubberBandExecutableFilePath = currentDirectory + ConfigurationManager.AppSettings["RubberBandExecutableFilePath"];

        }


        public static string Execute(string inputFilePath)
        {
            if (String.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException("Input file path cannot be null");
            }

            RubberBandParameters parameters = new RubberBandParameters()
            {
                InputFilePath = inputFilePath
            };

            return Execute(parameters);
        }

        public static string Execute(string inputFilePath, string outputOptions, string outputFilePath)
        {
            if (String.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException("Input file path cannot be null");
            }

            if (String.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException("Output file path cannot be null");
            }

            RubberBandParameters parameters = new RubberBandParameters()
            {
                InputFilePath = inputFilePath,
                OutputOptions = outputOptions,
                OutputFilePath = outputFilePath,
            };

            return Execute(parameters);

        }

        public static string Execute(string inputFilePath, string outputOptions)
        {
            if (String.IsNullOrWhiteSpace(inputFilePath))
            {
                throw new ArgumentNullException("Input file path cannot be null");
            }

            RubberBandParameters parameters = new RubberBandParameters()
            {
                InputFilePath = inputFilePath,
                OutputOptions = outputOptions
            };

            return Execute(parameters);
        }

        public static string Execute(RubberBandParameters parameters)
        {
            if (String.IsNullOrWhiteSpace(RubberBandExecutableFilePath))
            {
                throw new ArgumentNullException("Path to RubberBand executable cannot be null");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("RubberBand parameters cannot be completely null");
            }

            using (Process ffmpegProcess = new Process())
            {
                ProcessStartInfo info = new ProcessStartInfo(RubberBandExecutableFilePath)
                {
                    Arguments = parameters.ToString(),
                    WorkingDirectory = Path.GetDirectoryName(RubberBandExecutableFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                ffmpegProcess.StartInfo = info;
                ffmpegProcess.Start();
                string processOutput = ffmpegProcess.StandardError.ReadToEnd();
                ffmpegProcess.WaitForExit();
                PreviousBuffers.Enqueue(processOutput);
                lock (PreviousBuffers)
                {
                    while (PreviousBuffers.Count > MaximumBuffers)
                    {
                        PreviousBuffers.Dequeue();
                    }

                }

                return processOutput;
            }

        }

    }

}
