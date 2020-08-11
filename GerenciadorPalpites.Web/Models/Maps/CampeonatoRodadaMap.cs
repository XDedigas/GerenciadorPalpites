using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoRodadaMap : EntityTypeConfiguration<CampeonatoRodadaModel>
    {
        public CampeonatoRodadaMap()
        {
            ToTable("campeonatorodada");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdCampeonato).HasColumnName("idCampeonato").IsRequired();

            Property(x => x.IdRodada).HasColumnName("IdRodada").IsRequired();
        }
    }
}