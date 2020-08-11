using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class RodadaPartidaMap : EntityTypeConfiguration<RodadaPartidaModel>
    {
        public RodadaPartidaMap()
        {
            ToTable("rodadapartida");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdPartida).HasColumnName("IdPartida").IsRequired();
            HasRequired(x => x.Partida).WithMany().HasForeignKey(x => x.IdPartida).WillCascadeOnDelete(false);

            Property(x => x.IdRodada).HasColumnName("IdRodada").IsRequired();
            HasRequired(x => x.Rodada).WithMany().HasForeignKey(x => x.IdRodada).WillCascadeOnDelete(false);
        }
    }
}