using Common;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TemplateLayer.Entities;

namespace TemplateLayer
{
    public class VariablesLogic
    {
        public List<Variable> ExtractDocumentVariables(byte[] doument)
        {
            var document = new MemoryStream(doument);
            var wordProcessingDocument = WordprocessingDocument.Open(document, false);
            var matchedVariables = new List<Variable>();

            foreach (var section in wordProcessingDocument.MainDocumentPart.Document.Body.Elements())
            {
                if(!string.IsNullOrEmpty(section.InnerText))
                {
                    Regex rx = new Regex(@"(\[\[.+?\]\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var matches = rx.Matches(section.InnerText);
                    foreach (Match match in matches)
                    {
                        if (!string.IsNullOrEmpty(match.Value))
                        {
                            var trimVariable = match.Value.Trim('[', ']');
                            matchedVariables.Add(new Variable
                            {
                                VariableName = trimVariable,
                                Type = GetVariableType(trimVariable)
                            });
                        }
                    }
                }
            }

            return matchedVariables;
        }

        public VariableType GetVariableType(string variable)
        {
            var types = (IEnumerable<VariableType>)Enum.GetValues(typeof(VariableType));
            var matchedType = types.FirstOrDefault(s => s.ToString() == variable);

            return matchedType;
        }
    }
}
