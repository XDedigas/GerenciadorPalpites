using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoRodadaViewModel
    {
        public int Id { get; set; }
        public int IdCampeonato { get; set; }
        public int IdRodada { get; set; }
    }
}