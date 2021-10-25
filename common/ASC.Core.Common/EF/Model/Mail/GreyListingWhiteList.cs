
using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model.Mail
{
    [Keyless]
    public class GreyListingWhiteList
    {
        public string Comment { get; set; }
        public string Source { get; set; }
    }

    public static class GreyListingWhiteListExtensions
    {
        public static ModelBuilderWrapper AddGreyListingWhiteList(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddGreyListingWhiteList, Provider.MySql)
                .Add(PgSqlAddGreyListingWhiteList, Provider.Postgre)
                .Add(MSSqlAddGreyListingWhiteList, Provider.MSSql);

            return modelBuilder;
        }

        public static void MySqlAddGreyListingWhiteList(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GreyListingWhiteList>()
                .HasKey(e => e.Comment)
                .HasName("comment_pk");

            modelBuilder.Entity<GreyListingWhiteList>(entity =>
            {
                entity.ToTable("greylisting_whitelist", "onlyoffice");

                entity.Property(e => e.Comment)
                .HasColumnName("comment")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

                entity.Property(e => e.Source)
                .HasColumnName("source")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");
            });
        }

        public static void PgSqlAddGreyListingWhiteList(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GreyListingWhiteList>()
                .HasKey(e => e.Comment)
                .HasName("comment_pk");

            modelBuilder.Entity<GreyListingWhiteList>(entity =>
            {
                entity.ToTable("greylisting_whitelist", "onlyoffice");

                entity.Property(e => e.Comment)
                .HasColumnName("comment");

                entity.Property(e => e.Source)
                .HasColumnName("source");
            });
        }

        public static void MSSqlAddGreyListingWhiteList(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GreyListingWhiteList>(entity =>
            {
                entity.HasKey(e => e.Comment).HasName("comment_pk");

                entity.ToTable("greylisting_whitelist");

                entity.Property(e => e.Comment)
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                    .HasColumnName("comment");

                entity.Property(e => e.Source)
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                    .HasColumnName("source");
            });
        }
    }
}
