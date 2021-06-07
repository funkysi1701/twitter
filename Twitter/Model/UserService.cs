using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Twitter.Interfaces;

namespace Twitter.Model
{
    public class UserService : IUserService
    {
        private readonly AuthContext _context;
        private readonly string _secret;

        public UserService(AuthContext context, IOptions<ConfigOptions> appSettings)
        {
            _secret = appSettings.Value.Secret;
            _context = context;
        }

        public User Authenticate(string username, string password, string env)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == username && x.Password == password && x.Env == env);
            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.TokenExpires = tokenDescriptor.Expires;
            // remove password before returning
            user.Password = null;

            return user;
        }
    }
}
