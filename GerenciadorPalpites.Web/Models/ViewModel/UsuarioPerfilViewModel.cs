using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class UsuarioPerfilViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
    }
}