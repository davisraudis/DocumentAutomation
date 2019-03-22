using Data.Entities;
using DocumentAutomation.Data;
using DocumentFormat.OpenXml.Packaging;
using LogicLayer.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogicLayer.Managers
{
    public class TemplateManager : ITemplateManager
    {
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
    }
}
