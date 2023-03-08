using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore
{
    public class ApplicationContext : DbContext
    {
        public DbSet<VitalSign> VitalSigns { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasMany(b => b.VitalSigns)
                .WithOne();

            modelBuilder.Entity<Patient>()
                .Navigation(b => b.VitalSigns)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
