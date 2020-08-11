using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasBolaoMap : EntityTypeConfiguration<RegrasBolaoModel>
    {
        public RegrasBolaoMap()
        {
            ToTable("RegraBolao");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.IdBolao).HasColumnName("IdBolao").IsRequired();
            HasRequired(x => x.Bolao).WithMany().HasForeignKey(x => x.IdBolao).WillCascadeOnDelete(false);

            Property(x => x.IdRegra).HasColumnName("IdRegra").IsRequired();
            HasRequired(x => x.Regra).WithMany().HasForeignKey(x => x.IdRegra).WillCascadeOnDelete(false);
        }
    }
}