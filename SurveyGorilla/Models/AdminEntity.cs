using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SurveyGorilla.Models
{
    public class AdminEntity
    {
        public AdminEntity()
        {
            Surveys = new List<SurveyEntity>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Info { get; set; }

        [IgnoreDataMember]
        public List<SurveyEntity> Surveys { get; set; }
    }
}
