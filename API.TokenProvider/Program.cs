using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace API.TokenProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            const string issuer = "SkynetAPI";
            const string audience = "SkynetAPI.API.Client";
            const string signInKey = "mVUJiNwsMXVAfPEJCc8EXrkrPLD21e60";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "UserManagement"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var token = new JwtSecurityToken(issuer, audience, claims, signingCredentials: signingCredentials);

            Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
