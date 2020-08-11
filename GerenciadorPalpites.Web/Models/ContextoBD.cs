using System.Data.Entity;

namespace GerenciadorPalpites.Web.Models
{
    public class ContextoBD : DbContext
    {
        public ContextoBD() : base("name=principal")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BolaoMap());
            modelBuilder.Configurations.Add(new CampeonatoMap());
            modelBuilder.Configurations.Add(new CampeonatoRodadaMap());
            modelBuilder.Configurations.Add(new CidadeMap());
            modelBuilder.Configurations.Add(new ClassificacaoMap());
            modelBuilder.Configurations.Add(new EsporteMap());
            modelBuilder.Configurations.Add(new EstadioMap());
            modelBuilder.Configurations.Add(new EstadoMap());
            modelBuilder.Configurations.Add(new EstatisticasMap());
            modelBuilder.Configurations.Add(new PaisMap());
            modelBuilder.Configurations.Add(new PalpitesMap());
            modelBuilder.Configurations.Add(new PartidaMap());
            modelBuilder.Configurations.Add(new PerfilMap());
            modelBuilder.Configurations.Add(new RegrasBolaoMap());
            modelBuilder.Configurations.Add(new RegrasMap());
            modelBuilder.Configurations.Add(new RodadaMap());
            modelBuilder.Configurations.Add(new RodadaPartidaMap());
            modelBuilder.Configurations.Add(new TimeMap());
            modelBuilder.Configurations.Add(new TimeCampeonatoMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new UsuarioBolaoMap());
            modelBuilder.Configurations.Add(new UsuarioPerfilMap());
        }

        public DbSet<BolaoModel> Bolao { get; set; }
        public DbSet<CampeonatoModel> Campeonato { get; set; }
        public DbSet<CampeonatoRodadaModel> CampeonatoRodada { get; set; }
        public DbSet<CidadeModel> Cidades { get; set; }
        public DbSet<ClassificacaoModel> Classificacao { get; set; }
        public DbSet<EsporteModel> Esporte { get; set; }
        public DbSet<EstadioModel> Estadios { get; set; }
        public DbSet<EstadoModel> Estados { get; set; }
        public DbSet<EstatisticasModel> Estatisticas { get; set; }
        public DbSet<PaisModel> Paises { get; set; }
        public DbSet<PalpitesModel> Palpites { get; set; }
        public DbSet<PartidaModel> Partidas { get; set; }
        public DbSet<PerfilModel> Perfis { get; set; }
        public DbSet<RegrasBolaoModel> RegrasBolao { get; set; }
        public DbSet<RegrasModel> Regras { get; set; }
        public DbSet<RodadaModel> Rodada { get; set; }
        public DbSet<RodadaPartidaModel> RodadaPartida { get; set; }
        public DbSet<TimeModel> Time { get; set; }
        public DbSet<TimeCampeonatoModel> TimeCampeonato { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<UsuarioBolaoModel> UsuarioBolao { get; set; }
        public DbSet<UsuarioPerfilModel> UsuarioPerfil { get; set; }
    }
}