using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class RodadaMap : EntityTypeConfiguration<RodadaModel>
    {
        public RodadaMap()
        {
            ToTable("Rodada");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Descricao).HasColumnName("descricao").HasMaxLength(255).IsRequired();
        }
    }
}