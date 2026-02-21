using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
namespace auDASLib
{
    public class ZipHelper
    {
        public static void ExportZipArchive(string sourceDirectory, string zipFilePath)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                throw new DirectoryNotFoundException($"Source directory '{sourceDirectory}' not found.");
            }

            ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath, CompressionLevel.Fastest, false);
   
           // Console.WriteLine($"Zip archive '{zipFilePath}' created successfully.");
        }

        public static void ImportZipArchive(string zipFilePath, string destinationDirectory)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException($"Zip file '{zipFilePath}' not found.");
            }

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            ZipFile.ExtractToDirectory(zipFilePath, destinationDirectory,true);
           // Console.WriteLine($"Zip archive '{zipFilePath}' extracted to '{destinationDirectory}' successfully.");
        }
    }
}
