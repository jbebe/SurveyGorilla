using System;
using Microsoft.AspNetCore.Mvc;
using SurveyGorilla.Models;
using SurveyGorilla.Logic;
using Microsoft.AspNetCore.Http;

namespace SurveyGorilla.Controllers
{
    [Produces("text/html")]
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

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromBody] AdminData data)
        {
            try
            {
                return Ok(_logic.Register(data));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginData data)
        {
            try
            {
                _logic.Login(HttpContext.Session, data);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                _logic.Logout(HttpContext.Session, Response.Cookies);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /*
        // GET: App
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admins.ToListAsync());
        }

        // GET: App/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminEntity = await _context.Admins
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adminEntity == null)
            {
                return NotFound();
            }

            return View(adminEntity);
        }

        // GET: App/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: App/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PasswordHash,Info")] AdminEntity adminEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminEntity);
        }

        // GET: App/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminEntity = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            if (adminEntity == null)
            {
                return NotFound();
            }
            return View(adminEntity);
        }

        // POST: App/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PasswordHash,Info")] AdminEntity adminEntity)
        {
            if (id != adminEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminEntityExists(adminEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminEntity);
        }

        // GET: App/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminEntity = await _context.Admins
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adminEntity == null)
            {
                return NotFound();
            }

            return View(adminEntity);
        }

        // POST: App/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminEntity = await _context.Admins.SingleOrDefaultAsync(m => m.Id == id);
            _context.Admins.Remove(adminEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminEntityExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
        */
    }
}
