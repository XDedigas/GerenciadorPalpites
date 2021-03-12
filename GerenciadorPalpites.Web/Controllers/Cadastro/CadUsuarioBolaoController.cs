using AutoMapper;
using GerenciadorPalpites.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    [Authorize(Roles = "Gerente,Adm")]
    public class CadUsuarioBolaoController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        private const string _senhaPadrao = "{$127;$188}";

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarUsuario(int id)
        {
            var vm = Mapper.Map<UsuarioBolaoViewModel>(UsuarioBolaoModel.RecuperarPeloId(id));

            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioBolaoModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuarioBolao(UsuarioBolaoViewModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            model.IdUsuario = UsuarioModel.RecuperarIdPeloNome(model.NomeUsuario);

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    if (BolaoModel.IsValidPassword(model.IdBolao, model.Senha))
                    {
                        var vm = Mapper.Map<UsuarioBolaoModel>(model);
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