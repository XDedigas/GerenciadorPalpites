using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasBolaoModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdRegra { get; set; }
        public int IdBolao { get; set; }
        public virtual RegrasModel Regra { get; set; }
        public virtual BolaoModel Bolao { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.RegrasBolao.Count();
            }

            return ret;
        }

        public static List<RegrasBolaoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<RegrasBolaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(id) like '%{filtro.ToLower()}%'";
                }

                var pos = (pagina - 1) * tamPagina;

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = $" offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";
                }

                var sql = $"select * from RegrasBolao{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<RegrasBolaoModel>(sql).ToList();
            }

            return ret;
        }

        public static RegrasBolaoModel RecuperarPeloId(int id)
        {
            RegrasBolaoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.RegrasBolao.Find(id);
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
                    var local = new RegrasBolaoModel { Id = id };
                    db.RegrasBolao.Attach(local);
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
                    db.RegrasBolao.Add(this);
                }
                else
                {
                    db.RegrasBolao.Attach(this);
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