using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class UsuarioPerfilMap : EntityTypeConfiguration<UsuarioPerfilModel>
    {
        public UsuarioPerfilMap()
        {
            ToTable("UsuarioPerfil");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdUsuario).HasColumnName("IdUsuario").IsRequired();
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);

            Property(x => x.IdPerfil).HasColumnName("IdPerfil").IsRequired();
            HasRequired(x => x.Perfil).WithMany().HasForeignKey(x => x.IdPerfil).WillCascadeOnDelete(false);
        }
    }
}