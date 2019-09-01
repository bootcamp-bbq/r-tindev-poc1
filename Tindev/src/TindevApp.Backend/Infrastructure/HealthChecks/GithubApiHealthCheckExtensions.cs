using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using TindevApp.Backend.Infrastructure.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GithubApiHealthCheckExtensions
    {
        const string NAME = "github-group";

        public static IHealthChecksBuilder AddGithub(this IHealthChecksBuilder builder,
            Action<GithubApiHealthCheckOptions> uriOptions,
            string name = default,
            HealthStatus? failureStatus = default,
            IEnumerable<string> tags = default)
        {
            builder.Services.AddHttpClient();

            builder.Services.Configure(uriOptions);

            builder.Services.AddScoped<GithubApiHealthCheck>();

            return builder.Add(new HealthCheckRegistration
                (
                    name ?? NAME,
                    sp => sp.GetRequiredService<GithubApiHealthCheck>(),
                    failureStatus,
                    tags
                ));
        }
    }
}
