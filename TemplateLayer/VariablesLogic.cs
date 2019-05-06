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
                    Regex rx = new Regex($@"(\[\[(?<variableName>(.+?))(?<expression>\:((?<defaultValue>\(.+?\))?)((?<variableDescription>\'.+?\')?))?]\])", RegexOptions.Compiled);
                    var matches = rx.Matches(section.InnerText);
                    foreach (Match match in matches)
                    {
                        var variableName = match.Groups["variableName"].Value;
                        var expression = match.Groups["expression"].Value;
                        var defaultValue = match.Groups["defaultValue"].Value;
                        var variableDescription = match.Groups["variableDescription"].Value;

                        if (!matchedVariables.Any(v => v.VariableName == variableName))
                        {
                            matchedVariables.Add(new Variable
                            {
                                VariableName = variableName,
                                Type = GetVariableType(variableName),
                                Value = defaultValue,
                                Description = variableDescription,
                                DefaultValue = defaultValue
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
