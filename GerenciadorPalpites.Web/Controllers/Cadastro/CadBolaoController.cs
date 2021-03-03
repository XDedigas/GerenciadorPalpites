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
    public class CadBolaoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult InicioBolao(int id)
        {
            var lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarBolaoPeloID(id));
            return View(lista);
        }

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

            List<BolaoViewModel> lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarLista(filtro: termoPesquisa, ordem: ordenacao));
            ViewBag.Campeonato = Mapper.Map<List<CampeonatoViewModel>>(CampeonatoModel.RecuperarLista());

            int pageNumber = (page ?? 1);
            //Retorna os registros paginados
            return View(lista.ToPagedList(pageNumber, int.Parse(tamanhoPagina)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BolaoPagina(int pagina, int tamPag, string filtro, string ordem)
        {
            var lista = Mapper.Map<List<BolaoViewModel>>(BolaoModel.RecuperarLista(pagina, tamPag, filtro, ordem));

            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RecuperarBolao(int id)
        {
            var vm = Mapper.Map<BolaoViewModel>(BolaoModel.RecuperarPeloId(id));

            return Json(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Adm")]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirBolao(int id)
        {
            return Json(BolaoModel.ExcluirPeloId(id));
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