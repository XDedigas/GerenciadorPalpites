using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeMap : EntityTypeConfiguration<TimeModel>
    {
        public TimeMap()
        {
            ToTable("Time");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();
            Property(x => x.IdPais).HasColumnName("idPais").IsRequired();
            HasRequired(x => x.Pais).WithMany().HasForeignKey(x => x.IdPais).WillCascadeOnDelete(false);
            Property(x => x.IdEstado).HasColumnName("idEstado").IsRequired();
            HasRequired(x => x.Estado).WithMany().HasForeignKey(x => x.IdEstado).WillCascadeOnDelete(false);
            Property(x => x.IdCidade).HasColumnName("idCidade").IsRequired();
            HasRequired(x => x.Cidade).WithMany().HasForeignKey(x => x.IdCidade).WillCascadeOnDelete(false);
        }
    }
}