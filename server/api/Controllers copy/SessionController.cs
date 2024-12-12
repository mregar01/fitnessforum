using System;
using fitnessapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessapi.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("user")]
        public IActionResult SetCurrentUser([FromBody] CurrentUser currentUser)
        {
            if (currentUser.DisplayName != null)
            {
                Response.Cookies.Append("Id", currentUser.UserId.ToString());
                Response.Cookies.Append("Name", currentUser.DisplayName);
            }
            

            return Ok();
        }

        [HttpGet("user")]
        public ActionResult<CurrentUser>? GetCurrentUser()
        {
            try
            {
                string? idstring = Request.Cookies["Id"];
                if (idstring != null)
                {
                    var user = new CurrentUser
                    {
                        DisplayName = Request.Cookies["Name"],

                        UserId = int.Parse(idstring)
                    };
                    return user;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get user");
                Console.WriteLine(e);
                throw;
            }


        }
    }
}