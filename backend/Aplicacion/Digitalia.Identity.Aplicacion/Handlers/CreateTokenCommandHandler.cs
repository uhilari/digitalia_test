using Digitalia.Identity.Aplicacion.Commands;
using Digitalia.Identity.Dominio;
using Digitalia.Identity.Model;
using HS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Digitalia.Identity.Aplicacion.Handlers
{
    public class CreateTokenCommandHandler(IConfiguration configuration, UserManager<Usuario> userManager) : ICommandHandler<CreateTokenCommand, CreateTokenResponse>
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<Usuario> _userManager = userManager;

        public async Task<CreateTokenResponse> ExecuteAsync(CreateTokenCommand command)
        {
            var usuario = await _userManager.FindByNameAsync(command.Data.Username);
            if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, command.Data.Password))
            {
                throw new NotFoundEntityException("No existe el usuario");
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, usuario.Id.Cadena()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.Email)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return new CreateTokenResponse { Token = jwt };
        }
    }
}
