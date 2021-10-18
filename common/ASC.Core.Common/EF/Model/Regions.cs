
using System;

using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model
{
    public class Regions
    {
        public string Region { get; set; }
        public string Provider { get; set; }

        public string ConnectionString { get; set; }
    }

    public static class RegionsExtension
    {
        public static ModelBuilderWrapper AddRegions(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddRegions, Provider.MySql)
                .Add(PgSqlAddRegions, Provider.Postgre)
                .Add(MSSqlAddRegions, Provider.MSSql);
            return modelBuilder;
        }

        public static void MySqlAddRegions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Regions>()
                .HasKey(e => e.Region)
                .HasName("region_pk");

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.ToTable("regions");

                entity.Property(e => e.Region)
                .HasColumnName("region")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

                entity.Property(e => e.Provider)
                .HasColumnName("provider")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

                entity.Property(e => e.ConnectionString)
                .HasColumnName("connection_string")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");
            });
        }

        public static void PgSqlAddRegions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Regions>()
                .HasKey(e => e.Region)
                .HasName("region_pk");

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.ToTable("regions");

                entity.Property(e => e.Region)
                .HasColumnName("region");

                entity.Property(e => e.Provider)
                .HasColumnName("provider");

                entity.Property(e => e.ConnectionString)
                .HasColumnName("connection_string");
            });
        }

        public static void MSSqlAddRegions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Regions>()
                .HasKey(e => e.Region)
                .HasName("region_pk");

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.ToTable("regions");

                entity.Property(e => e.Region)
                .HasColumnName("region");

                entity.Property(e => e.Provider)
                .HasColumnName("provider");

                entity.Property(e => e.ConnectionString)
                .HasColumnName("connection_string");
            });
        }
    }
}
