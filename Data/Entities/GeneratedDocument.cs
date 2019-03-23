using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class GeneratedDocument
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }

        public DateTime GenerationDate { get; set; }

        public int TemplateId { get; set; }

        public string UserID { get; set; }

        public virtual IdentityUser User { get; set; }

    }
}
