using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class PalpitesMap : EntityTypeConfiguration<PalpitesModel>
    {
        public PalpitesMap()
        {
            ToTable("Palpites");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdTimeCasa).HasColumnName("IdTimeCasa").IsRequired();
            HasRequired(x => x.TimeCasa).WithMany().HasForeignKey(x => x.IdTimeCasa).WillCascadeOnDelete(false);

            Property(x => x.IdTimeFora).HasColumnName("IdTimeFora").IsRequired();
            HasRequired(x => x.TimeFora).WithMany().HasForeignKey(x => x.IdTimeFora).WillCascadeOnDelete(false);

            Property(x => x.IdPartida).HasColumnName("IdPartida").IsRequired();
            HasRequired(x => x.Partida).WithMany().HasForeignKey(x => x.IdPartida).WillCascadeOnDelete(false);

            Property(x => x.IdUsuario).HasColumnName("IdUsuario").IsRequired();
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);

            Property(x => x.IdBolao).HasColumnName("IdBolao").IsRequired();
            HasRequired(x => x.Bolao).WithMany().HasForeignKey(x => x.IdBolao).WillCascadeOnDelete(false);

            Property(x => x.PalpiteTimeCasa).HasColumnName("PalpiteTimeCasa").IsRequired();
            Property(x => x.PalpiteTimeFora).HasColumnName("PalpiteTimeFora").IsRequired();
        }
    }
}