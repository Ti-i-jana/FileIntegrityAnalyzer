using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Graph.Drives.Item.Items.Item;
using Microsoft.Kiota.Abstractions;

/// <summary>
/// Upload file to OneDrive folder using Microsoft Graph API and log time taken to upload file and any errors.
/// </summary>
public class Uploader
{
    private GraphServiceClient _graphClient;
    private string _myDriveId;
    
    public Uploader(GraphServiceClient graphClient)
    {
        _graphClient = graphClient;

    }

    /// <summary>
    /// Get and store the authenticated user's OneDrive ID used in other methods
    /// </summary>
    public async Task InitializeMyDriveAsync()
    {
        try
        {
            var myDrive = await _graphClient.Me.Drive.GetAsync();
            _myDriveId = myDrive.Id;
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

    /// <summary>
    /// Creates folder in user's OneDrive root if it doesn't exist
    /// <param name="folderName">Name of folder to check if exists or create in root of OneDrive.</param>
    /// </summary>
    public async Task CreateFolderAsync(string folderName)
    {
        if (string.IsNullOrEmpty(_myDriveId))
        {
            await InitializeMyDriveAsync();
        }

        try
        {   
            await _graphClient.Drives[_myDriveId].Root.ItemWithPath(folderName).GetAsync(); //checks if folder exists in root in drive
            Console.WriteLine($"Folder {folderName} already exists");
        }

        catch (Exception ex) when (ex.Message.Contains("Item does not exist", StringComparison.OrdinalIgnoreCase)) //Kiota throws plain exception instead of serviceexpecption when item does not exist 
        {

            Console.WriteLine($"Folder {folderName} doesn't exist creating now..");
            var newFolder = new DriveItem
            {
                Name = folderName,
                Folder = new Folder(),
                AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "rename" }
                }
            };
            try
            {
                var _root = await _graphClient.Drives[_myDriveId].Root.GetAsync();
                var _rootId = _root.Id; // get ID of root folder
                await _graphClient.Drives[_myDriveId].Items[_rootId].Children.PostAsync(newFolder); // creates new folder in root
                Console.WriteLine($"Created Folder {folderName}");
            }
            catch(ServiceException innerex)
            {
                //Microsoft Graph API Call fail
                Console.WriteLine($"ServiceException {innerex.Message}");
                Console.WriteLine($"StackTrace: {innerex.StackTrace}");
            }
            catch(Exception innerex)
            {
                Console.WriteLine($"Exception {innerex.Message}");
                Console.WriteLine($"StackTrace: {innerex.StackTrace}");
            }

        }
    }

    /// <summary>
    /// Creates the folder the file should be stored in if it doesn't exist. Uploads the file to the OneDrive folder.
    /// Logs time taken to upload file and any Microsoft Graph API call falls as well as runtime errors.
    /// </summary>
    /// <param name="filePath">Path of file to upload.</param>
    /// <param name="folderName">The destination folder in OneDrive where the file will be uploaded.</param>

    public async Task UploadFileAsync(string filePath, string folderName)
    {
        var elapsedTime = Stopwatch.StartNew();

        try
        {

            await CreateFolderAsync(folderName);
            using var fileStream = File.OpenRead(filePath);

            await _graphClient.Drives[_myDriveId].Root.ItemWithPath($"{folderName}/{Path.GetFileName(filePath)}").Content.PutAsync(fileStream); //uploads file to specified folder at specified path

            Console.WriteLine("Finished upload");
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
        finally
        {
            elapsedTime.Stop();
            Console.WriteLine($"Elapsed time: {elapsedTime.ElapsedMilliseconds} ms");
        }

    }


}