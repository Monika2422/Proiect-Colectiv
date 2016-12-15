using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CWMD.Models
{
    public class Document
    {
        public int Id { get; set; }
        public String FileName { get; set; }
        public String FileExtension { get; set; }
        public DateTime CreationDate { get; set; }
        public String TemplateName { get; set; }
        public String Abstract { get; set; }
        public String Status { get; set; }
        public String KeyWords { get; set; }
        public String AuthorUserName { get; set; }
    }
}