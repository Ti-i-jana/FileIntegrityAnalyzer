# Investigating File Integrity in Microsoft OneDrive Using the Graph API


## Summary

This project conducts an experiment using the Microsoft Graph API  to validate the data integrity during file transfer to Microsoft OneDrive.
It implements a C# console application that uploads and downloads files from OneDrive and verifies whether the retrieved files are identical to the originals using SHA-256 hashes.

## Project Structure

FileIntegrityAnalyzer/

│   FileIntegrityAnalyzer.sln

│   FileIntegrityAnalyzer.csproj

│   README.md

│

├───docs/  

│

└───src/            

    ├── Program.cs

    ├── AuthService.cs


## AuthService 
This helper class handles authentication with Microsoft Graph. It does so by using Device Code Flow which shows a code  in the console for login.
It returns a ready to use GraphServiceClient for calling Microsoft Graph APIs.

## Uploader
The Uploader class handles uploading files to a specified folder in the authenticated user's OneDrive. It also handles the creation of the folder if it doesn't exist.

### Key methods:

#### InitializeMyDriveAsync()
Gets the signed-in user’s OneDrive ID and stores it for use in other calls.

#### CreateFolderAsync(string folderName)
Checks if the folder exists in the user’s OneDrive root.  
If it doesn’t exist, creates the folder.

#### UploadFileAsync(string filePath, string folderName)
Ensures the destination folder exists. Uploads the file to the specified folder. Logs time taken to upload and any errors.