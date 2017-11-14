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
    /// <summary>Manages admins (survey creators)</summary>
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly SurveyContext _context;
        private readonly AdminLogic _logic;

        public AdminController(SurveyContext context)
        {
            _context = context;
            _logic = new AdminLogic(_context);
        }

        /// <summary>Returns all admins</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <returns>Array of admin entities</returns>
        [HttpGet]
        public IActionResult GetAdmins()
        {
            try
            {
                return Ok(_logic.GetAllAdmins());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Returns an admin with given ID</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <param name="adminId">ID of the admin to get</param>
        /// <returns>Admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpGet("{adminId}")]
        public IActionResult GetAdminEntity([FromRoute] int adminId)
        {
            try
            {
                return Ok(_logic.GetAdmin(adminId));
            } catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Updates an admin with given ID</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <param name="adminId">ID of the admin to update</param>
        /// <param name="adminData">Partially filled admin data</param>
        /// <returns>Admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPut("{adminId}")]
        public IActionResult PutAdminEntity([FromRoute] int adminId, [FromBody] AdminData adminData)
        {
            try
            {
                return Ok(_logic.UpdateAdmin(adminId, adminData));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Creates a new admin</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <param name="adminData">Partially filled admin data</param>
        /// <returns>Entity with ID fields filled</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id or content</response>
        [HttpPost]
        public IActionResult PostAdminEntity([FromBody] AdminData adminData)
        {
            try
            {
                return Ok(_logic.CreateAdmin(adminData));
            } catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Deletes an admin with given ID</summary>
        /// <remarks>
        /// You have to be a server admin (stored in a session variable) 
        /// to access these endpoints.
        /// </remarks>
        /// <param name="adminId">ID of the admin to get</param>
        /// <returns>Admin entity</returns>
        /// <response code="200">Successful</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{adminId}")]
        public IActionResult DeleteAdminEntity([FromRoute] int adminId)
        {
            try
            {
                return Ok(_logic.DeleteAdmin(adminId));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
 
    }
}