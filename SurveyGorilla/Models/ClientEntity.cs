using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class ClientEntity
    {
        [Key]
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        public string Info { get; set; }

        public SurveyEntity Survey { get; set; }
    }
}
