﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class VeseetaDBContext : IdentityDbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        // optionsBuilder.UseSqlServer("Server=LAPTOP-I2QOALF5\\SQLEXPRESS;Database=VeseetaDBV1;Integrated Security=True;Encrypt=False");


       public VeseetaDBContext() { }
       public VeseetaDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Review Entity Configuration
          
            base.OnModelCreating(modelBuilder);
        }

        // DbSet properties for each entity in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BlockOutHours> BlockOutHours { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<ClinicInsurance> ClinicInsurances { get; set; }
    }
}
