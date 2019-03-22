using Data.Entities;
using DocumentAutomation.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Managers.Interfaces
{
    public interface ITemplateManager
    {
        int? CreateTemplate(DatabaseContext db, Template template);

        void CreateTemplateFile(DatabaseContext db, IFormFile file, int templateId);

        IEnumerable<Template> GetAllTemplates(DatabaseContext db);

        Template GetTemplate(DatabaseContext db, int templateId);
    }
}
