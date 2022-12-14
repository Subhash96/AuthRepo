using Auth.API.Data;
using Auth.API.Models;
using Auth.API.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;


        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appsettings)
        {
            _db = db;
            _appSettings = appsettings.Value;
        }
        public LoginModel Authentication(string username, string password)
        {
            
            var user = _db.LoginModels.SingleOrDefault(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesciptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;


         }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LoginModels.SingleOrDefault(x => x.Username == username);
            if (user == null)
                return true;
            return false;
        }
        
        public LoginModel Register(string username, string password)
        {
            LoginModel userObj = new LoginModel()
            {
                Username = username,
                Password = password
            };
            _db.LoginModels.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;

        }
    }
}
