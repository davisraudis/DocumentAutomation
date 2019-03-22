using System;
using System.Collections.Generic;
using System.Text;
using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentAutomation.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        public DbSet<File> File { get; set; }

        public DbSet<Template> Template { get; set; }

        public DbSet<TemplateVariable> TemplateVariable { get; set; }
    }
}
