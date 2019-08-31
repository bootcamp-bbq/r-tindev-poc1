using Hangfire;
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

        internal async Task<bool> AddLike(string currentUsername, string targetUsername, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding like to {TargetUserId} from {CurrentUserId}", targetUsername, currentUsername);

            var targetUserDb = await _developerRepository.GetByUsername(targetUsername, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return false;

            if (targetUserDb.Likes == null)
                targetUserDb.Likes = new List<string>();
            targetUserDb.Likes.Add(currentUsername);

            _ = await _developerRepository.Update(targetUserDb, cancellationToken);

            var currentUserDb = await _developerRepository.GetByUsername(currentUsername, cancellationToken);

            return currentUserDb.Likes != null && currentUserDb.Likes.Contains(targetUsername);
        }

        internal async Task AddDeslike(string currentUserId, string targetUserId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding dislike to {TargetUserId} from {CurrentUserId}", targetUserId, currentUserId);

            var targetUserDb = await _developerRepository.GetById(targetUserId, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;

            if (targetUserDb.Deslikes == null)
                targetUserDb.Deslikes = new List<string>();
            targetUserDb.Deslikes.Add(currentUserId);

            _ = await _developerRepository.Update(targetUserDb, cancellationToken);
        }

        internal async Task<Developer> GetDeveloper(string username, CancellationToken cancellationToken = default)
        {
            var gitDeveloper = await _githubService.GetDeveloper(username, cancellationToken);
            if (gitDeveloper == null)
                return null;

            BackgroundJob.Enqueue<DeveloperDomain>(x => x.UpdateDeveloperFollowers(username, CancellationToken.None));

            var dbDeveloper = await _developerRepository.CreateOrUpdate(gitDeveloper, cancellationToken);

            return dbDeveloper;
        }

        public async Task UpdateDeveloperFollowers(string username, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var devFriends = await GetDeveloperFriends(username, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            List<Task> taskDev = new List<Task>();
            foreach (var dev in devFriends)
            {
                BackgroundJob.Enqueue<DeveloperDomain>(x => x.GetDeveloper(dev.Username, CancellationToken.None));
            }
        }

        internal async Task<IEnumerable<Developer>> GetDeveloperFriends(string username, CancellationToken cancellationToken = default)
        {
            var gitFriendsColl = await _githubService.GetFollowers(username, cancellationToken);
            if (gitFriendsColl == null)
                return null;

            List<Developer> friendsColl = new List<Developer>();
            foreach (var item in gitFriendsColl)
            {
                var dbDeveloper = await _developerRepository.CreateOrUpdate(item, cancellationToken);
                friendsColl.Add(dbDeveloper);
            }

            return await _developerRepository.ListAllExceptInLikeAndDeslike(username, cancellationToken);
        }
    }
}

