﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoMap : EntityTypeConfiguration<CampeonatoModel>
    {
        public CampeonatoMap()
        {
            ToTable("Campeonato");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            Property(x => x.IdEsporte).HasColumnName("IdEsporte").IsRequired();
            HasRequired(x => x.Esporte).WithMany().HasForeignKey(x => x.IdEsporte).WillCascadeOnDelete(false);
        }
    }
}