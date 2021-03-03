using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasMap : EntityTypeConfiguration<RegrasModel>
    {
        public RegrasMap()
        {
            ToTable("Regras");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Pontuacao1).HasColumnName("pontuacao1").IsRequired();
            Property(x => x.Pontuacao2).HasColumnName("pontuacao2").IsRequired();
            Property(x => x.Pontuacao3).HasColumnName("pontuacao3").IsRequired();
        }
    }
}