using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CWMD.Models
{
    public class DocumentVersion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string filePath { get; set; }
        public int DocumentId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public float VersionNumber { get; set; }
    }
}