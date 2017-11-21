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
                a.Email == data.Email && 
                a.PasswordHash == Crypto.Sha256(data.Password)
            );
            session.SetInt32(Session.adminId, admin.Id);
            return admin;
        }
        
        public AdminEntity Logout(ISession session, IResponseCookies cookies)
        {
            var adminId = session.GetInt32(Session.adminId).Value;
            var adminEntity = _context.Admins.Single(admin => admin.Id == adminId);
            session.Clear();
            cookies.Delete(Session.cookieName);
            return adminEntity;
        }
    }
}
