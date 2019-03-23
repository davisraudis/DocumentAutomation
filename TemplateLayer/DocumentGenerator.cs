using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TemplateLayer.Entities;

namespace TemplateLayer
{
    public class DocumentGenerator
    {
        public byte[] GenerateDocumentByVariables(byte[] documentContent, List<Variable> variables)
        {
            using (MemoryStream stream = new MemoryStream(documentContent))
            {
                using (WordprocessingDocument wordProcessingDocument = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordProcessingDocument.MainDocumentPart.Document.Body;
                    var paragraphs = body.Elements<Paragraph>();
                    Regex rx = new Regex(@"(\[\[.+?\]\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    foreach (var para in paragraphs)
                    {
                        foreach (var run in para.Elements<Run>())
                        {
                            foreach (var text in run.Elements<Text>())
                            {
                                foreach (Match match in rx.Matches(text.Text))
                                {
                                    foreach (var variable in variables)
                                    {
                                        var matchRegex = new Regex($@"(\[\[{variable.VariableName}]\])");
                                        text.Text = matchRegex.Replace(text.Text, variable.Value ?? string.Empty);
                                        //if (variable.VariableName == match.Value.Trim('[', ']'))
                                        //{
                                        //    text.Text = rx.Replace(text.Text, variable.Value ?? string.Empty, 1);
                                        //    break;
                                        //}
                                    }            
                                }
                            }
                        }
                    }

                    foreach (var text in body.Descendants<Text>())
                    {
                        foreach (Match match in rx.Matches(text.Text))
                        {
                            foreach (var variable in variables)
                            {
                                var matchRegex = new Regex($@"(\[\[{variable.VariableName}]\])");
                                text.Text = matchRegex.Replace(text.Text, variable.Value ?? string.Empty);
                                //if (variable.VariableName == match.Value.Trim('[', ']'))
                                //{
                                //    text.Text = rx.Replace(text.Text, variable.Value ?? string.Empty, 1);

                                //    //if (!string.IsNullOrEmpty(variable.Value))
                                //    //{
                                //    //    text.Text = text.Text.Remove(match.Index, match.Length).Insert(match.Index, variable.Value);
                                //    //}
                                //    //else
                                //    //{
                                //    //    text.Text = text.Text.Remove(match.Index, match.Length).Insert(match.Index, string.Empty);
                                //    //}
                                //    break;
                                //}
                            }
                        }
                    }
                }

                return stream.ToArray();
            }
        }
    }
}
