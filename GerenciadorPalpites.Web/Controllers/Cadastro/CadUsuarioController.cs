﻿using AutoMapper;
using GerenciadorPalpites.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm")]
    public class CadUsuarioController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const string _senhaPadrao = "{$127;$188}";

        public ActionResult Index(string ordenacao, string filtro, string termoPesquisa, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.NameSortParam = string.IsNullOrEmpty(ordenacao) ? "nome desc" : "";
            ViewBag.LoginSortParam = ordenacao == "login" ? "login desc" : "login";

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

            List<UsuarioViewModel> lista = Mapper.Map<List<UsuarioViewModel>>(UsuarioModel.RecuperarLista(filtro: termoPesquisa, ordem: ordenacao));

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<UsuarioViewModel>>(UsuarioModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            var vm = Mapper.Map<UsuarioViewModel>(UsuarioModel.RecuperarPeloId(id));
            vm.Senha = _senhaPadrao;

            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioViewModel model)
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
                    if (model.Senha == _senhaPadrao)
                    {
                        model.Senha = "";
                    }

                    var vm = Mapper.Map<UsuarioModel>(model);
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