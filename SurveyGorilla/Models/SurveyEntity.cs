using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class SurveyEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public string Info { get; set; }

        public AdminEntity Admin { get; set; }
        public IEnumerable<ClientEntity> Clients { get; set; }
    }
}
