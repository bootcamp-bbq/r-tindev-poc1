using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Services
{
    public class GithubService
    {
        private readonly HttpClient _client;

        private readonly ILogger<GithubService> _logger;

        public GithubService(HttpClient client, ILogger<GithubService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Developer> GetDeveloper(string username, CancellationToken cancellationToken = default)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"/users/{username}"))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await _client.SendAsync(request, cancellationToken);

                if (!result.IsSuccessStatusCode)
                    throw new Exception("Error when trying to locate user");

                var contentResult = await result.Content.ReadAsStringAsync();

                JObject o = JObject.Parse(contentResult);


                throw new NotImplementedException();
            }
        }
    }
}
