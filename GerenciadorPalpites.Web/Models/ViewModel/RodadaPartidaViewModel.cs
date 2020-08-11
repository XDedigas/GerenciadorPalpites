using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class RodadaPartidaViewModel
    {
        public int Id { get; set; }
        public int IdRodada { get; set; }
        public int IdPartida { get; set; }
    }
}