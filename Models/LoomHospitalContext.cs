using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LoomCrm_Hospital.Models
{
    public class LoomHospitalContext : DbContext
    {
        public LoomHospitalContext() : base("name=LoomHospitalDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Professional> Professional { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<MyHospital> MyHospital { get; set; }
        public DbSet<SmsLog> SmsLog { get; set; }



    }
}