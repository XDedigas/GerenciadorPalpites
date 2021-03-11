using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class UsuarioBolaoViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdBolao { get; set; }
        public string NomeUsuario { get; set; }
    }
}