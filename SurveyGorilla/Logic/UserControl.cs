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
        
        public void Register(RegisterData data)
        {
            var admin = new AdminEntity();
            var adminInfo = new { registered = DateTime.UtcNow };
            admin.EmailAddress = data.Email;
            admin.Info = JsonConvert.SerializeObject(adminInfo);
            admin.PasswordHash = Crypto.Sha256(data.Password);
            _context.Add(admin);
            _context.SaveChanges();
        }

        public void Login(ISession session, LoginData data)
        {
            var admin = _context.Admins.First(a => 
                a.EmailAddress == data.Email && 
                a.PasswordHash == Crypto.Sha256(data.Password)
            );
            session.SetInt32(Session.adminId, admin.Id);
        }
        
        public void Logout(ISession session, IResponseCookies cookies)
        {
            session.Clear();
            cookies.Delete(".AspNetCore.Session");
        }
    }
}
