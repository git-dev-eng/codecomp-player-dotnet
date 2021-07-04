using codecomp.player.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace codecomp.player
{
    public class ApiHandler : IApiHandler
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ILogger<Bot> _logger;

        public ApiHandler(IOptions<AppSettings> config, ILogger<Bot> logger, HttpClient httpClient)
        {
            if (config == null) throw new ArgumentNullException("config");
            _logger = logger ?? throw new ArgumentNullException("logger");
            _httpClient = httpClient ?? throw new ArgumentNullException("httpClient");

            _appSettings = config.Value;
        }

        public async Task<GameStatus> GetGameStatus()
        {
            GameStatus status = null;
            using (var res = await _httpClient.GetAsync($"{_appSettings.ApiBase}/gamestatus")) //Fetching current game status
            {
                res.EnsureSuccessStatusCode();

                using (var content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(data);
                        json.data.secretLength = 5; // Please remove this, it's a fix not receiving secret length from controller
                        string fetchData = JsonConvert.SerializeObject(json.data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                        status = JsonConvert.DeserializeObject<GameStatus>(fetchData);
                    }
                }
            }

            return status;
        }

        public async Task<JoinResponse> Join()
        {
            JoinResponse response = null;
            using (var res = await _httpClient.PostAsync($"{_appSettings.ApiBase}/join", new StringContent(string.Empty, Encoding.UTF8, "application/json")))
            {
                res.EnsureSuccessStatusCode();

                using (var content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(data);
                        json.data.errorMessage = json.err;
                        string fetchData = JsonConvert.SerializeObject(json.data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                        response = JsonConvert.DeserializeObject<JoinResponse>(fetchData);
                    }
                }

            }

            return response;
        }

        public async Task<MyActionResponse> Action(MyAction action)
        {
            MyActionResponse response = null;

            using (var res = await _httpClient.PostAsync($"{_appSettings.ApiBase}/guess", new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json")))
            {
                res.EnsureSuccessStatusCode();

                using (var content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(data);
                        json.data.error = json.err;
                        json.data.requestId = json.requestId;
                        string fetchData = JsonConvert.SerializeObject(json.data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                        response = JsonConvert.DeserializeObject<MyActionResponse>(fetchData);
                    }
                }

            }

            return response;
        }

    }
}