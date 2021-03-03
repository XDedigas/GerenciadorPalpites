using AutoMapper;
using GerenciadorPalpites.Web.Models;

namespace GerenciadorPalpites.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BolaoViewModel, BolaoModel>();
            CreateMap<CampeonatoViewModel, CampeonatoModel>();
            CreateMap<CampeonatoRodadaViewModel, CampeonatoRodadaModel>();
            CreateMap<CidadeViewModel, CidadeModel>();
            CreateMap<ClassificacaoViewModel, ClassificacaoModel>();
            CreateMap<EsporteViewModel, EsporteModel>();
            CreateMap<EstadioViewModel, EstadioModel>();
            CreateMap<EstadoViewModel, EstadoModel>();
            CreateMap<EstatisticasViewModel, EstatisticasModel>();
            CreateMap<PaisViewModel, PaisModel>();
            CreateMap<PalpitesViewModel, PalpitesModel>();
            CreateMap<PartidaViewModel, PartidaModel>();
            CreateMap<RegrasBolaoViewModel, RegrasBolaoModel>();
            CreateMap<RegrasViewModel, RegrasModel>();
            CreateMap<RodadaViewModel, RodadaModel>();
            CreateMap<RodadaPartidaViewModel, RodadaPartidaModel>();
            CreateMap<TimeViewModel, TimeModel>();
            CreateMap<TimeCampeonatoViewModel, TimeCampeonatoModel>();
            CreateMap<UsuarioViewModel, UsuarioModel>();
            CreateMap<UsuarioBolaoViewModel, UsuarioBolaoModel>();
        }
    }
}