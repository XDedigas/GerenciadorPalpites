﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class UsuarioModel
    {
        #region Atributos

        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        #endregion

        #region Métodos

        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;
            senha = CriptoHelper.HashMD5(senha);

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios
                    .Where(x => x.Login == login && x.Senha == senha)
                    .SingleOrDefault();
            }

            return ret;
        }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios.Count();
            }

            return ret;
        }

        public static List<UsuarioModel> RecuperarLista(int pagina = -1, int tamPagina = -1, string filtro = "", string ordem = "")
        {
            var ret = new List<UsuarioModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" where lower(nome) like '%{0}%' or lower(login) like '%{0}%'", filtro.ToLower());
                }

                var pos = (pagina - 1) * tamPagina;
                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                }

                var sql =
                    "select * from usuario" +
                    filtroWhere +
                    $" order by {(string.IsNullOrEmpty(ordem) ? "nome" : ordem)}" +
                    paginacao;

                ret = db.Database.Connection.Query<UsuarioModel>(sql).ToList();
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios.Find(id);
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloLogin(string login)
        {
            UsuarioModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios
                    .Where(x => x.Login == login)
                    .SingleOrDefault();
            }

            return ret;
        }

        public static int RecuperarIdPeloNome(string nome)
        {
            int ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios.Where(x => x.Nome.ToLower() == nome.ToLower()).SingleOrDefault()?.Id ?? 0;
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var db = new ContextoBD())
                {
                    var usuario = new UsuarioModel { Id = id };
                    db.Usuarios.Attach(usuario);
                    db.Entry(usuario).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var db = new ContextoBD())
            {
                if (model == null)
                {
                    if (!string.IsNullOrEmpty(this.Senha))
                    {
                        this.Senha = CriptoHelper.HashMD5(this.Senha);
                    }
                    db.Usuarios.Add(this);
                }
                else
                {
                    db.Usuarios.Attach(this);
                    db.Entry(this).State = EntityState.Modified;

                    if (string.IsNullOrEmpty(this.Senha))
                    {
                        db.Entry(this).Property(x => x.Senha).IsModified = false;
                    }
                    else
                    {
                        this.Senha = CriptoHelper.HashMD5(this.Senha);
                    }
                }

                db.SaveChanges();
                ret = this.Id;
            }

            return ret;
        }

        public bool ValidarSenhaAtual(string senhaAtual)
        {
            var ret = false;

            string senhaCripto = CriptoHelper.HashMD5(senhaAtual);

            using (var db = new ContextoBD())
            {
                ret = db.Usuarios
                    .Where(x => x.Senha == senhaCripto && x.Id == this.Id)
                    .Any();
            }

            return ret;
        }

        public bool AlterarSenha(string novaSenha)
        {
            var ret = false;

            using (var db = new ContextoBD())
            {
                this.Senha = CriptoHelper.HashMD5(novaSenha);
                db.Usuarios.Attach(this);
                db.Entry(this).Property(x => x.Senha).IsModified = true;
                ret = Convert.ToBoolean(db.SaveChanges());

                return ret;
            }
        }

        #endregion
    }
}