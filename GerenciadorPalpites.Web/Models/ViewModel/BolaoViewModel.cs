using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class BolaoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(255, ErrorMessage = "O nome pode ter no máximo 255 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Preencha o campeonato.")]
        public int IdCampeonato { get; set; }
        public string Senha { get; set; }
        public bool Publico { get; set; }
        public string NomeCampeonato { get; set; }
        public CampeonatoModel Campeonato { get; set; }
        public bool AlterarPontuacao { get; set; }
        public int PlacarExato { get; set; }
        public int AcertouVencedor { get; set; }
        public int GolsFeitos { get; set; }
    }
}