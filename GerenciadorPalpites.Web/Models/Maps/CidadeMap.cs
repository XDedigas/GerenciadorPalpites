using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class CidadeMap : EntityTypeConfiguration<CidadeModel>
    {
        public CidadeMap()
        {
            ToTable("cidade");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(30).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();
            Property(x => x.IdPais).HasColumnName("idPais").IsRequired();

            Property(x => x.IdEstado).HasColumnName("idEstado").IsRequired();
            HasRequired(x => x.Estado).WithMany().HasForeignKey(x => x.IdEstado).WillCascadeOnDelete(false);
        }
    }
}