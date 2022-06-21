using Drawer.Domain.Models.Authentication;
using Drawer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
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
                        services.RemoveAll(typeof(DbContextOptions<DrawerIdentityDbContext>));

                        var jsonString = File.ReadAllText("Secrets/drawer_identity_db_secret.json");
                        var jObj = JObject.Parse(jsonString);
                        var connectionString = jObj["DrawerIdentityDb"]["ConnectionString"].ToString();

                        services.AddDbContext<DrawerIdentityDbContext>(options =>
                        {
                            options.UseNpgsql(connectionString);
                        });

                        var scope = services.BuildServiceProvider().CreateScope();
                        SeedData.Initialize(scope);
                    });
                });

            Client = factory.CreateClient();
            Client.BaseAddress = new Uri(Client.BaseAddress!.AbsoluteUri + "api/");


        }

        public void Dispose()
        {
        }
    }
}