using Anade.Khadamat.Domain;
using Anade.Khadamat.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Anade.Khadamat.Data
{
    public class CommunicationDbContext : DbContext
    {
        public CommunicationDbContext() : base()
        {

        }

        public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options) : base(options)
        {
            

        }


        #region DbSet

        public DbSet<Activite> Activites { get; set; }
     

        public DbSet<ActiviteJourneeInfo> ActiviteJourneeInfos { get; set; }
        public DbSet<ActiviteSalon> ActiviteSalons { get; set; }
        public DbSet<ActiviteForum> ActiviteForums { get; set; }
        public DbSet<ActiviteReunionExterne> ActiviteReunionExternes { get; set; }
        public DbSet<ActiviteRadio> ActiviteRadios { get; set; }
        public DbSet<ActiviteTelevision> ActiviteTVs { get; set; }
        public DbSet<ActivitePresse> ActivitePresses { get; set; }
        public DbSet<AgenceWilaya> AgenceWilayas { get; set; }
        public DbSet<MoisCloture> MoisClotures { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=.\serverDB;Database=KhadamatDB;Integrated Security=true;");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           modelBuilder.Entity<ActiviteJourneeInfo>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteJournesInfos)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiviteForum>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteForums)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivitePresse>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activitePresse)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiviteRadio>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteRadio)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ActiviteReunionExterne>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteReunionExterne)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiviteSalon>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteSalon)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiviteTelevision>()
       .HasOne(p => p.Activite)
       .WithMany(e => e.activiteTelevision)
       .HasForeignKey(p => p.ActiviteId)
       .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Activite>()
       .HasOne(a => a.AgenceWilaya)
       .WithMany(w => w.Activities)
       .HasForeignKey(a => a.AgenceWilayaId)
       .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<MoisCloture>()
            .HasIndex(x => new { x.Annee, x.Mois })
            .IsUnique();
            base.OnModelCreating(modelBuilder);

        }

        
    }

}
