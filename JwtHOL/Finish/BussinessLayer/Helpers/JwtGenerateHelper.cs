using BussinessLayer.Factories;
using CommonDomainLayer.Configurations;
using DataTransferObjects.Dtos;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Helpers
{
    public class JwtGenerateHelper
    {
        private readonly JwtConfiguration jwtConfiguration;

        public JwtGenerateHelper(IOptions<JwtConfiguration> jwtConfiguration)
        {
            this.jwtConfiguration = jwtConfiguration.Value;
        }
        public string GenerateAccessToken(MyUser user, List<Claim> claims)
        {
            var token = new JwtSecurityToken
            (
                issuer: jwtConfiguration.ValidIssuer,
                audience: jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtConfiguration.JwtExpireMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(jwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512)
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public string GenerateRefreshToken(MyUser user, List<Claim> claims)
        {
            var token = new JwtSecurityToken
            (
                issuer: jwtConfiguration.ValidIssuer,
                audience: jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwtConfiguration.JwtRefreshExpireDays),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(jwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512)
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
