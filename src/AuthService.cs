using Microsoft.Graph;
using Azure.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Handles Authentication and returns a GraphServiceClient for accessing Microsoft Graph APIs using device code flow.
/// </summary>
static class AuthService
{
    private static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
            .Build();

    public static string clientID = config["AzureAd:ClientId"];
    public static string tenantID = config["AzureAd:TenantId"];
    public static string[] scopes = new string[] {"User.Read","Files.ReadWrite"};

    /// <summary>
    /// Creates and returns an authenticated GraphServiceClient 
    /// using device code authentication which prints the device code URL to the console.
    /// </summary>
    /// <returns>
    /// An authenticated GraphServiceClient 
    /// with the specified clientID,tenantID and scopes ready to make Microsoft Graph API calls.
    /// </returns>
    public static async Task<GraphServiceClient> GetGraphServiceClient()
    {
        var options = new DeviceCodeCredentialOptions
        {
            ClientId = clientID,
            TenantId = tenantID,
            DeviceCodeCallback = (code, cancellation) =>
            {
                Console.WriteLine(code.Message);
                return Task.FromResult(0);
            }
        };

        var deviceCodeCredential = new DeviceCodeCredential(options);

        var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

        return graphClient;
    }

}
