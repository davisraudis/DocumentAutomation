using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Entities;
using DocumentAutomation.Data;
using DocumentAutomation.Models;
using LogicLayer.Managers;
using LogicLayer.Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAutomation.Controllers
{
    [Authorized]
    public class TemplateController : Controller
    {
        private ITemplateManager _templateManager = new TemplateManager();
        private readonly DatabaseContext _context;

        public TemplateController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Create(TemplateBuildModel model)
        {
            if (ModelState.IsValid)
            {
                var templateModel = new Template
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    CreateDate = DateTime.Now
                };

                var templateId = _templateManager.CreateTemplate(_context, templateModel);

                if (templateId != null)
                {
                    foreach (var item in model.Files)
                    {
                        _templateManager.CreateTemplateFile(_context, item, (int)templateId);
                    }
                }
            }
        }

        public IActionResult GetTemplates()
        {
            var templates = _templateManager.GetAllTemplates(_context);
            List<TemplateViewModel> model = new List<TemplateViewModel>();

            foreach (var template in templates)
            {
                model.Add(new TemplateViewModel
                {
                    Id = template.Id,
                    Name = template.Name,
                    Description = template.Description,
                    Files = template.Files
                });
            }

            return View("TemplatesMenu", model);
        }

        public IActionResult GetTemplate(int id)
        {
            var template = _templateManager.GetAllTemplates(_context);

            return View("ViewTemplate", template);
        }
    }
}