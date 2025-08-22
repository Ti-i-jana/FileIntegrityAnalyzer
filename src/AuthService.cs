using Microsoft.Graph;
using Azure.Identity;
using System.Threading.Tasks;
using System;

static class AuthService
{
    public static string clientID = "9efd73c8-a015-466e-98e1-433173eeff92";
    public static string tenantID = "common";
    public static string[] scopes = new string[] {"User.Read","Files.ReadWrite"};

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
