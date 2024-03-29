﻿using System;
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
        public IActionResult Create(TemplateBuildModel model)
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

                return GetTemplates();
            }

            return Index();
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
            var template = _templateManager.GetTemplate(_context, id);
            var model = new TemplateViewModel
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                Files = template.Files.OrderBy(f => f.CreateDate),
                Variables = template.Variables
            };

            return View("ViewTemplate", template);
        }

        public IActionResult GenerateTemplateValues(int id)
        {
            _templateManager.GenerateVariablesFromTemplateFiles(_context, id);
            return GetTemplate(id);
        }

        [HttpPost]
        public void SetTemplateVariableValue([FromForm]int templateId, [FromForm]int variableId, [FromForm]string value)
        {
            _templateManager.SetTemplateVariableValue(_context, templateId, variableId, value);
        }

        public IActionResult GenerateTemplateDocuments(int templateId)
        {
            _templateManager.GenerateTemplateDocumentsByVariables(_context, templateId, User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return GetTemplate(templateId);
        }

        [HttpGet]
        public IActionResult DownloadGeneratedDocument(int documentId)
        {
            var document = _templateManager.GetGeneratedDocument(_context, documentId);
            var content = new System.IO.MemoryStream(document.Content);
            var contentType = "APPLICATION/octet-stream";
            return File(content, contentType, document.FileName);
        }
    }
}