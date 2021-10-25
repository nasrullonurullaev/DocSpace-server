using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model.Resource
{
    public class ResAuthorsFile
    {
        public string AuthorLogin { get; set; }
        public int FileId { get; set; }
        public bool WriteAccess { get; set; }
    }

    public static class ResAuthorsFileExtension
    {
        public static ModelBuilderWrapper AddResAuthorsFile(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddResAuthorsFile, Provider.MySql)
                .Add(PgSqlAddResAuthorsFile, Provider.Postgre)
                .Add(MSSqlAddResAuthorsFile, Provider.MSSql);
            return modelBuilder;
        }
        public static void MySqlAddResAuthorsFile(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthorsFile>(entity =>
            {
                entity.HasKey(e => new { e.AuthorLogin, e.FileId })
                    .HasName("PRIMARY");

                entity.ToTable("res_authorsfile");

                entity.HasIndex(e => e.FileId)
                    .HasDatabaseName("res_authorsfile_FK2");

                entity.Property(e => e.AuthorLogin)
                    .HasColumnName("authorLogin")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.FileId).HasColumnName("fileid");

                entity.Property(e => e.WriteAccess).HasColumnName("writeAccess");
            });
        }
        public static void PgSqlAddResAuthorsFile(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthorsFile>(entity =>
            {
                entity.HasKey(e => new { e.AuthorLogin, e.FileId })
                    .HasName("res_authorsfile_pkey");

                entity.ToTable("res_authorsfile", "onlyoffice");

                entity.HasIndex(e => e.FileId)
                    .HasDatabaseName("res_authorsfile_FK2");

                entity.Property(e => e.AuthorLogin)
                    .HasColumnName("authorLogin")
                    .HasMaxLength(50);

                entity.Property(e => e.FileId).HasColumnName("fileid");

                entity.Property(e => e.WriteAccess).HasColumnName("writeAccess");
            });
        }

        public static void MSSqlAddResAuthorsFile(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResAuthorsFile>(entity =>
            {
                entity.HasKey(e => new { e.AuthorLogin, e.FileId })
                    .HasName("res_authorsfile_pkey");

                entity.ToTable("res_authorsfile");

                entity.HasIndex(e => e.FileId)
                    .HasDatabaseName("res_authorsfile_FK2");

                entity.Property(e => e.AuthorLogin)
                    .HasColumnName("authorLogin")
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                    .HasMaxLength(50);

                entity.Property(e => e.FileId).HasColumnName("fileid");

                entity.Property(e => e.WriteAccess).HasColumnName("writeAccess");
            });
        }
    }
}
