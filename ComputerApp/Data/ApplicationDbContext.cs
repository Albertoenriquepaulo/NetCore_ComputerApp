using System;
using System.Collections.Generic;
using System.Text;
using ComputerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ComputerApp.Models.Component> Component { get; set; }
        public DbSet<ComputerApp.Models.Computer> Computer { get; set; }
        public DbSet<ComputerApp.Models.ComputerComponent> ComputerComponent { get; set; }
        public DbSet<ComputerApp.Models.CType> CType { get; set; }
        public DbSet<ComputerApp.Models.Order> Order { get; set; }
    }
}
