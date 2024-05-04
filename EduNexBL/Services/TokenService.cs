using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationMechanism.tokenservice
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateAccessToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); // Fetch the user
            if (user == null)
                return null; // User not found

            var role = await _userManager.GetRolesAsync(user); // Fetch the user's role
            if (role == null || role.Count == 0)
                return null; // Role not found or user has no role

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:5293/"),
                new Claim(JwtRegisteredClaimNames.Iss, "http://localhost:5293/"),// Set the audience claim

                new Claim(ClaimTypes.Role, role[0]) // Add the user's role claim
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("D6AE1F1B3F45D32D2E18B5E9F1D301298C1C87223578F8D063DAC8E2E255971B");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using (var rng = new RNGCryptoServiceProvider())
        //    {
        //        rng.GetBytes(randomNumber);
        //        return Convert.ToBase64String(randomNumber);
        //    }
        //}

    }
}
