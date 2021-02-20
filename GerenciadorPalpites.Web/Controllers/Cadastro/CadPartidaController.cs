using AutoMapper;
using GerenciadorPalpites.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm,Operador")]
    public class CadPartidaController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index(string ordenacao, string filtro, string termoPesquisa, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.DateSort = string.IsNullOrEmpty(ordenacao) ? "data desc" : "";
            ViewBag.HomeTeamSort = ordenacao == "timeCasa" ? "timeCasa desc" : "timeCasa";
            ViewBag.HomeScoreSort = ordenacao == "placarTimeCasa" ? "placarTimeCasa desc" : "placarTimeCasa";
            ViewBag.AwayTeamSort = ordenacao == "timeFora" ? "timeFora desc" : "timeFora";
            ViewBag.AwayScoreSort = ordenacao == "placarTimeFora" ? "placarTimeFora desc" : "placarTimeFora";
            ViewBag.ChampionshipSort = ordenacao == "campeonato" ? "campeonato desc" : "campeonato";

            //ComboBox para definir o tamanho das páginas
            if (tamanhoPagina != null)
                ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, int.Parse(tamanhoPagina));
            else
            {
                ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
                tamanhoPagina = _quantMaxLinhasPorPagina.ToString();
            }

            ViewBag.CurrentPageSize = tamanhoPagina;

            if (termoPesquisa != null)
                page = 1;
            else
                termoPesquisa = filtro;

            ViewBag.CurrentFilter = termoPesquisa;

            List<PartidaViewModel> lista = Mapper.Map<List<PartidaViewModel>>(PartidaModel.RecuperarLista(filtro: termoPesquisa, ordem: ordenacao));

            foreach (var item in lista)
                item.DataFormatada = item.Data.ToString(new CultureInfo("pt-BR"));

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartidaPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<PartidaViewModel>>(PartidaModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarPartida(int id)
        {
            var vm = Mapper.Map<PartidaViewModel>(PartidaModel.RecuperarPeloId(id));

            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarQuantidadeCampeonato(int id)
        {
            var model = PartidaModel.RecuperarPeloId(id);
            if (model != null)
            {
                return Json(new { OK = true, Result = model.IdCampeonato });
            }
            else
            {
                return Json(new { OK = false });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Adm")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirPartida(int id)
        {
            return Json(PartidaModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarPartida()
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            var model = new PartidaModel()
            {
                Id = Int32.Parse(Request.Form["Id"]),
                IdTimeCasa = Int32.Parse(Request.Form["IdTimeCasa"]),
                IdTimeFora = Int32.Parse(Request.Form["IdTimeFora"]),
                IdCampeonato = Int32.Parse(Request.Form["IdCampeonato"]),
                Data = DateTime.Parse(Request.Form["Data"]),                
                PlacarTimeCasa = Int32.Parse(Request.Form["PlacarTimeCasa"]),
                PlacarTimeFora = Int32.Parse(Request.Form["PlacarTimeFora"])
            };

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}