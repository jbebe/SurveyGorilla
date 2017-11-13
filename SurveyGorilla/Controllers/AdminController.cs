using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyGorilla.Models;

namespace SurveyGorilla.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly SurveyContext _context;

        public AdminController(SurveyContext context)
        {
            _context = context;
        }

        // GET: api/Admin
        [HttpGet]
        public IEnumerable<AdminEntity> GetAdmins()
        {
            return _context.Admins;
        }
        
        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEntity = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);

            if (adminEntity == null)
            {
                return NotFound();
            }

            return Ok(adminEntity);
        }
        
        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminEntity([FromRoute] int id, [FromBody] AdminEntity adminEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != adminEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(adminEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminEntityExists(id))
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
        
        // POST: api/Admin
        [HttpPost]
        public async Task<IActionResult> PostAdminEntity([FromBody] AdminEntity adminEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Admins.Add(adminEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminEntity", new { id = adminEntity.Id }, adminEntity);
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminEntity = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            if (adminEntity == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(adminEntity);
            await _context.SaveChangesAsync();

            return Ok(adminEntity);
        }

        private bool AdminEntityExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
 
    }
}