using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Blazor.Models;

namespace TindevApp.Blazor.Data
{
    public class DevelopersApi
    {
        private readonly HttpClient httpClient;

        public DevelopersApi(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this.httpClient.BaseAddress = new Uri(@"https://rmauro-tindev-api.herokuapp.com/");
        }

        public async Task<string> LoginAsync(string username, CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Post, "/Users/authenticate"))
            {
                var @params = new List<KeyValuePair<string, string>>();
                @params.Add(new KeyValuePair<string, string>("username", username));

                rq.Content = new FormUrlEncodedContent(@params);

                var response = await this.httpClient.SendAsync(rq, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var jO = JObject.Parse(content);

                    return jO["token"].Value<string>();
                }
                return string.Empty;
            }
        }

        public async Task<List<Developer>> GetDevelopersAsync(string token, CancellationToken cancellationToken = default)
        {
            using (HttpRequestMessage rq = new HttpRequestMessage(HttpMethod.Get, "/devs/friends"))
            {
                rq.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await this.httpClient.SendAsync(rq, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();


                    return JsonConvert.DeserializeObject<DevResponse>(content).Items;
                }
                return Array.Empty<Developer>().ToList();
            }
        }
    }
}
