using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWMD.Models
{
    public class FluxTypes
    {
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }


    }
}