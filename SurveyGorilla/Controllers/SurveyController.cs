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

        public SurveyController(SurveyContext context)
        {
            _context = context;
            _logic = new SurveyLogic(_context);
        }

        // GET: api/Survey
        [HttpGet]
        public IEnumerable<SurveyEntity> GetSurveys()
        {
            return _context.Surveys;
        }

        // GET: api/Survey/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurveyEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var surveyEntity = await _context.Surveys.SingleOrDefaultAsync(m => m.Id == id);

            if (surveyEntity == null)
            {
                return NotFound();
            }

            return Ok(surveyEntity);
        }

        // PUT: api/Survey/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurveyEntity([FromRoute] int id, [FromBody] SurveyEntity surveyEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(surveyEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Survey
        [HttpPost]
        public IActionResult PostSurveyEntity([FromBody] SurveyData data)
        {
            if (ModelState.IsValid)
            {
                _logic.CreateSurvey(HttpContext.Session, data);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Survey/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurveyEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var surveyEntity = await _context.Surveys.SingleOrDefaultAsync(m => m.Id == id);
            if (surveyEntity == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(surveyEntity);
            await _context.SaveChangesAsync();

            return Ok(surveyEntity);
        }

        private bool SurveyEntityExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }
    }
}