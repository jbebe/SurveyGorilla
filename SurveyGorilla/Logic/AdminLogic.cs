#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

        public AdminEntity CreateAdmin(AdminData data)
        {
            var admin = new AdminEntity();
            var adminInfo = new { registered = DateTime.UtcNow };
            if (new[] { data.Email, data.Info, data.Password }.Any(entry => entry == null))
            {
                throw new Exception("Important properties were not filled!");
            }
            admin.EmailAddress = data.Email;
            admin.PasswordHash = Crypto.Sha256(data.Password);
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
                admin.EmailAddress = adminData.Email;
            }
            if (adminData.Password != null)
            {
                admin.PasswordHash = Crypto.Sha256(adminData.Password);
            }
            if (adminData.Info != null)
            {
                var oldInfo = JObject.Parse(admin.Info);
                var newInfo = JObject.Parse(adminData.Info);
                newInfo.Merge(oldInfo, new JsonMergeSettings()
                {
                    MergeArrayHandling = MergeArrayHandling.Union,
                    MergeNullValueHandling = MergeNullValueHandling.Ignore
                });
                admin.Info = newInfo.ToString();
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
