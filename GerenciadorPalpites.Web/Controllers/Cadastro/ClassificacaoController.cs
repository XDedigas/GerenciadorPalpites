using AutoMapper;
using GerenciadorPalpites.Web.Models;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm,Operador")]
    public class ClassificacaoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index(string ordenacao, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.PosicaoSortParam = string.IsNullOrEmpty(ordenacao) ? "posicao desc" : "";
            ViewBag.TotalSortParam = ordenacao == "total" ? "total desc" : "total";
            ViewBag.VariacaoSortParam = ordenacao == "variacao" ? "variacao desc" : "variacao";
            ViewBag.NameSortParam = ordenacao == "nome" ? "nome desc" : "nome";
            ViewBag.PCSortParam = ordenacao == "placarCheio" ? "placarCheio desc" : "placarCheio";
            ViewBag.PVSortParam = ordenacao == "placarVencedor" ? "placarVencedor desc" : "placarVencedor";
            ViewBag.PPSortParam = ordenacao == "placarPerdedor" ? "placarPerdedor desc" : "placarPerdedor";
            ViewBag.AVSortParam = ordenacao == "acertouVencedor" ? "acertouVencedor desc" : "acertouVencedor";

            //ComboBox para definir o tamanho das páginas
            if (tamanhoPagina != null)
                ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, int.Parse(tamanhoPagina));
            else
            {
                ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
                tamanhoPagina = _quantMaxLinhasPorPagina.ToString();
            }

            ViewBag.CurrentPageSize = tamanhoPagina;

            List<ClassificacaoViewModel> lista = Mapper.Map<List<ClassificacaoViewModel>>(ClassificacaoModel.RecuperarLista(filtro: "", ordem: ordenacao));

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ClassificacaoPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }
    }
}