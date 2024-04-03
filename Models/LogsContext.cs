using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yet_Another_Traceback_Tracker.Models
{
    public class LogEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LogContent { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; } 
    }

    public class LogsContext : DbContext
    {
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntity>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        public DbSet<LogEntity> MyEntities { get; set; }
    }
}