using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha a descrição da regra.")]
        [MaxLength(255, ErrorMessage = "A descrição da regra pode ter no máximo 255 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Preencha a pontuação da regra.")]
        public float Pontuacao { get; set; }
    }
}