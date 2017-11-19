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

    /// <summary>Manages surveys</summary>
    [Produces("application/json")]
    [Route("api/Survey")]
    public class SurveyController : Controller
    {
        private readonly SurveyContext _context;
        private readonly SurveyLogic _logic;

        private int AdminId {
            get
            {
                return HttpContext.Session.GetInt32(Session.adminId).Value;
            }
        }

        public SurveyController(SurveyContext context)
        {
            _context = context;
            _logic = new SurveyLogic(_context);
        }

        /// <summary>Returns all surveys</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <returns>Array of survey entities</returns>
        [HttpGet]
        public IActionResult GetSurveys()
        {
            try
            {
                return Ok(_logic.GetAllSurvey(AdminId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Returns a survey with given ID</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <param name="surveyId">ID of the survey</param>
        /// <returns>Client entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpGet("{surveyId}")]
        public IActionResult GetSurveyEntity([FromRoute] int surveyId)
        {
            try
            {
                return Ok(_logic.GetSurvey(AdminId, surveyId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Creates a new survey</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// availability.start/end should have this value: (new Date()).toISOString().replace(/T/g,' ').substring(0,19)
        /// </remarks>
        /// <param name="surveyData">Partially filled survey data</param>
        /// <returns>Survey entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPost]
        public IActionResult PostSurveyEntity([FromBody] SurveyData surveyData)
        {
            try
            {
                return Ok(_logic.CreateSurvey(AdminId, surveyData));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Updates a survey with given ID</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <param name="surveyId">ID of the survey</param>
        /// <param name="surveyData">Partially filled survey data</param>
        /// <returns>Survey entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPut("{surveyId}")]
        public IActionResult PutSurveyEntity([FromRoute] int surveyId, [FromBody] SurveyData surveyData)
        {
            try
            {
                return Ok(_logic.UpdateSurvey(AdminId, surveyId, surveyData));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Deletes a survey with given ID</summary>
        /// <remarks>
        /// You have to be logged in as admin, to access this endpoint!
        /// </remarks>
        /// <param name="surveyId">ID of the survey</param>
        /// <returns>Survey entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{surveyId}")]
        public IActionResult DeleteSurveyEntity([FromRoute] int surveyId)
        {
            try
            {
                return Ok(_logic.DeleteSurvey(AdminId, surveyId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}