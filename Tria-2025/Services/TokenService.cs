using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Tria_2025.Models;
using Tria_2025.Interface;

namespace Tria_2025.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        // Banco simulado em memória (pode ser substituído por EF depois)
        private static readonly List<Funcionario> _users = new()
        {
            new Funcionario
            {
                Id = 1,
                Nome = "Admin",
                Cargo = "Administrador",
                Email = "admin@tria.com",
                Senha = BCrypt.Net.BCrypt.HashPassword("123456")
            }
        };

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // LOGIN por email
        public Task<string> LoginAndGenerateTokenAsync(string email, string senha)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null || !BCrypt.Net.BCrypt.Verify(senha, user.Senha))
                return Task.FromResult(string.Empty);

            var token = GenerateToken(user.Id, user.Nome, user.Cargo);
            return Task.FromResult(token);
        }

        // REGISTRO de novo funcionário
        public Task<bool> RegisterUserAsync(string nome, string email, string senha, string cargo)
        {
            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return Task.FromResult(false);

            var newUser = new Funcionario
            {
                Id = _users.Max(u => u.Id) + 1,
                Nome = nome,
                Email = email,
                Senha = BCrypt.Net.BCrypt.HashPassword(senha),
                Cargo = cargo
            };

            _users.Add(newUser);
            return Task.FromResult(true);
        }

        // GERAÇÃO do token JWT
        public string GenerateToken(int userId, string nome, string cargo)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not found.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, cargo)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
