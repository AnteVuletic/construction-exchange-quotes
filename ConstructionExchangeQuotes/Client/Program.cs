using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConstructionExchangeQuotes.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace ConstructionExchangeQuotes.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<PermissionCacheService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();

            await builder.Build().RunAsync();
        }
    }
}
