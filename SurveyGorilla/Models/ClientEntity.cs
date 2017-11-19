using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class ClientEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int SurveyId { get; set; }

        [Required]
        public string Email { get; set; }

        public string Token { get; set; }

        [Required]
        public string Info { get; set; }

        [IgnoreDataMember]
        public SurveyEntity Survey { get; set; }
    }
}
