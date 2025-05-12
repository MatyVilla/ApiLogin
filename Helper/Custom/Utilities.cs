using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using DAL.DBAccess.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace Helper.Custom
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;
        private readonly DbaccessContext _dbaccessContext;

        public Utilities(IConfiguration configuration,DbaccessContext dbaccessContext)
        {
            _configuration = configuration;
            _dbaccessContext = dbaccessContext;
        }

        public string encryptedSHA256(string pass)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));

            StringBuilder builder = new StringBuilder();

            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        public string createJWT(User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()));
            //claims.Add(new Claim(ClaimTypes.Email, user.Email!));
            //claims.Add(new Claim(ClaimTypes.Name, user.Name!));
            //claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone!));
            //claims.Add(new Claim("Rut", user.Rut!));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name!));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var expiration = DateTime.UtcNow.AddMinutes(30);
            var securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return jwt;
        }
        public async Task<User?> ValidateJWT(ClaimsIdentity? claimsIdentity)
        {
            try
            {
                if (!claimsIdentity.Claims.Any())
                {
                    return null;
                }
                var idUser = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "name");

                User? user = await _dbaccessContext.Users.Where(u => u.IdUser.Equals(idUser)).Include(r => r.Role).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception e)
            {
                Log.Error($"Error ValidateJWT[01]: {e}");
                throw new Exception(e.Message);
            }
        }
    }
}
