using Microsoft.Graph;                  
using Microsoft.Graph.Models;
using System;
using System.IO;                    
using System.Threading.Tasks;

/// <summary>
/// Handles downloading files from an authenticated user’s OneDrive to a local directory.
/// </summary>
public class Downloader
{
    private GraphServiceClient _graphClient;
    private string _myDriveId;

    /// <summary>
    /// Initializes a new instance of the Downloader class.
    /// </summary>
    /// <param name="graphClient">An authenticated GraphServiceClient instance.</param>
    public Downloader(GraphServiceClient graphClient)
    {
        _graphClient = graphClient;
    }


    /// <summary>
    /// Get and store the authenticated user's OneDrive ID used in other methods
    /// </summary>
    public async Task InitializeMyDriveAsync()
    {
        try { 
            var myDrive = await _graphClient.Me.Drive.GetAsync();
            _myDriveId = myDrive.Id;
        }
        catch(ServiceException ex)
        {
            //Microsoft Graph API call fail
            Console.WriteLine($"ServiceException {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }


    }

    /// <summary>
    /// Downloads a file from OneDrive to the specified destination path.
    /// Creates the destination folder if it does not exist.
    /// Logs any Microsoft Graph API Call fails and other exceptions.
    /// </summary>
    /// <param name="srcPath">The path of the file on OneDrive.</param>
    /// <param name="destPath">The destination file path.</param>
    public async Task DownloadFileAsync(string srcPath, string destPath)
    {
        await InitializeMyDriveAsync();
        string directoryPath = Path.GetDirectoryName(destPath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath)) {
            Console.WriteLine("Destination folder doesn't exist. Creating directory now..");
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            using var fileStream = await _graphClient.Drives[_myDriveId].Root.ItemWithPath(srcPath).Content.GetAsync(); //downloads file's data from OneDrive
            using var dstfileStream= new FileStream(destPath, FileMode.Create); // create a file at the specified path
            await fileStream.CopyToAsync(dstfileStream);
            Console.WriteLine("Copied file from OneDrive to local dir");

        }
        catch (ServiceException ex)
        {
            //Microsoft Graph API call fail
            Console.WriteLine($"ServiceException {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }

    }
}