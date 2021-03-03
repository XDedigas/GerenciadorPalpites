using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class ClassificacaoMap : EntityTypeConfiguration<ClassificacaoModel>
    {
        public ClassificacaoMap()
        {
            ToTable("Classificacao");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdBolao).HasColumnName("IdBolao").IsRequired();
            HasRequired(x => x.Bolao).WithMany().HasForeignKey(x => x.IdBolao).WillCascadeOnDelete(false);

            Property(x => x.IdUsuario).HasColumnName("IdUsuario").IsRequired();
            HasRequired(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).WillCascadeOnDelete(false);

            Property(x => x.Total).HasColumnName("Total").IsRequired();
            Property(x => x.PlacarCheio).HasColumnName("PlacarCheio").IsRequired();
            Property(x => x.PlacarVencedor).HasColumnName("PlacarVencedor").IsRequired();
            Property(x => x.PlacarPerdedor).HasColumnName("PlacarPerdedor").IsRequired();
            Property(x => x.Variacao).HasColumnName("Variacao").IsRequired();
            Property(x => x.AcertouVencedor).HasColumnName("AcertouVencedor").IsRequired();
            Property(x => x.Posicao).HasColumnName("Posicao").IsRequired();
            Property(x => x.PosicaoAnterior).HasColumnName("PosicaoAnterior").IsRequired();
        }
    }
}