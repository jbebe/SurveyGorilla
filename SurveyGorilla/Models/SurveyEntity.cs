using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SurveyGorilla.Models
{
    public class SurveyEntity
    {
        public SurveyEntity()
        {
            Clients = new List<ClientEntity>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int AdminId { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Info { get; set; }

        [IgnoreDataMember]
        public AdminEntity Admin { get; set; }

        [IgnoreDataMember]
        public List<ClientEntity> Clients { get; set; }
    }
}
