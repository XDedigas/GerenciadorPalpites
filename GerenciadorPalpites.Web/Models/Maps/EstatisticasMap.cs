using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class EstatisticasMap : EntityTypeConfiguration<EstatisticasModel>
    {
        public EstatisticasMap()
        {
            ToTable("Estatisticas");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdTimeA).HasColumnName("IdTimeA").IsRequired();
            HasRequired(x => x.TimeA).WithMany().HasForeignKey(x => x.IdTimeA).WillCascadeOnDelete(false);

            Property(x => x.IdTimeB).HasColumnName("IdTimeB").IsRequired();
            HasRequired(x => x.TimeB).WithMany().HasForeignKey(x => x.IdTimeB).WillCascadeOnDelete(false);

            Property(x => x.VitoriasTimeA).HasColumnName("VitoriasTimeA").IsRequired();
            Property(x => x.VitoriasTimeB).HasColumnName("VitoriasTimeB").IsRequired();
            Property(x => x.Empates).HasColumnName("Empates").IsRequired();
            Property(x => x.Total).HasColumnName("Total").IsRequired();
        }
    }
}