using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projetFinale.Models;

namespace projetFinale.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Produits>Goudes { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Chariot> Chariots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Chariot>()
                .HasOne(c => c.Produit).WithMany()
                .HasForeignKey(c => c.Id);
        }
    }
}
