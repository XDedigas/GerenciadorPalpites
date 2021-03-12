using AutoMapper;
using GerenciadorPalpites.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm,Operador")]
    public class CadTimeController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index(string ordenacao, string filtro, string termoPesquisa, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.NameSortParam = string.IsNullOrEmpty(ordenacao) ? "nome desc" : "";
            ViewBag.CountrySortParam = ordenacao == "pais" ? "pais desc" : "pais";
            ViewBag.SportSortParam = ordenacao == "Esporte.Nome" ? "Esporte.Nome desc" : "Esporte.Nome";

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

            //Realiza a busca completa (retorna todos os registros)
            List<TimeViewModel> lista = Mapper.Map<List<TimeViewModel>>(TimeModel.RecuperarLista(filtro: termoPesquisa, ordem: ordenacao));

            var times = TimeModel.RecuperarLista();
            times.Insert(0, new TimeModel { Id = -1, Nome = "Informe o time..." });
            ViewBag.Times = new SelectList(times, "Id", "Nome", "-1");

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }
        [HttpPost]
        public JsonResult RecuperarEstatisticaTime(string idTime1, string idTime2)
        {
            var vm = Mapper.Map<EstatisticasViewModel>(EstatisticasModel.RecuperarPeloIdsTimes(int.Parse(idTime1), int.Parse(idTime2)));
            return Json(vm);
        }
    }
}