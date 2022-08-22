
using Auth.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        LoginModel Authentication(string username, string password);
        LoginModel Register(string username, string password);
    }
}
