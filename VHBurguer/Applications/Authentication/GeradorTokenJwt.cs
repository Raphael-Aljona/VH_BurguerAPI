using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Authentication
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;

        public GeradorTokenJwt(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(Usuario usuario)
        {
            // KEY -> chave secreta usada para assinar o token, garante que o token não foi alterado
            var key = _config["Jwt:Key"]!;

            // ISSUER -> quem gerou o token (nome da API / sistema que gerou)
            // a API valida se o token veio do emissor correto
            var issuer = _config["Jwt:Issuer"]!;

            // AUDIENCE -> para quem o token foi criado
            // define qual sistema pode usar o token
            var audience = _config["Jwt:Audience"]!;

            // TEMPO DE EXPIRAÇÃO -> define quantos minutos o token será válido
            // depois disso, o usuário precisa logar novamente.
            var expiraEmMinutos = int.Parse(_config["Jwt:ExpiraEmMinutos"]!);

            // Converte para bytes
            var keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length < 32) throw new DomainException("Token inválido");

            // Cria a chave de segurança
            var securityKey = new SymmetricSecurityKey(keyBytes);

            // Define o altoritmo de assinatura do token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims -> Informações do usuário que vão dentro do Token
            // essas informações podem ser recuperadas na API para identificar quem está logado
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiraEmMinutos),
                signingCredentials: credentials // assinatura de segurança
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
