using BLL.Login;
using BOL.Login;
using BOL.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.ApiLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly ILogin _iLogin;

        public AuthenticationController(ILogin login)
        {
            _iLogin = login;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginInputDTO loginInput)
        {
            string jwt = await _iLogin.Login(loginInput);
            if (jwt == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            return StatusCode(StatusCodes.Status200OK, new { token = jwt });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserInputDTO userInput)
        {

            UserOutputDTO user = await _iLogin.Register(userInput);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return StatusCode(StatusCodes.Status200OK, new { user });
        }

    }
}
