using AutoMapper;
using GerenciadorPalpites.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm,Operador")]
    public class PalpiteController : Controller
    {
        public ActionResult Index(long? idBolao)
        {
            if (idBolao.HasValue)
            {
                ViewBag.IdBolao = idBolao;
                //Buscar o id do campeonato (a partir do Id do bolão informado)
                var idCampeonato = BolaoModel.RecuperarIdCampeonato(idBolao ?? default(int));
                //Buscar as partidas do campeonato (a partir do id do campeonato recuperado acima e filtrar até uma semana após)
                List<PartidaViewModel> partidas = Mapper.Map<List<PartidaViewModel>>(PartidaModel.RecuperarPartidasParaPalpite(idCampeonato));
                List<PalpitesViewModel> palpites = new List<PalpitesViewModel>();

                foreach (var partida in partidas)
                {
                    PalpitesViewModel palpiteView = null;
                    var palpite = PalpitesModel.RecuperarPalpiteUsuarioPeloIdPartida(User.Identity.Name, partida.Id, idBolao.Value);
                    if (palpite != null)
                    {
                        palpiteView = Mapper.Map<PalpitesViewModel>(palpite);
                        palpiteView.NomeRodada = partida.NomeRodada;
                        palpiteView.NomeTimeCasa = partida.NomeTimeCasa;
                        palpiteView.NomeTimeFora = partida.NomeTimeFora;
                    }
                    else
                    {
                        palpiteView = new PalpitesViewModel
                        {
                            Id = 0,
                            IdPartida = partida.Id,
                            IdTimeCasa = partida.IdTimeCasa,
                            NomeTimeCasa = partida.NomeTimeCasa,
                            IdTimeFora = partida.IdTimeFora,
                            NomeTimeFora = partida.NomeTimeFora,
                            NomeRodada = partida.NomeRodada,
                            PalpiteTimeCasa = 0,
                            PalpiteTimeFora = 0
                        };
                    }

                    //Buscar as últimas 5 partidas entre
                    palpiteView.UltimasPartidas = Mapper.Map<List<PartidaViewModel>>(PartidaModel.RecuperarUltimasPartidasEntreTimes(partida.IdTimeCasa, partida.IdTimeFora));
                    palpites.Add(palpiteView);
                }

                return View(palpites);
            }
            else
            {
                return View(new List<PalpitesViewModel>());
            }
        }

        [HttpPost]
        public JsonResult SalvarPalpites(List<PalpitesViewModel> palpites)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var url = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    int idUser = UsuarioModel.RecuperarIdPeloNome(User.Identity.Name);
                    foreach (var palpite in palpites)
                    {
                        var palpiteMap = Mapper.Map<PalpitesModel>(palpite);
                        palpiteMap.IdUsuario = idUser;
                        palpiteMap.Salvar();
                    }
                }
                catch
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens });
        }
    }
}