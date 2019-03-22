using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class TemplateVariable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public VariableType Type { get; set; }

        public int? TemplateId { get; set; }
    }
}
