using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tockoteka.Models;

namespace tockoteka.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<BlogCategory> BlogCategory { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
