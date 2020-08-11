using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeCampeonatoModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdTime { get; set; }
        public int IdCampeonato { get; set; }
        public virtual CampeonatoModel Campeonato { get; set; }
        public virtual TimeModel Time { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.TimeCampeonato.Count();
            }

            return ret;
        }

        public static List<TimeCampeonatoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<TimeCampeonatoModel>();

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

                var sql = $"select * from TimeCampeonato{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<TimeCampeonatoModel>(sql).ToList();
            }

            return ret;
        }

        public static TimeCampeonatoModel RecuperarPeloId(int id)
        {
            TimeCampeonatoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.TimeCampeonato.Find(id);
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
                    var local = new TimeCampeonatoModel { Id = id };
                    db.TimeCampeonato.Attach(local);
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
                    db.TimeCampeonato.Add(this);
                }
                else
                {
                    db.TimeCampeonato.Attach(this);
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