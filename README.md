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