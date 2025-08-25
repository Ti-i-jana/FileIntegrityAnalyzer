# Investigating File Integrity in Microsoft OneDrive Using the Graph API


## Summary

This project conducts an experiment using the Microsoft Graph API  to validate the data integrity during file transfer to Microsoft OneDrive.
It implements a C# console application that uploads and downloads files from OneDrive and verifies whether the retrieved files are identical to the originals using SHA-256 hashes.

## Install and Run

1. Clone the repository:
```bash
git clone https://github.com/Ti-i-jana/FileIntegrityAnalyzer.git
cd FileIntegrityAnalyzer
```
2. Restore dependencies
```bash
dotnet restore
```
3. Build Project
```bash
dotnet build
```
4. Configure path values in appsettings.json:

5. Configure ClientId and TenantId in a secrets.json file according to the secrets.json.example

6. Run app
```bash
dotnet run --project .\FileIntegrityAnalyzer.csproj
```

## Project Structure

```bash
FileIntegrityAnalyzer
├── docs/                            # Documentation
│   └── LabNotes.pdf				# Lab notes
├── src/                             # Source code
│   ├── Program.cs                   # Main entry point
│   ├── AuthService.cs               # Handles authentication and Graph API client
│   ├── Uploader.cs                  # Handles file uploads
│   ├── Downloader.cs                # Handles file downloads
│   └── IntegrityVerifier.cs         # Verifies file integrity
├── appsettings.json                 # Configuration file for paths
├── FileIntegrityAnalyzer.sln        # Visual Studio solution file
├── FileIntegrityAnalyzer.csproj     # Project file
├── README.md                        # Project README
└── .gitignore                       # Git ignore file
```

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

## Analysis
The experiments included uploading and downloading a txt, pdf, jpg and mp4 file of different sizes.
The hashes matched in all conducted experiments except for the mp4 file which threw an exception of a timeout in the request to upload the file.
The differences in the different files included the time taken to upload and download the files which depended on the file size, and number of api requests(like to create a folder in OneDrive to store the file) before the actual api request to upload/download.


# Report

- The hypothesis was confirmed which means that OneDrive preserves the integrity of the data. This was concluded by running multiple experiments by uploading and downloading a file from OneDrive and checking their SHA256 hashes which matched in all cases.

- One challenge was managing authentication when the app registration was created with a different account which was resolved by using a multi-tenant app registration. Another Challenge was enabling the public client flows in the app registration which had to be mended in the App Registration manifest directly. Another challenge is the cancelled request due to the limit of the HttpClient.Timeout which is 100s. 

- Future work would include changing the HttpClient.Timeout so that requests don't timeout for larger files or for slow connection. 
Also testing out parallel uploads and downloads.