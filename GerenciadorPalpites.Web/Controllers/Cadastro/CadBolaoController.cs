﻿using AutoMapper;
using GerenciadorPalpites.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    public class CadBolaoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult MeusBoloes(string ordenacao, string filtro, string termoPesquisa, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.NameSortParam = string.IsNullOrEmpty(ordenacao) ? "nome desc" : "";
            ViewBag.ChampionshipSortParam = ordenacao == "campeonato" ? "campeonato desc" : "campeonato";

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

            List<BolaoViewModel> lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarListaMeusBoloes(User.Identity.Name, filtro: termoPesquisa, ordem: ordenacao));

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        public ActionResult Index(string ordenacao, string filtro, string termoPesquisa, string tamanhoPagina, int? page)
        {
            ViewBag.CurrentSort = ordenacao;
            ViewBag.NameSortParam = string.IsNullOrEmpty(ordenacao) ? "nome desc" : "";
            ViewBag.ChampionshipSortParam = ordenacao == "campeonato" ? "campeonato desc" : "campeonato";

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

            List<BolaoViewModel> lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarLista(User.Identity.Name, filtro: termoPesquisa, ordem: ordenacao));
            ViewBag.Campeonato = Mapper.Map<List<CampeonatoViewModel>>(CampeonatoModel.RecuperarLista());

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SalvarBolao(BolaoViewModel model)
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
                    var bolao = Mapper.Map<BolaoModel>(model);
                    bolao.Publico = !bolao.Publico;
                    var id = bolao.Salvar();

                    if (model.AlterarPontuacao)
                    {
                        CriarRegra(model.PlacarExato, model.AcertouVencedor, model.GolsFeitos, id);
                    }
                    else {
                        RegrasBolaoViewModel regraBolaoModel = new RegrasBolaoViewModel();
                        regraBolaoModel.IdBolao = id;
                        regraBolaoModel.IdRegra = 1;

                        var regraBolao = Mapper.Map<RegrasBolaoModel>(regraBolaoModel);
                        regraBolao.Salvar();
                    }

                    UsuarioBolaoModel usuario = new UsuarioBolaoModel();
                    int idUser = usuario.RecuperarIDPeloNome(User.Identity.Name);

                    UsuarioBolaoViewModel usuarioBolaoModel = new UsuarioBolaoViewModel();
                    usuarioBolaoModel.IdBolao = id;
                    usuarioBolaoModel.IdUsuario = idUser;

                    var usuarioMap = Mapper.Map<UsuarioBolaoModel>(usuarioBolaoModel);
                    usuarioMap.Salvar();

                    if (id > 0)
                    {
                        url = new UrlHelper(Request.RequestContext).Action("Index", "CadBolao");
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

            return Json(new { Resultado = resultado, Mensagens = mensagens, Url = url });
        }
        [HttpPost]
        public JsonResult VerficarBolaoPublico(int idBolao)
        {
            return Json(new { IsPublic = BolaoModel.VerificaBolaoPublico(idBolao) });
        }
        public void CriarRegra(float valor1, float valor2, float valor3, int idBolao)
        {
            RegrasViewModel regraModel = new RegrasViewModel();
            regraModel.Pontuacao1 = valor1;
            regraModel.Pontuacao2 = valor2;
            regraModel.Pontuacao3 = valor3;

            var regra = Mapper.Map<RegrasModel>(regraModel);
            int idRegra = regra.RecuperarIDPelosValores(regraModel);

            if (idRegra == 0)
            {
                idRegra = regra.Salvar();
            }

            RegrasBolaoViewModel regraBolaoModel = new RegrasBolaoViewModel();
            regraBolaoModel.IdBolao = idBolao;
            regraBolaoModel.IdRegra = idRegra;

            var regraBolao = Mapper.Map<RegrasBolaoModel>(regraBolaoModel);
            regraBolao.Salvar();
        }
    }
}