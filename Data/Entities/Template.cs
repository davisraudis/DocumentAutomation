using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class Template
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual IEnumerable<File> Files { get; set; }

        public virtual IEnumerable<TemplateVariable> Variables { get; set; }

        public virtual IEnumerable<GeneratedDocument> GeneratedDocuments { get; set; }
    }
}
