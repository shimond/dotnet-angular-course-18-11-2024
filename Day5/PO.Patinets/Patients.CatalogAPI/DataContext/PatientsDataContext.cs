using Microsoft.EntityFrameworkCore;
using Patients.CatalogAPI.Models;

namespace Patients.CatalogAPI.DataContext
{
    public class PatientsDataContext : DbContext
    {
        public DbSet<PatientModel> Patients  { get; set; }

        public PatientsDataContext(DbContextOptions<PatientsDataContext> options) :base(options)
        {
                
        }
    }
}
