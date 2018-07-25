using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelloNetcore.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace HelloNetcore.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IssuedClaims = await GetClaimsFromUserAsync(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = user != null;
        }

        private async Task<List<Claim>> GetClaimsFromUserAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
            if (!string.IsNullOrEmpty(user.Avator))
            {
                claims.Add(new Claim("avator", user.Avator));
            }

            return claims;
        }

    }
}
