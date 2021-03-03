using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasViewModel
    {
        public int Id { get; set; }
        public float Pontuacao1 { get; set; }
        public float Pontuacao2 { get; set; }
        public float Pontuacao3 { get; set; }
    }
}