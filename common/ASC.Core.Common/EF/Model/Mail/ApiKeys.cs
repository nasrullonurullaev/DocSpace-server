
using Microsoft.EntityFrameworkCore;

namespace ASC.Core.Common.EF.Model.Mail
{
    public class ApiKeys
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
    }

    public static class ApiKeysExtenshion
    {
        public static ModelBuilderWrapper AddApiKeys(this ModelBuilderWrapper modelBuilder)
        {
            modelBuilder
                .Add(MySqlAddApiKeys, Provider.MySql)
                .Add(PgSqlAddApiKeys, Provider.Postgre)
                .Add(MSSqlAddApiKeys, Provider.MSSql);

            return modelBuilder;
        }

        public static void MySqlAddApiKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKeys>(entity =>
            {
                entity.ToTable("api_keys", "onlyoffice");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.AccessToken)
                .HasColumnName("access_token")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");
            });
        }

        public static void PgSqlAddApiKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKeys>(entity =>
            {
                entity.ToTable("api_keys", "onlyoffice");

                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.AccessToken)
                .HasColumnName("access_token");
            });
        }

        public static void MSSqlAddApiKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiKeys>(entity =>
            {
                entity.ToTable("api_keys");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessToken)
                    .HasColumnName("access_token")
                    .UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8");
            });
        }
    }
}
