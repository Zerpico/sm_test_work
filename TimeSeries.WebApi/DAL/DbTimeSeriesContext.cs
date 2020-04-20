using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using TimeSeries.Common.Models;
using System.Configuration;

namespace TimeSeries.WebApi.DAL
{
    public partial class DbTimeSeriesContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Observable> Observables { get; set; }
        public DbSet<Serie> Series { get; set; }

          public DbTimeSeriesContext(DbContextOptions options)
              : base(options)
          {
          }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            //optionsBuilder.UseInMemoryDatabase("seriesDB");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);
                entity.Property(e => e.CountryId).ValueGeneratedNever();
                entity.Property(e => e.CountryName).IsRequired();
            });

            modelBuilder.Entity<Indicator>(entity =>
            {
                entity.HasKey(e => e.IndicatorId);
                entity.Property(e => e.IndicatorId).ValueGeneratedNever();
                entity.Property(e => e.IndicatorName);
            });

            modelBuilder.Entity<Observable>(entity =>
            {
                entity.HasKey(e => e.ObservableId);
                entity.Property(e => e.Time).HasColumnType("DATETIME");
                entity.Property(e => e.ObservableValue);
                entity.HasOne(d => d.Serie)
                    .WithMany(p => p.Observables);
            });

            modelBuilder.Entity<Serie>(entity =>
            {
                entity.HasKey(e => e.SerieId);
                entity.Property(e => e.SerieId).ValueGeneratedNever();
                entity.Property(e => e.Comment);
                entity.HasOne(d => d.Country);
                entity.HasOne(d => d.Indicator);
                entity.HasMany(d => d.Observables);
            });
        }

    }
}
