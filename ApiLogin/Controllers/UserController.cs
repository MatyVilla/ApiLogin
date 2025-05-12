using System.Security.Claims;
using BLL.Interface;
using DAL.DBAccess.Models;
using Helper.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.ApiLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly Utilities _utilities;
        readonly IUser _user;
        public UserController(Utilities utilities, IUser user)
        {
            _utilities = utilities;
            _user = user;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Route("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                User? user = await _utilities.ValidateJWT(identity);
                if (user == null) { StatusCode(StatusCodes.Status401Unauthorized); }
                List<User>? users = await _user.GetAllUser();
                return StatusCode(StatusCodes.Status200OK,users);
            }
            catch (Exception e)
            {
                Log.Error($"Error GetAll[01]: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
