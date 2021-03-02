﻿using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class ClassificacaoModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdBolao { get; set; }
        public int IdUsuario { get; set; }
        public int Total { get; set; }
        public int PlacarCheio { get; set; }
        public int PlacarVencedor { get; set; }
        public int PlacarPerdedor { get; set; }
        public int Variacao { get; set; }
        public int AcertouVencedor { get; set; }
        public string NomeUsuario { get; set; }
        public int Posicao { get; set; }
        public virtual BolaoModel Bolao { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Classificacao.Count();
            }

            return ret;
        }

        public static List<ClassificacaoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<ClassificacaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where idBolao = {filtro}";
                }

                var pos = (pagina - 1) * tamPagina;

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = $" offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";
                }

                var sql = $"select c.*, u.Nome as NomeUsuario from Classificacao c left join Usuario u on(u.id = c.idUsuario){filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")} desc{paginacao}";

                ret = db.Database.Connection.Query<ClassificacaoModel>(sql).ToList();
                int i = 1;

                foreach (var item in ret)
                {
                    item.Bolao = BolaoModel.RecuperarPeloId(item.IdBolao);
                    item.Usuario = UsuarioModel.RecuperarPeloId(item.IdUsuario);
                    item.Posicao = i;
                    i++;
                }
            }

            return ret;
        }

        public static ClassificacaoModel RecuperarPeloId(int id)
        {
            ClassificacaoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Classificacao.Find(id);
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
                    var local = new ClassificacaoModel { Id = id };
                    db.Classificacao.Attach(local);
                    db.Entry(local).State = EntityState.Deleted;
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
                    db.Classificacao.Add(this);
                }
                else
                {
                    db.Classificacao.Attach(this);
                    db.Entry(this).State = EntityState.Modified;
                }

                db.SaveChanges();
                ret = this.Id;
            }

            return ret;
        }
        #endregion
    }
}