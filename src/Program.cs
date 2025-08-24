using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

class Program
{
    static async Task Main(string[] args)
    {

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string sourcePath = config["Paths:SourceFile"]; // Source file to be uploaded 
        string folderName = config["OneDrive:FolderName"];
        string destinationPath = config["Paths:DestinationFile"];
        
        var graphClient = await AuthService.GetGraphServiceClient();
        var uploader = new Uploader(graphClient);
        var downloader=new Downloader(graphClient);

        await uploader.UploadFileAsync(sourcePath, folderName);
        await downloader.DownloadFileAsync($"{folderName}/{Path.GetFileName(sourcePath)}",destinationPath);

        var uploadedHash=IntegrityVerifier.ComputeSHA256Hash(sourcePath);
        var downloadedHash = IntegrityVerifier.ComputeSHA256Hash(destinationPath);

        if(IntegrityVerifier.CompareSHA256Hash(uploadedHash, downloadedHash)){
            Console.WriteLine("Hashes match");
        }
        else
        {
            Console.WriteLine("Hashes don't match");
        }

    }
}