using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdEsporte { get; set; }
        public virtual EsporteModel Esporte { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Campeonato.Count();
            }

            return ret;
        }

        public static List<CampeonatoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", int idEsporte = 0)
        {
            var ret = new List<CampeonatoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(c.Nome) like '%{filtro.ToLower()}%'";
                }

                if (idEsporte > 0)
                {
                    filtroWhere += $"{(string.IsNullOrEmpty(filtroWhere) ? " where" : " and")} c.idEsporte = {idEsporte}";
                }

                var pos = (pagina - 1) * tamPagina;

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = $" offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";
                }

                var sql = $"select c.id as id, c.Nome as Nome, c.idEsporte as idEsporte from campeonato c{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "c.Nome")}{paginacao}";

                ret = db.Database.Connection.Query<CampeonatoModel>(sql).ToList();

                foreach (var item in ret)
                {
                    item.Esporte = EsporteModel.RecuperarPeloId(item.IdEsporte);
                }
            }

            return ret;
        }

        public static CampeonatoModel RecuperarPeloId(int id)
        {
            CampeonatoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Campeonato.Find(id);
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
                    var local = new CampeonatoModel { Id = id };
                    db.Campeonato.Attach(local);
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
                    db.Campeonato.Add(this);
                }
                else
                {
                    db.Campeonato.Attach(this);
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