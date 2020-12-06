using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageJsonGenerator
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            args = new string[] { @"C://home-dev/stephenwike.com/src/StephenWikeWeb/src/assets/images",
            "C://home-dev/stephenwike.com/src/StephenWikeWeb/src/app/components/my-art/my-art.component.json"};

            if (args.Length != 2)
            {
                _logger.Error(@"Incorrect number of arguments. imagejsongenerator <image-base-dir> <out-put-file>");
                return;
            }
            var imagesPath = args[0];
            _logger.Information("Images Base Path: {imagesPath}", imagesPath);
            var outputFile = args[1];
            _logger.Information("Output File: {outputFile}", outputFile);
            

            var imageFiles = SearchDirectories(imagesPath);

            var imageInfos = CreateImageInfoFromFiles(imageFiles);

            var imageString = JsonConvert.SerializeObject(imageInfos, Formatting.Indented);

            System.IO.File.WriteAllText(outputFile, imageString);
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
                var img = Image.FromFile(x.FullName);

                return new SimpleImage()
                {
                    FileName = x.FullName,
                    Name = x.Name,
                    Width = img.Width,
                    Height = img.Height,
                    WidthRes = img.HorizontalResolution,
                    HeightRes = img.VerticalResolution
                };

            }).ToList();
            return images;
        }
    }
}
