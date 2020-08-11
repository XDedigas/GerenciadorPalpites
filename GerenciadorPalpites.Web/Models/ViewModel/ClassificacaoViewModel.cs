﻿using System.ComponentModel.DataAnnotations;

namespace GerenciadorPalpites.Web.Models
{
    public class ClassificacaoViewModel
    {
        public int Id { get; set; }
        public int IdBolao { get; set; }
        public int IdUsuario { get; set; }
        public int Total { get; set; }
        public int PlacarCheio { get; set; }
        public int PlacarVencedor { get; set; }
        public int PlacarPerdedor { get; set; }
        public int Variacao { get; set; }
        public int AcertouVencedor { get; set; }
    }
}