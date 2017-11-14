using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Logic
{
    public class AdminData
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Info { get; set; }
    }

    public class LoginData
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class SurveyData
    {
        public string Name { get; set; }

        public string Info { get; set; }
    }

    public class ClientData
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Info { get; set; }
    }
    
}
