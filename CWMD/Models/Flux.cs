using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CWMD.Models
{
    public class Flux
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdFlux { get; set; }
        [Required]
        public int IdTypeFlux { get; set; }

        public String InitiaterUsername { get; set; }

        public int CurentStep { get; set; }
    }
}