using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class RodadaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha a descrição da rodada.")]
        [MaxLength(255, ErrorMessage = "A descrição da rodada pode ter no máximo 255 caracteres.")]
        public string Descricao { get; set; }
    }
}