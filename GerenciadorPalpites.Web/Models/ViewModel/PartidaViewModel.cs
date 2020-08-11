using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class PartidaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o Time da Casa.")]
        public int TimeCasa { get; set; }

        [Required(ErrorMessage = "Selecione o Time de Fora.")]
        public int TimeFora { get; set; }

        [Required(ErrorMessage = "Preencha a Data.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Preencha o placar do time da casa.")]
        public int PlacarTimeCasa { get; set; }

        [Required(ErrorMessage = "Preencha o placar do time de fora.")]
        public int PlacarTimeFora { get; set; }

        [Required(ErrorMessage = "Selecione o Campeonato.")]
        public int IdCampeonato { get; set; }
    }
}