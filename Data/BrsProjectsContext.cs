using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BrsBlogWeb.Models;

namespace BrsBlogWeb.Data
{
    public class BrsProjectsContext : DbContext
    {
        public BrsProjectsContext (DbContextOptions<BrsProjectsContext> options)
            : base(options)
        { 
        }

        public DbSet<BrsBlogWeb.Models.Projects> Projects { get; set; }
    }
}
