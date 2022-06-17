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

namespace Drawer.IntergrationTest
{
    /// <summary>
    /// Api 서버 인스턴스
    /// </summary>
    public class ApiInstance 
    {
        public HttpClient Client { get; set; }

        public ApiInstance()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(webHostBuilder =>
                {

                    webHostBuilder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DrawerIdentityDbContext>));

                        var jsonString = File.ReadAllText("Secrets/draw_identity_db_secret.json");
                        var jObj = JObject.Parse(jsonString);
                        var connectionString = jObj["DrawerIdentityDb"]["ConnectionString"].ToString();

                        services.AddDbContext<DrawerIdentityDbContext>(options =>
                        {
                            options.UseNpgsql(connectionString);
                        });

                        var scope = services.BuildServiceProvider().CreateScope();
                        var dbContext = scope.ServiceProvider.GetService<DrawerIdentityDbContext>();
                        dbContext?.Database.Migrate();

                    });
                });

            Client = factory.CreateClient();
            Client.BaseAddress = new Uri(Client.BaseAddress!.AbsoluteUri + "api/");
        }
    }
}