using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class EsporteMap : EntityTypeConfiguration<EsporteModel>
    {
        public EsporteMap()
        {
            ToTable("Esporte");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        }
    }
}