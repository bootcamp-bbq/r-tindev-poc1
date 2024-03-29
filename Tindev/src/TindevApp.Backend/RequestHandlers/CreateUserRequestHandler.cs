﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Commands;
using TindevApp.Backend.Repositories;
using TindevApp.Backend.Services;

namespace TindevApp.Backend.RequestHandlers
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserCmdRequest, CreateUserCmdResponse>
    {
        private readonly IGithubService _githubService;

        private readonly IDeveloperRepository _developerRepository;

        private readonly ILogger<CreateUserRequestHandler> _logger;

        public CreateUserRequestHandler(IGithubService githubService, IDeveloperRepository developerRepository, ILogger<CreateUserRequestHandler> logger)
        {
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
            _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateUserCmdResponse> Handle(CreateUserCmdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("executing create user request handler");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var githubResult = await _githubService.GetDeveloper(request.Username, cancellationToken);

            var dbDeveloper = await _developerRepository.Create(githubResult, cancellationToken);

            return new CreateUserCmdResponse
            {
                Developer = dbDeveloper
            };
        }
    }
}
