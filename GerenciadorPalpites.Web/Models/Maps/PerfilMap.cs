using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class PerfilMap : EntityTypeConfiguration<PerfilModel>
    {
        public PerfilMap()
        {
            ToTable("perfil");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            Property(x => x.Ativo).HasColumnName("ativo").IsRequired();

            HasMany(x => x.Usuarios).WithMany(x => x.Perfis)
                .Map(x =>
                {
                    x.ToTable("usuarioperfil");
                    x.MapLeftKey("idPerfil");
                    x.MapRightKey("idUsuario");
                });
        }
    }
}