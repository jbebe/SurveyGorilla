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

        // GET: api/Survey
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

        // GET: api/Survey/{id}
        [HttpGet("{id}")]
        public IActionResult GetSurveyEntity([FromRoute] int id)
        {
            try
            {
                return Ok(_logic.GetSurvey(AdminId, id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/Survey/
        [HttpPost]
        public IActionResult PostSurveyEntity([FromBody] SurveyData data)
        {
            try
            {
                return Ok(_logic.CreateSurvey(AdminId, data));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT: api/Survey/{id}
        [HttpPut("{id}")]
        public IActionResult PutSurveyEntity([FromRoute] int surveyId, [FromBody] SurveyData data)
        {
            try
            {
                return Ok(_logic.UpdateSurvey(AdminId, surveyId, data));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/Survey/{id}
        [HttpDelete("{id}")]
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