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
    [Route("api/Mail")]
    public class MailController : Controller
    {
        private readonly SurveyContext _context;
        private readonly MailLogic _logic;

        private int AdminId
        {
            get
            {
                return HttpContext.Session.GetInt32(Session.adminId).Value;
            }
        }

        public MailController(SurveyContext context)
        {
            _context = context;
            _logic = new MailLogic(_context, () => this.Request);
        }

        /// <summary>Send out all emails in survey</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <returns>Array of clients who get the email</returns>
        [HttpGet("Survey/{surveyId}")]
        public IActionResult SendToAllSurveyMembers([FromRoute] int surveyId)
        {
            try
            {
                return Ok(_logic.SendToAllSurveyMembers(AdminId, surveyId));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>Send out all emails in survey</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <returns>Array of clients who get the email</returns>
        [HttpGet("Survey/{surveyId}/Client/{clientId}")]
        public IActionResult SendToSurveyMember([FromRoute] int surveyId, [FromRoute] int clientId)
        {
            try
            {
                return Ok(_logic.SendToSurveyMember(AdminId, surveyId, clientId));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

    }
}