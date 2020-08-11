using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeCampeonatoMap : EntityTypeConfiguration<TimeCampeonatoModel>
    {
        public TimeCampeonatoMap()
        {
            ToTable("TimeCampeonato");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdTime).HasColumnName("IdTime").IsRequired();
            HasRequired(x => x.Time).WithMany().HasForeignKey(x => x.IdTime).WillCascadeOnDelete(false);

            Property(x => x.IdCampeonato).HasColumnName("IdCampeonato").IsRequired();
            HasRequired(x => x.Campeonato).WithMany().HasForeignKey(x => x.IdCampeonato).WillCascadeOnDelete(false);
        }
    }
}