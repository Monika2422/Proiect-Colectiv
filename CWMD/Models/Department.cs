using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWMD.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        
        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}