using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SurveyGorilla.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace SurveyGorilla.Logic
{
    public class AdminLogic
    {
        private SurveyContext _context;

        public AdminLogic(SurveyContext context)
        {
            _context = context;
        }

        public AdminEntity CreateAdmin(AdminData adminData)
        {
            var admin = new AdminEntity();
            var adminInfo = new { registered = DateTime.UtcNow };
            if (new[] { adminData.Email, adminData.Info, adminData.Password }.Any(entry => entry == null))
            {
                throw new Exception("Important properties were not filled!");
            }
            admin.Email = adminData.Email;
            admin.PasswordHash = Crypto.Sha256(adminData.Password);
            admin.Info = JsonConvert.SerializeObject(adminInfo);
            _context.Add(admin);
            _context.SaveChanges();
            return admin;
        }

        public IEnumerable<AdminEntity> GetAllAdmins()
        {
            return _context.Admins;
        }

        public AdminEntity GetAdmin(int adminId)
        {
            return _context.Admins.Single(admin => admin.Id == adminId);
        }

        public AdminEntity UpdateAdmin(int adminId, AdminData adminData)
        {
            var admin = GetAdmin(adminId);

            if (adminData.Email != null)
            {
                admin.Email = adminData.Email;
            }
            if (adminData.Password != null)
            {
                admin.PasswordHash = Crypto.Sha256(adminData.Password);
            }
            if (adminData.Info != null)
            {
                admin.Info = adminData.Info;
            }

            _context.SaveChanges();
            return admin;
        }

        public AdminEntity DeleteAdmin(int adminId)
        {
            var admin = GetAdmin(adminId);
            _context.Remove(admin);
            return admin;
        }
    }
}
