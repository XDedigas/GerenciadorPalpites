using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(255, ErrorMessage = "O nome pode ter no máximo 255 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Selecione o país.")]
        public int IdPais { get; set; }

        public string NomePais { get; set; }

        public bool Ativo { get; set; }
    }
}