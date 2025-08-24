using System;
using System.Threading.Tasks;
using Microsoft.Graph;

class Program
{
    static async Task Main(string[] args)
    {
        string filePath = "C:\\Users\\Tijana\\Desktop\\test.txt";
        string folderName = "testFolder";
        string destinationPath = "C:\\Users\\Tijana\\source\\repos\\FileIntegrityAnalyzer\\TestDir\\downloadedtest.txt";
        var graphClient = await AuthService.GetGraphServiceClient();
        var uploader = new Uploader(graphClient);
        var downloader=new Downloader(graphClient);

        await uploader.UploadFileAsync(filePath, folderName);
        await downloader.DownloadFileAsync($"{folderName}/{Path.GetFileName(filePath)}",destinationPath);


        //TODO: delete when everything works
        var me = await graphClient.Me.GetAsync();
        Console.WriteLine($"Hello, {me.DisplayName} ({me.UserPrincipalName})");
    }
}