using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TindevApp.Backend.Infrastructure.HealthChecks
{
    public class GithubApiHealthCheck : IHealthCheck
    {
        private readonly GithubApiHealthCheckOptions _options;

        private readonly ILogger<GithubApiHealthCheck> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public GithubApiHealthCheck(IOptions<GithubApiHealthCheckOptions> options, ILogger<GithubApiHealthCheck> logger, IHttpClientFactory httpClientFactory)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var defaultTimeout = TimeSpan.FromSeconds(10);
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, description: $"{nameof(GithubApiHealthCheck)} execution is cancelled.");
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = _options.Uri;
                httpClient.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/"))
                using (var timeoutSource = new CancellationTokenSource(defaultTimeout))
                using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(timeoutSource.Token, cancellationToken))
                {
                    requestMessage.Headers.Add("Authorization", $"token {_options.Token}");

                    var response = await httpClient.SendAsync(requestMessage, linkedSource.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        return new HealthCheckResult(context.Registration.FailureStatus, description: $"Discover endpoint is not responding with code {HttpStatusCode.OK}, the current status is {response.StatusCode}.");
                    }
                }
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}
