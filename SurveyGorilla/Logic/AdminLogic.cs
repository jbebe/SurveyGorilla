using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SurveyGorilla.Models;
using Newtonsoft.Json;

namespace SurveyGorilla.Logic
{
    public class AdminLogic
    {
        private SurveyContext _context;

        public AdminLogic(SurveyContext context)
        {
            _context = context;
        }

        public AdminEntity CreateAdmin(AdminData data)
        {
            var admin = new AdminEntity();
            var adminInfo = new { registered = DateTime.UtcNow };
            admin.EmailAddress = data.Email;
            admin.Info = JsonConvert.SerializeObject(adminInfo);
            admin.PasswordHash = Crypto.Sha256(data.Password);
            _context.Add(admin);
            _context.SaveChanges();
            return admin;
        }
    }
}
