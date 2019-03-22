using Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TemplateLayer.Entities;

namespace DocumentAutomation.Models
{
    public class TemplateBuildModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class TemplateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<File> Files { get; set; }

        public IEnumerable<TemplateVariable> Variables { get; set; }
    }
}
