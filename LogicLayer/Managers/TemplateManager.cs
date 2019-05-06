using Data.Entities;
using DocumentAutomation.Data;
using DocumentFormat.OpenXml.Packaging;
using LogicLayer.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TemplateLayer;
using TemplateLayer.Entities;

namespace LogicLayer.Managers
{
    public class TemplateManager : ITemplateManager
    {
        private readonly VariablesLogic _variablesLogic = new VariablesLogic();
        private readonly DocumentGenerator _documentGenerator = new DocumentGenerator();

        public void LoadDocument(string path)
        {
            WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(path, true);
            wordprocessingDocument.Save();
        }

        public int? CreateTemplate(DatabaseContext db, Template template)
        {
            if (template != null)
            {
                db.Template.Add(template);
                db.SaveChanges();

                return template.Id;
            }

            return null;
        }

        public void CreateTemplateFile(DatabaseContext db, IFormFile file, int templateId)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var fileModel = new Data.Entities.File
                {
                    FileName = file.FileName,
                    CreateDate = DateTime.Now,
                    TemplateId = templateId,
                    Content = memoryStream.ToArray()
                };

                db.File.Add(fileModel);
                db.SaveChanges();
            }
        }

        public IEnumerable<Template> GetAllTemplates(DatabaseContext db)
        {
            return db.Template.AsEnumerable();
        }

        public Template GetTemplate(DatabaseContext db, int templateId)
        {
            return db.Template.FirstOrDefault(t => t.Id == templateId);
        }

        public void SetTemplateVariableValue(DatabaseContext db, int templateId, int variableId, string value)
        {
            var template = GetTemplate(db, templateId);
            var variable = template.Variables.FirstOrDefault(v => v.Id == variableId);

            variable.Value = value;
            db.SaveChanges();
        }

        public void GenerateVariablesFromTemplateFiles(DatabaseContext db, int templateId)
        {
            if (!db.TemplateVariable.Any(t => t.TemplateId == templateId))
            {
                var files = db.Template.FirstOrDefault(t => t.Id == templateId)?.Files;
                List<Variable> extractedVariables = new List<Variable>();

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (extractedVariables.Any())
                        {
                            extractedVariables.Concat(_variablesLogic.ExtractDocumentVariables(file.Content));
                        }
                        else
                        {
                            extractedVariables = _variablesLogic.ExtractDocumentVariables(file.Content);
                        }
                    }
                }

                if (extractedVariables != null)
                {
                    foreach (var variable in extractedVariables)
                    {
                        db.TemplateVariable.Add(new TemplateVariable
                        {
                            Name = variable.VariableName,
                            TemplateId = templateId,
                            Type = variable.Type,
                            Description = variable.Description,
                            DefaultValue = variable.DefaultValue
                        });
                    }

                    db.SaveChanges();
                }
            }
        }

        public void GenerateTemplateDocumentsByVariables(DatabaseContext db, int templateId, string userId)
        {
            var template = GetTemplate(db, templateId);

            if (template != null)
            {
                foreach (var file in template.Files)
                {
                    List<TemplateLayer.Entities.Variable> documentVariables = new List<TemplateLayer.Entities.Variable>();
                    foreach (var variable in template.Variables)
                    {
                        documentVariables.Add(new TemplateLayer.Entities.Variable
                        {
                            VariableName = variable.Name,
                            Value = variable.Value
                        });
                    }

                    var document = _documentGenerator.GenerateDocumentByVariables(file.Content, documentVariables);
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToLongDateString().Trim('.') + Path.GetExtension(file.FileName);

                    db.GeneratedDocument.Add(new GeneratedDocument
                    {
                        FileName = fileName,
                        Content = document,
                        GenerationDate = DateTime.Now,
                        TemplateId = templateId,
                        UserID = userId
                    });
                }

                db.SaveChanges();
            }
        }

        public GeneratedDocument GetGeneratedDocument(DatabaseContext db, int documentId)
        {
            return db.GeneratedDocument.FirstOrDefault(d => d.Id == documentId);
        }
    }
}
