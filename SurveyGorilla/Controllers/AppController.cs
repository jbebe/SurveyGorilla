#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Microsoft.AspNetCore.Mvc;
using SurveyGorilla.Models;
using SurveyGorilla.Logic;
using Microsoft.AspNetCore.Http;
using static SurveyGorilla.Misc.Helper;

namespace SurveyGorilla.Controllers
{
    /// <summary>Controls the non-api part of the SPA</summary>
    [Route("/")]
    public class AppController : Controller
    {
        private readonly SurveyContext _context;
        private readonly UserControl _logic;

        public AppController(SurveyContext context)
        {
            _context = context;
            _logic = new UserControl(_context);
        }

        /// <summary>Gets the index page</summary>
        /// <remarks>
        /// It is only for convenience. A single page 
        /// web-application does not need html to be served from code.
        /// </remarks>
        /// <returns>index.html</returns>
        /// <response code="200">Successful</response>
        [Produces("text/html")]
        [HttpGet]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        /// <summary>Register a new user</summary>
        /// <returns>admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Unsuccessful</response>
        [Produces("application/json")]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] AdminData data)
        {
            try
            {
                return Ok(_logic.Register(data));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Logs in a new user</summary>
        /// <remarks>
        /// The session now holds the admin id.
        /// The api is now restricted in a way that the users 
        /// can only manipulate data that was created by them.
        /// </remarks>
        /// <returns>Admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Could not log in with the credentials</response>
        [Produces("application/json")]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginData data)
        {
            try
            {
                return Ok(_logic.Login(HttpContext.Session, data));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Logs out a new user</summary>
        /// <remarks>
        /// The session variable holding the the admin id is deleted.
        /// To access the api again, you have to log in.
        /// </remarks>
        /// <returns>admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Could not log out</response>
        [Produces("application/json")]
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            try
            {
                return Ok(_logic.Logout(HttpContext.Session, Response.Cookies));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

    }
}
