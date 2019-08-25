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
            var gitDeveloper = await _githubService.GetDeveloper(username, cancellationToken);
            if (gitDeveloper == null)
                return null;

            var dbDeveloper = await _developerRepository.CreateOrUpdate(gitDeveloper, cancellationToken);

            return dbDeveloper;
        }

        public async Task<IEnumerable<Developer>> GetDeveloperFrieds(string username, CancellationToken cancellationToken = default)
        {
            var gitFriedsColl = await _githubService.GetFollowers(username, cancellationToken);
            if (gitFriedsColl == null)
                return null;

            List<Developer> friendsColl = new List<Developer>();
            foreach (var item in gitFriedsColl)
            {
                var dbDeveloper = await _developerRepository.CreateOrUpdate(item, cancellationToken);
                friendsColl.Add(dbDeveloper);
            }

            return friendsColl; ;
        }
    }
}
