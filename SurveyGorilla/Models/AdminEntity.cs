using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class AdminEntity
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string Info { get; set; }

        public IEnumerable<SurveyEntity> Surveys { get; set; }
    }
}
