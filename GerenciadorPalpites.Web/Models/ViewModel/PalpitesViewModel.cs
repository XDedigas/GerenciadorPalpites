using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class PalpitesViewModel
    {
        public int Id { get; set; }
        public int IdTimeCasa { get; set; }
        public string NomeTimeCasa { get; set; }
        public int IdTimeFora { get; set; }
        public string NomeTimeFora { get; set; }
        public int IdBolao { get; set; }
        public int IdUsuario { get; set; }
        public int PalpiteTimeCasa { get; set; }
        public int PalpiteTimeFora { get; set; }
        public int IdPartida { get; set; }
        public string NomeRodada { get; set; }
        public bool Contabilizado { get; set; }
        public List<PartidaViewModel> UltimasPartidas { get; set; }
    }
}