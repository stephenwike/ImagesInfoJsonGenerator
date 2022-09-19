using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageJsonGenerator
{
    class Program
    {
        private static ILogger _logger;
        private static Args _args;

        static void Main(string[] args)
        {
            _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            
            //Example
            args = new string[] { @"assets/images/my-art/",
            "C://home-dev/stephenwike.com/src/StephenWikeWeb/src/app/components/my-art/my-art.component.json"};

            if (args.Length != 2)
            {
                _logger.Error(@"Incorrect number of arguments. imagejsongenerator <image-base-dir>");
                _logger.Error(@"Incorrect number of arguments. imagejsongenerator <image-base-dir> <out-put-file>");
                return;
            }

            var currentDirectory = Directory.GetCurrentDirectory();

            //Example
            currentDirectory = "C://home-dev/stephenwike.com/src/StephenWikeWeb/src/";

            _args = new Args()
            {
                ProvidedPath = args[0],
                BasePath = Path.Combine(currentDirectory, args[0]),
                OutputFile = Path.Combine(currentDirectory, args[1])
            };
            _logger.Information("Current Directory: {currentDirectory}", Directory.GetCurrentDirectory());
            _logger.Information("Provided Path: {providedPath}", _args.ProvidedPath);
            _logger.Information("Images Base Path: {imagesPath}", _args.BasePath);
            _logger.Information("Output File: {outputFile}", _args.OutputFile);

            var imageFiles = SearchDirectories(_args.BasePath);
            _logger.Information("Program: SearchDirectories: complete");

            var imageInfos = CreateImageInfoFromFiles(imageFiles);
            _logger.Information("Program: CreateImageInfoFromFiles: complete");

            var imageString = JsonConvert.SerializeObject(imageInfos, Formatting.Indented);
            _logger.Information("Program: SerializeObject: complete");

            System.IO.File.WriteAllText(_args.OutputFile, imageString);
            _logger.Information("Program: WriteAllText: complete");
            _logger.Information("{imageString}", imageString);
            System.Console.WriteLine(imageString);
        }

        private static List<FileInfo> SearchDirectories(string path)
        {
            var files = new List<FileInfo>();
            var approvedFiles = new DirectoryInfo(path).GetFiles()
                .Where(x => Constants.ApprovedExtensions.Contains(x.Extension)).ToList();
            approvedFiles.ForEach(x => files.Add(x));
            Directory.GetDirectories(path).ToList().ForEach(dir =>
            {
                var subdirFiles = SearchDirectories(dir);
                subdirFiles.ForEach(x =>
                {
                    _logger.Information("IMAGE FOUND: {image}", x.Name);
                    files.Add(x);
                });
            }) ;
            return files;
        }

        private static object CreateImageInfoFromFiles(List<FileInfo> imageFiles)
        {
            var images = imageFiles.Select(x =>
            {
                try
                {
                    var img = Image.FromFile(x.FullName);
                    return new SimpleImage()
                    {
                        FilePath = Path.Combine(_args.ProvidedPath, Path.GetRelativePath(_args.BasePath, x.FullName)),
                        Name = x.Name,
                        Width = img.Width,
                        Height = img.Height,
                        WidthRes = img.HorizontalResolution,
                        HeightRes = img.VerticalResolution,
                        Topic = x.Directory.Name
                    };
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception Throw on image: {x}");
                    Console.WriteLine(ex);
                    throw;
                }
                

            }).ToList();
            return images;
        }
    }
}
