using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public class DataDbContext : DbContext
    {
            public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

            }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(u => u.Username).IsUnique();
            });

            //builder.Entity<Comment>()
            //    .HasOne(f => f.Expense)
            //    .WithMany(c => c.Comments)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        // DbSet = Repository, O tabela din baza de date
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
