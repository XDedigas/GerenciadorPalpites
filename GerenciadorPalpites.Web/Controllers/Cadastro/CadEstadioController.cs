﻿using AutoMapper;
using GerenciadorPalpites.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers.Cadastro
{
    [Authorize(Roles = "Gerente")]
    public class CadEstadioController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = Mapper.Map<List<EstadioViewModel>>(EstadioModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina));
            var quant = EstadioModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EstadioPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<EstadioViewModel>>(EstadioModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarEstadio(int id)
        {
            var ret = Mapper.Map<EstadioViewModel>(EstadioModel.RecuperarPeloId(id));
            return Json(ret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirEstadio(int id)
        {
            return Json(EstadioModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarEstadio(EstadioViewModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var vm = Mapper.Map<EstadioModel>(model);
                    var id = vm.Salvar();
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