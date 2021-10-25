﻿using System;

using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model.Resource
{
    public class ResAuthors
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool Online { get; set; }
        public DateTime LastVisit { get; set; }
    }
    public static class ResAuthorsExtension
    {
        public static ModelBuilderWrapper AddResAuthors(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddResAuthors, Provider.MySql)
                .Add(PgSqlAddResAuthors, Provider.Postgre)
                .Add(MSSqlAddResAuthors, Provider.MSSql);
            return modelBuilder;
        }
        public static void MySqlAddResAuthors(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthors>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.ToTable("res_authors");

                entity.Property(e => e.Login)
                    .HasColumnName("login")
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.LastVisit)
                    .HasColumnName("lastVisit")
                    .HasColumnType("datetime");

                entity.Property(e => e.Online).HasColumnName("online");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");
            });
        }
        public static void PgSqlAddResAuthors(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthors>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("res_authors_pkey");

                entity.ToTable("res_authors", "onlyoffice");

                entity.Property(e => e.Login)
                    .HasColumnName("login")
                    .HasMaxLength(150);

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.LastVisit).HasColumnName("lastVisit");

                entity.Property(e => e.Online).HasColumnName("online");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);
            });
        }

        public static void MSSqlAddResAuthors(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthors>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("res_authors_pkey");

                entity.ToTable("res_authors");

                entity.Property(e => e.Login)
                    .HasColumnName("login")
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                    .HasMaxLength(150);

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.LastVisit).HasColumnName("lastVisit");

                entity.Property(e => e.Online).HasColumnName("online");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                    .HasMaxLength(50);
            });
        }
    }
}
