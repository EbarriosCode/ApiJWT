using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiJWT.Models;
using ApiJWT.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiJWT.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Token")]
        public IActionResult Token([FromBody] UserData data)
        {
            if (_authService.ValidateLogin(data.UserName, data.Password))
            {
                var date = DateTime.UtcNow;
                var expireDate = TimeSpan.FromHours(5);
                var expireDateTime = expireDate.Add(expireDate);

                var token = _authService.GenerateToken(date, data.UserName, expireDate);

                return Ok(new
                {
                    Token = token,
                    ExpireAt = expireDateTime
                });
            }

            else
            {
                return StatusCode(401);
            }
        }
    }
}
