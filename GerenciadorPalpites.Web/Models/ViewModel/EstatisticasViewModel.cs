using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class EstatisticasViewModel
    {
        public int Id { get; set; }
        public int IdTimeA { get; set; }
        public int IdTimeB { get; set; }
        public int VitoriasTimeA { get; set; }
        public int VitoriasTimeB { get; set; }
        public int Empates { get; set; }
        public int Total { get; set; }
        public string NomeTimeA { get; set; }
        public string NomeTimeB { get; set; }
    }
}