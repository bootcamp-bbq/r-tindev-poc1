using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Models;
using TindevApp.Backend.Repositories;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.Domains
{
    public class DeveloperDomain
    {
        private readonly ILogger<DeveloperDomain> _logger;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IGithubService _githubService;

        public DeveloperDomain(ILogger<DeveloperDomain> logger, IDeveloperRepository developerRepository, IGithubService githubService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
        }

        public async Task<Developer> GetDeveloper(string username, CancellationToken cancellationToken = default)
        {
            var dbDeveloper = await _developerRepository.GetByUsername(username, cancellationToken);
            if (dbDeveloper != null)
                return dbDeveloper;

            var gitDeveloper = await _githubService.GetDeveloper(username, cancellationToken);
            if (gitDeveloper == null)
                return null;

            dbDeveloper = await _developerRepository.Create(gitDeveloper, cancellationToken);

            return dbDeveloper;
        }
    }
}
