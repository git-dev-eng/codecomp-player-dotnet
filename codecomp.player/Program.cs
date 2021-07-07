using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Security;
namespace codecomp.player
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                ConfigureServices(services);
            })
            .UseConsoleLifetime()
            .Build();


            host.Run();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                services.AddOptions();
                services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

                // add configured instance of logging
                services.AddLogging(cfg => cfg.AddConsole()).Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information);

                // add services
                services.AddScoped<HttpClient>(factory =>
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;

                    var httpClient = new HttpClient(clientHandler);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
                    //Setup basic auth
                    var settings = factory.GetService<IOptions<AppSettings>>().Value;

                    var myTeam = Environment.GetEnvironmentVariable("Team");
                    var myPassword = Environment.GetEnvironmentVariable("Password");

                    if (!string.IsNullOrWhiteSpace(myTeam)) settings.Team = myTeam;
                    if (!string.IsNullOrWhiteSpace(myPassword)) settings.Password = myPassword;

                    var byteArray = Encoding.ASCII.GetBytes($"{settings.Team}:{settings.Password}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    return httpClient;
                });

                services.AddScoped<IApiHandler, ApiHandler>();
                services.AddScoped<IAlgo, Algo>();

                // add app
                services.AddHostedService<Bot>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while configuring");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }

        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {

            return true;
        }
    }
}