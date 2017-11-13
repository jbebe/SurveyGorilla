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

        // GET: {surveyId}/Client
        [HttpGet("{surveyId}/Client")]
        public IActionResult GetClients()
        {
            try
            {
                return Ok(_logic.GetAllClient(AdminId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: {surveyId}/Client/{clientId}
        [HttpGet("{surveyId}/Client/{clientId}")]
        public IActionResult GetClientEntity([FromRoute] int surveyId, [FromRoute] int clientId)
        {
            try
            {
                return Ok(_logic.GetClient(AdminId, surveyId, clientId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: {surveyId}/Client
        [HttpPost("{surveyId}/Client")]
        public IActionResult PostClientEntity([FromRoute] int surveyId, [FromBody] ClientData clientData)
        {
            try
            {
                return Ok(_logic.CreateClient(AdminId, surveyId, clientData));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: {surveyId}/Client/{clientId}
        [HttpDelete("{surveyId}/Client/{clientId}")]
        public IActionResult DeleteClientEntity([FromRoute] int surveyId, [FromRoute] int clientId)
        {
            try
            {
                return Ok(_logic.DeleteClient(AdminId, surveyId, clientId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}