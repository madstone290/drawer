using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Drawer.IntergrationTest
{
    /// <summary>
    /// Api 서버 인스턴스
    /// </summary>
    public class ApiInstance : IDisposable
    {
        public HttpClient Client { get; }

        public ApiInstance()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(webHostBuilder =>
                {
                    webHostBuilder.ConfigureTestServices(services =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Fatal()
                            .CreateLogger();

                        services.RemoveAll(typeof(DbContextOptions<DrawerDbContext>));

                        var jsonString = File.ReadAllText("Secrets/drawer_test_db_secret.json");
                        var jObj = JObject.Parse(jsonString);
                        var connectionString = jObj["DrawerTestDb"]["ConnectionString"].ToString();

                        services.AddDbContext<DrawerDbContext>(options =>
                        {
                            options.UseNpgsql(connectionString);
                        });

                        var scope = services.BuildServiceProvider().CreateScope();
                        SeedManager.ClearDatabase(scope).GetAwaiter().GetResult();
                        SeedManager.UsingScopeAsync(scope).GetAwaiter().GetResult();
                    });
                });

            Client = factory.CreateClient();
            Client.BaseAddress = new Uri(Client.BaseAddress!.AbsoluteUri + "api/");

            SeedManager.UsingApiAsync(Client).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
        }
    }
}