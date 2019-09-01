using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TindevApp.Backend.Domains;
using TindevApp.Backend.Models;
using TindevApp.Backend.Repositories;

namespace TindevApp.Backend.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly JwtUserOptions _jwtUserOptions;

        private readonly ILogger<UserService> _logger;

        private readonly DeveloperDomain _developerDomain;

        public UserService(IOptions<JwtUserOptions> jwtUserOptions, ILogger<UserService> logger, DeveloperDomain developerDomain)
        {
            _jwtUserOptions = jwtUserOptions?.Value ?? throw new ArgumentNullException(nameof(jwtUserOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _developerDomain = developerDomain ?? throw new ArgumentNullException(nameof(developerDomain));
        }

        public async Task<User> Authenticate(string username, CancellationToken cancellationToken = default)
        {
            var developer = await _developerDomain.CreateOrUpdateDeveloper(username, cancellationToken)
                .ConfigureAwait(false);

            if (developer == null)
            {
                _logger.LogInformation("User not found. username: {username}", username);
                return null;
            }

            await _developerDomain.UpdateDeveloperFollowers(developer.Username, cancellationToken)
                .ConfigureAwait(false);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = _jwtUserOptions.ByteSecret;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, developer.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string textToken = tokenHandler.WriteToken(token);

            var user = new User
            {
                Id = developer.Id,
                Token = textToken,
                Username = username
            };
            return user;
        }
    }
}
