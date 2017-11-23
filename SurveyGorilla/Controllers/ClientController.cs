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

    /// <summary>Manages clients (who fills the survey)</summary>
    [Produces("application/json")]
    [Route("api/Survey")]
    public class ClientController : Controller
    {
        private readonly SurveyContext _context;
        private readonly ClientLogic _logic;

        private int AdminId
        {
            get
            {
                return HttpContext.Session.GetInt32(Session.adminId).Value;
            }
        }

        public ClientController(SurveyContext context)
        {
            _context = context;
            _logic = new ClientLogic(_context);
        }

        /// <summary>Returns all clients</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <param name="surveyId">ID of the survey that holds the clients</param>
        /// <returns>Array of client entities</returns>
        [HttpGet("{surveyId}/Client")]
        public IActionResult GetClients(int surveyId)
        {
            try
            {
                return Ok(_logic.GetAllClient(AdminId, surveyId));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Returns a client with given ID</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access this endpoint.
        /// </remarks>
        /// <param name="surveyId">ID of the survey that holds the client</param>
        /// <param name="clientId">ID of the client</param>
        /// <returns>Client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpGet("{surveyId}/Client/{clientId}")]
        public IActionResult GetClientEntity([FromRoute] int surveyId, [FromRoute] int clientId)
        {
            try
            {
                return Ok(_logic.GetClient(AdminId, surveyId, clientId));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Creates a new client</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access this endpoint.
        /// </remarks>
        /// <param name="surveyId">ID of the survey that holds the client</param>
        /// <param name="clientData">Partially filled client data</param>
        /// <returns>Client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPost("{surveyId}/Client")]
        public IActionResult PostClientEntity([FromRoute] int surveyId, [FromBody] ClientData clientData)
        {
            try
            {
                return Ok(_logic.CreateClient(AdminId, surveyId, clientData));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Updates a client with given ID</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <param name="surveyId">ID of the survey that holds the client</param>
        /// <param name="clientId">ID of the client to update</param>
        /// <param name="clientData">Partially filled client data</param>
        /// <returns>Client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPut("{surveyId}/Client/{clientId}")]
        public IActionResult UpdateClientEntity([FromRoute] int surveyId, [FromRoute] int clientId, [FromBody] ClientData clientData)
        {
            try
            {
                return Ok(_logic.UpdateClient(AdminId, surveyId, clientId, clientData));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Deletes a client with given ID</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <param name="surveyId">ID of the survey that holds the client</param>
        /// <param name="clientId">ID of the client</param>
        /// <returns>Client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{surveyId}/Client/{clientId}")]
        public IActionResult DeleteClientEntity([FromRoute] int surveyId, [FromRoute] int clientId)
        {
            try
            {
                return Ok(_logic.DeleteClient(AdminId, surveyId, clientId));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

    }
}