using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class PartidaMap : EntityTypeConfiguration<PartidaModel>
    {
        public PartidaMap()
        {
            ToTable("partida");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdTimeCasa).HasColumnName("idTimeCasa").IsRequired();
            HasRequired(x => x.TimeCasa).WithMany().HasForeignKey(x => x.IdTimeCasa).WillCascadeOnDelete(false);

            Property(x => x.IdTimeFora).HasColumnName("idTimeFora").IsRequired();
            HasRequired(x => x.TimeFora).WithMany().HasForeignKey(x => x.IdTimeFora).WillCascadeOnDelete(false);

            Property(x => x.IdCampeonato).HasColumnName("idCampeonato").IsRequired();
            HasRequired(x => x.Campeonato).WithMany().HasForeignKey(x => x.IdCampeonato).WillCascadeOnDelete(false);

            Property(x => x.Data).HasColumnName("data").IsRequired();
            Property(x => x.PlacarTimeCasa).HasColumnName("placarTimeCasa").IsRequired();
            Property(x => x.PlacarTimeFora).HasColumnName("placarTimeFora").IsRequired();
        }
    }
}