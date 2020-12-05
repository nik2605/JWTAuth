using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CookieAuthentication.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly string _username = "test";
        private readonly string _password;

        public AccountController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _username = "nadhaduk";
            _password = "password";
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    throw new Exception($"{nameof(model.Username)} cannot be null or empty");
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    throw new Exception($"{nameof(model.Password)} cannot be null or empty");
                }

                var valid = await Task.FromResult(string.Equals(model.Username, _username, StringComparison.CurrentCultureIgnoreCase));

                if (valid)
                {
                    valid = await Task.FromResult(string.Equals(model.Password, _password, StringComparison.CurrentCultureIgnoreCase));
                }

                if (valid)
                {
                    string guid = Guid.NewGuid().ToString();

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, _username),
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim(ClaimTypes.NameIdentifier, guid)
                    };

                    var claimIdentity = new ClaimsIdentity(claims, AuthenticationSchema.DefaultAuthenticateScheme);
                    var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                    await HttpContext.SignInAsync(AuthenticationSchema.DefaultAuthenticateScheme, claimPrincipal, new AuthenticationProperties(){ IsPersistent = false} );
                }
                else
                {
                    return BadRequest("Username and Password is not recorgnized");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
