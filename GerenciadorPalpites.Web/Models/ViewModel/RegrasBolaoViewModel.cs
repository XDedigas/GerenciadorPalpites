using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasBolaoViewModel
    {
        public int Id { get; set; }
        public int IdRegra { get; set; }
        public int IdBolao { get; set; }
    }
}