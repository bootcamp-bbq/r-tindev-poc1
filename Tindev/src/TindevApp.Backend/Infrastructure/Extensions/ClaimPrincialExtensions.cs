using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend
{
    public static class ClaimPrincialExtensions
    {
        internal static string UserId(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.NameIdentifier);
        }

        internal static string UserName(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.Name);
        }

        private static string GetClaim(ClaimsPrincipal principal, string claimType)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}
