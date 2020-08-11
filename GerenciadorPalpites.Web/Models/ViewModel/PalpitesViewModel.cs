using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class PalpitesViewModel
    {
        public int Id { get; set; }
        public int IdTimeCasa { get; set; }
        public int IdTimeFora { get; set; }
        public int IdTimeBolao { get; set; }
        public int IdTimeParticipante { get; set; }
        public int PalpiteTimeCasa { get; set; }
        public int PalpiteTimeFora { get; set; }
        public int IdPartida { get; set; }
    }
}