using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;

namespace TindevApp.Backend.Services
{
    public class HttpGithubService : IGithubService
    {
        private readonly HttpClient _client;

        private readonly ILogger<HttpGithubService> _logger;

        public HttpGithubService(HttpClient client, ILogger<HttpGithubService> logger)
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

                try
                {
                    var result = await _client.SendAsync(request, cancellationToken);

                    if (!result.IsSuccessStatusCode)
                        throw new Exception("Error when trying to locate user");

                    var contentResult = await result.Content.ReadAsStringAsync();

                    JObject o = JObject.Parse(contentResult);

                    return new Developer
                    {
                        AvatarUri = (string)o["avatar_url"],
                        Bio = (string)o["bio"],
                        CreatedAt = DateTime.MinValue,
                        GithubUri = (string)o["html_url"],
                        Name = (string)o["name"],
                        Username = (string)o["login"],
                        Id = (int)o["id"]
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when getting user {username} from GitHub from URI {URI}", username, _client.BaseAddress.ToString());
                    throw;
                }
            }
        }
    }
}
