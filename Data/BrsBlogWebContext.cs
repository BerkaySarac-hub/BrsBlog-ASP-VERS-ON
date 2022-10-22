using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BrsBlogWeb.Models;

namespace BrsBlogWeb.Data
{
    public class BrsBlogWebContext : DbContext
    {
        public BrsBlogWebContext (DbContextOptions<BrsBlogWebContext> options)
            : base(options)
        {

        }

        public DbSet<BrsBlogWeb.Models.BlogPost> BlogPost { get; set; }

        
    }
}
