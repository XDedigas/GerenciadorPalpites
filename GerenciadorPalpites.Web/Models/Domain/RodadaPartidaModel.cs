using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class RodadaPartidaModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdRodada { get; set; }
        public int IdPartida { get; set; }
        public virtual RodadaModel Rodada { get; set; }
        public virtual PartidaModel Partida { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.RodadaPartida.Count();
            }

            return ret;
        }

        public static List<RodadaPartidaModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<RodadaPartidaModel>();

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

                var sql = $"select * from RodadaPartida{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<RodadaPartidaModel>(sql).ToList();
            }

            return ret;
        }

        public static RodadaPartidaModel RecuperarPeloId(int id)
        {
            RodadaPartidaModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.RodadaPartida.Find(id);
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
                    var local = new RodadaPartidaModel { Id = id };
                    db.RodadaPartida.Attach(local);
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
                    db.RodadaPartida.Add(this);
                }
                else
                {
                    db.RodadaPartida.Attach(this);
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