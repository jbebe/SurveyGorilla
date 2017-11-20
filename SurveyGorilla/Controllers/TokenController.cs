using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyGorilla.Models;
using SurveyGorilla.Logic;

namespace SurveyGorilla.Controllers
{
    /// <summary>Create and upload filled survey</summary>
    [Route("survey")]
    public class TokenController : Controller
    {
        private readonly SurveyContext _context;
        private readonly ClientLogic _logic;

        public TokenController(SurveyContext context)
        {
            _context = context;
            _logic = new ClientLogic(_context);
        }

        /// <summary>Gets the survey page</summary>
        /// <remarks>
        /// The page does not know about the token, it has to query it by itself.
        /// </remarks>
        /// <param name="token">The token for the client</param>
        /// <returns>token.html</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Unsuccessful</response>
        [Produces("text/html")]
        [HttpGet("{token}")]
        public IActionResult GetPage([FromRoute] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return File("~/index.html", "text/html");
            } else
            {
                return NotFound();
            }
        }

        /// <summary>Receives the survey answers</summary>
        /// <remarks>
        /// This method updates the client with its answers.
        /// If the user re-take the survey, the answers will be updated.
        /// </remarks>
        /// <param name="token">Token for the client</param>
        /// <param name="clientData">Questions filled in info property</param>
        /// <returns>The updated client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Unsuccessful</response>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("{token}")]
        public IActionResult ReceiveAnswers([FromRoute] string token, [FromBody] ClientData clientData)
        {
            try
            {
                return Ok(_logic.UpdateClientAnswers(token, clientData));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }   
        }
        
    }
}