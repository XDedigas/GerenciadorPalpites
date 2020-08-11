using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class BolaoMap : EntityTypeConfiguration<BolaoModel>
    {
        public BolaoMap()
        {
            ToTable("Bolao");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome).HasColumnName("Nome").HasMaxLength(255).IsRequired();
            Property(x => x.Senha).HasColumnName("Senha").HasMaxLength(255);
            Property(x => x.Publico).HasColumnName("Publico");

            Property(x => x.IdCampeonato).HasColumnName("idCampeonato").IsRequired();
            HasRequired(x => x.Campeonato).WithMany().HasForeignKey(x => x.IdCampeonato).WillCascadeOnDelete(false);
        }
    }
}