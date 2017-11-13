using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Logic
{
    public class AdminData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginData
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class SurveyData
    {
        [Required]
        public string Info { get; set; }
    }

    public class ClientData
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Info { get; set; }
    }
    
}
