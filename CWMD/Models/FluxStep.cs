using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWMD.Models
{
    public class FluxStep
    {
        public int IdTypeFlux { get; set; }
        [Required]
        public int StepNumber { get; set; }

        public String Username { get; set; }

        public String DepartmentName { get; set; }


    }
}