using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public int? TemplateId { get; set; }

        public byte[] Content { get; set; }

        public DateTime CreateDate { get; set; }

        //public virtual Template Template { get; set; }
    }
}
