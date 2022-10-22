using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BrsBlogWeb.Models;

namespace BrsBlogWeb.Data
{
    public class BrsAdminsContext : DbContext
    {
        public BrsAdminsContext (DbContextOptions<BrsAdminsContext> options)
            : base(options)
        {
        }

        public DbSet<BrsBlogWeb.Models.Admins> Admins { get; set; }
    }
}
