using AutoMapper;
using GerenciadorPalpites.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GerenciadorPalpites.Web.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var usuario = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (usuario != null)
            {
                var tiket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                    1, usuario.Nome, DateTime.Now, DateTime.Now.AddHours(12), login.LembrarMe, usuario.Id.ToString()));
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, tiket);
                Response.Cookies.Add(cookie);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login inválido.");
            }

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult AlterarSenhaUsuario(AlteracaoSenhaUsuarioViewModel model)
        {
            ViewBag.Mensagem = null;

            if (HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var usuarioLogado = (HttpContext.User as AplicacaoPrincipal);
                var alterou = false;
                if (usuarioLogado != null)
                {
                    if (!usuarioLogado.Dados.ValidarSenhaAtual(model.SenhaAtual))
                    {
                        ModelState.AddModelError("SenhaAtual", "A senha atual não confere.");
                    }
                    else
                    {
                        alterou = usuarioLogado.Dados.AlterarSenha(model.NovaSenha);

                        if (alterou)
                        {
                            ViewBag.Mensagem = new string[] { "ok", "Senha alterada com sucesso." };
                        }
                        else
                        {
                            ViewBag.Mensagem = new string[] { "erro", "Não foi possível alterar a senha." };
                        }
                    }
                }

                return View();
            }
            else
            {
                ModelState.Clear();
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult CadastrarSe(UsuarioViewModel model)
        {
            ViewBag.Mensagem = null;

            if (HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var mensagens = new List<string>();
                const string _senhaPadrao = "{$127;$188}";

                if (!ModelState.IsValid)
                {
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
                            ViewBag.Mensagem = new string[] { "ok", "Cadastro realizado com sucesso." };
                        }
                        else
                        {
                            ViewBag.Mensagem = new string[] { "erro", "Não foi possível efetuar o cadastro." };
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.Mensagem = new string[] { "erro", "Não foi possível efetuar o cadastro." };
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.Clear();
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult EsqueciMinhaSenha(EsqueciMinhaSenhaViewModel model)
        {
            ViewBag.EmailEnviado = true;
            if (HttpContext.Request.HttpMethod.ToUpper() == "GET")
            {
                ViewBag.EmailEnviado = false;
                ModelState.Clear();
            }
            else
            {
                var usuario = UsuarioModel.RecuperarPeloLogin(model.Login);
                if (usuario != null)
                {
                    EnviarEmailRedefinicaoSenha(usuario);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RedefinirSenha(int id)
        {
            var usuario = UsuarioModel.RecuperarPeloId(id);
            if (usuario == null)
            {
                id = -1;
            }

            var model = new NovaSenhaViewModel() { Usuario = id };

            ViewBag.Mensagem = null;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RedefinirSenha(NovaSenhaViewModel model)
        {
            ViewBag.Mensagem = null;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = UsuarioModel.RecuperarPeloId(model.Usuario);
            if (usuario != null)
            {
                var ok = usuario.AlterarSenha(model.Senha);
                ViewBag.Mensagem = ok ? "Senha alterada com sucesso!" : "Não foi possível alterar a senha!";
            }

            return View();
        }

        private void EnviarEmailRedefinicaoSenha(UsuarioModel usuario)
        {
            var callbackUrl = Url.Action("RedefinirSenha", "Conta", new { id = usuario.Id }, protocol: Request.Url.Scheme);
            var client = new SmtpClient()
            {
                Host = ConfigurationManager.AppSettings["EmailServidor"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPorta"]),
                EnableSsl = (ConfigurationManager.AppSettings["EmailSsl"] == "S"),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["EmailUsuario"],
                    ConfigurationManager.AppSettings["EmailSenha"])
            };

            var mensagem = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["EmailOrigem"], "Gerenciador de Palpites")
            };
            mensagem.To.Add(usuario.Email);
            mensagem.Subject = "Redefinição de senha";
            mensagem.Body = $"Redefina a sua senha <a href='{callbackUrl}'>aqui</a>";
            mensagem.IsBodyHtml = true;

            client.Send(mensagem);
        }
    }
}