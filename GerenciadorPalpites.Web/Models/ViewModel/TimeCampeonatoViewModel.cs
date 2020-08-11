using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeCampeonatoViewModel
    {
        public int Id { get; set; }
        public int IdTime { get; set; }
        public int IdCampeonato { get; set; }
    }
}