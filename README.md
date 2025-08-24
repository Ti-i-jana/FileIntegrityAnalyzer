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
    
    ├── Uploader.cs

    ├── Downloader.cs

    ├── IntegrityVerifier.cs


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

## Downloader
The Downloader class handles downloading files from a user's OneDrive to a local destination path.

### Key methods:

#### InitializeMyDriveAsync()
Gets the signed-in user’s OneDrive ID and stores it for use in other calls.

#### DownloadFileAsync(string srcPath, string destPath)
Checks if the destination directory exists, if not creates it. Downloads the file in the specified srcPath from the user's OneDrive to the specified local destination path.

## IntegrityVerifier
The IntegrityVerifier class is a utility class that handles computing the SHA256 hash of a file and compares hashes of two files.

### Key methods:

#### ComputeSHA256Hash(string filepath)
Computes the SHA256 hash for the specified file and returns a byte array representing the hash.

#### CompareSHA256Hash(byte[] uploadFile, byte[] downloadFile)
Compares the hashes of two files and returns a boolean value.