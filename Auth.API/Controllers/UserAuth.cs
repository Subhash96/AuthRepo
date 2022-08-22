using Auth.API.Models;
using Auth.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuth : ControllerBase
    {
        private IUserRepository _userRepo;

        public UserAuth(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model)
        {
            var user = _userRepo.Authentication(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "username or password is incorrect" });
            }
            user.Password = "";
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] LoginModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Uaername already exists" });
            }
            var user = _userRepo.Register(model.Username, model.Password);
            if(user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }
            return Ok(user);
        }





    }
}
