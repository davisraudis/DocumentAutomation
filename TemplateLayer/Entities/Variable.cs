using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateLayer.Entities
{
    public class Variable
    {
        public string VariableName { get; set; }

        public VariableType Type { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }
    }
}
