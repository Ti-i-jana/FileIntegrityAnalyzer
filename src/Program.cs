using System;
using System.Threading.Tasks;
using Microsoft.Graph;

class Program
{
    static async Task Main(string[] args)
    {
        var graphClient = await AuthService.GetGraphServiceClient();
        var me = await graphClient.Me.GetAsync();
        Console.WriteLine($"Hello, {me.DisplayName} ({me.UserPrincipalName})");
    }
}