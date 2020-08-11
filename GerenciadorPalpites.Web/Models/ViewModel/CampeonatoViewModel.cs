using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Preencha o esporte.")]
        public int IdEsporte { get; set; }
        public EsporteModel Esporte { get; set; }
    }
}