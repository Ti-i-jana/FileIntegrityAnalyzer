using System;
using System.IO;
using System.Security.Cryptography;


/// <summary>
/// Utility class that handles computing SHA256 hash for given file and verifies if the hashes of two files match 
/// </summary>

static class IntegrityVerifier
{
    /// <summary>
    /// Computes the SHA-256 hash of a file at the specified path.
    /// </summary>
    /// <param name="filepath">The full path to the file to compute the hash for.</param>
    /// <returns>
    /// A byte array representing the SHA-256 hash of the file,
    /// or null if an exception occurs.
    /// </returns>
    public static byte[] ComputeSHA256Hash(string filepath)
    {
        try
        {
            var sha256 = SHA256.Create();
            using var fileStream = File.OpenRead(filepath);
            var hash = sha256.ComputeHash(fileStream);
            return hash;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception {ex.Message}");
            Console.WriteLine($"StackTrace {ex.StackTrace}");
            return null;
        }
    }

    /// <summary>
    /// Compares two arrays of bytes representing two hashes.
    /// </summary>
    /// <param byte[]="uploadFile">The hash of the file that is to be uploaded to OneDrive</param>
    /// <param byte[]="downloadFile">The hash of the file that is downloaded from OneDrive</param>
    /// <returns>
    /// A bool representing the result of the comparison of the two hashes.
    /// </returns>
    public static bool CompareSHA256Hash(byte[] uploadFile, byte[] downloadFile)
    {
        if (uploadFile == null || downloadFile == null)
        {
            Console.WriteLine("One of the hashes isn't available");
            return false;
        }

        return uploadFile.SequenceEqual(downloadFile);
    }

}