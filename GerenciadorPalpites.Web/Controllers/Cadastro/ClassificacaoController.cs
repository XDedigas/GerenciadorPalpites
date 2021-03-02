using AutoMapper;
using GerenciadorPalpites.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm,Operador")]
    public class ClassificacaoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index(int idBolao)
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.ListaTipoFiltro = new SelectList(new string[] { "Geral", "Por Mês", "Por Data" }, "Geral");
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = Mapper.Map<List<ClassificacaoViewModel>>(ClassificacaoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina, idBolao.ToString(), "Total"));
            var quant = ClassificacaoModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
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