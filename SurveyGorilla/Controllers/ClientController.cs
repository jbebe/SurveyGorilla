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
    [Route("api/Client")]
    public class ClientController : Controller
    {
        private readonly SurveyContext _context;

        public ClientController(SurveyContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public IEnumerable<ClientEntity> GetClients()
        {
            return _context.Clients;
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientEntity = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);

            if (clientEntity == null)
            {
                return NotFound();
            }

            return Ok(clientEntity);
        }

        // PUT: api/Client/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientEntity([FromRoute] int id, [FromBody] ClientEntity clientEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientEntityExists(id))
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

        // POST: api/Client
        [HttpPost]
        public async Task<IActionResult> PostClientEntity([FromBody] ClientEntity clientEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clients.Add(clientEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientEntity", new { id = clientEntity.Id }, clientEntity);
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientEntity = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);
            if (clientEntity == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(clientEntity);
            await _context.SaveChangesAsync();

            return Ok(clientEntity);
        }

        private bool ClientEntityExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}