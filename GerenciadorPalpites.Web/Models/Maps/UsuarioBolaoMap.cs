using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class UsuarioBolaoMap : EntityTypeConfiguration<UsuarioBolaoModel>
    {
        public UsuarioBolaoMap()
        {
            ToTable("UsuarioBolao");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdUsuario).HasColumnName("IdUsuario").IsRequired();
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);

            Property(x => x.IdBolao).HasColumnName("IdBolao").IsRequired();
            HasRequired(x => x.Bolao).WithMany().HasForeignKey(x => x.IdBolao).WillCascadeOnDelete(false);
        }
    }
}