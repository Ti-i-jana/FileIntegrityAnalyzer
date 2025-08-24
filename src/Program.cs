using System;
using System.Threading.Tasks;
using Microsoft.Graph;

class Program
{
    static async Task Main(string[] args)
    {
        var graphClient = await AuthService.GetGraphServiceClient();
        var uploader = new Uploader(graphClient);
        string filePath = "C:\\Users\\Tijana\\Desktop\\test.txt";
        string folderName = "testFolder";
        await uploader.UploadFileAsync(filePath, folderName);



        //TODO: delete when everything works
        var me = await graphClient.Me.GetAsync();
        Console.WriteLine($"Hello, {me.DisplayName} ({me.UserPrincipalName})");
    }
}