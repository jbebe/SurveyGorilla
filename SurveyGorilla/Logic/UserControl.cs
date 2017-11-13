using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SurveyGorilla.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace SurveyGorilla.Logic
{
    public class UserControl
    {
        private readonly SurveyContext _context;

        public UserControl(SurveyContext context)
        {
            _context = context;
        }
        
        public AdminEntity Register(AdminData data)
        {
            var adminLogic = new AdminLogic(_context);
            return adminLogic.CreateAdmin(data);
        }

        public AdminEntity Login(ISession session, LoginData data)
        {
            var admin = _context.Admins.First(a => 
                a.EmailAddress == data.Email && 
                a.PasswordHash == Crypto.Sha256(data.Password)
            );
            session.SetInt32(Session.adminId, admin.Id);
            return admin;
        }
        
        public void Logout(ISession session, IResponseCookies cookies)
        {
            session.Clear();
            cookies.Delete(".AspNetCore.Session");
        }
    }
}
