#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Microsoft.AspNetCore.Mvc;
using SurveyGorilla.Models;
using SurveyGorilla.Logic;
using Microsoft.AspNetCore.Http;
using static SurveyGorilla.Misc.Helper;

namespace SurveyGorilla.Controllers
{
    /// <summary>Controls the non-api part of the SPA</summary>
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

        /// <summary>Gets the index page</summary>
        /// <remarks>
        /// It is only for convenience. A single page 
        /// web-application does not need html to be served from code.
        /// </remarks>
        /// <returns>index.html</returns>
        /// <response code="200">Successful</response>
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        /// <summary>Register a new user</summary>
        /// <returns>admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Unsuccessful</response>
        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] AdminData data)
        {
            try
            {
                return Json(_logic.Register(data));
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(e);
            }
        }

        /// <summary>Logs in a new user</summary>
        /// <remarks>
        /// The session now holds the admin id.
        /// The api is now restricted in a way that the users 
        /// can only manipulate data that was created by them.
        /// </remarks>
        /// <returns>Admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Could not log in with the credentials</response>
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginData data)
        {
            try
            {
                return Json(_logic.Login(HttpContext.Session, data));
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(e);
            }
        }

        /// <summary>Logs out a new user</summary>
        /// <remarks>
        /// The session variable holding the the admin id is deleted.
        /// To access the api again, you have to log in.
        /// </remarks>
        /// <returns>admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Could not log out</response>
        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                return Json(_logic.Logout(HttpContext.Session, Response.Cookies));
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(e);
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
