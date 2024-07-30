
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class TokenService
{
    public string GenerateToken(Usuario usuario)
    {
        // Dados do token
        Claim[] claims = new Claim[]
        {
            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id),
            new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString())
        };

        // Gera a chave com base na string
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MUEyQzRFRkYxMjM0NTY3ODlBQkNERUZHSElKS0xNTk8="));

        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // Gera o token
        var token = new JwtSecurityToken(expires: DateTime.Now.AddSeconds(10), claims: claims, signingCredentials: signingCredentials);

        // Converte para string e retorna
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}