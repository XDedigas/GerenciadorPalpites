using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class CampeonatoRodadaModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdCampeonato { get; set; }
        public int IdRodada { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.CampeonatoRodada.Count();
            }

            return ret;
        }

        public static List<CampeonatoRodadaModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", int idCampeonato = 0, int idRodada = 0)
        {
            var ret = new List<CampeonatoRodadaModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(id) like '%{filtro.ToLower()}%'";
                }

                if (idCampeonato > 0)
                {
                    filtroWhere += $"{(string.IsNullOrEmpty(filtroWhere) ? " where" : " and")} idCampeonato = {idCampeonato}";
                }
                if (idRodada > 0)
                {
                    filtroWhere += $"{(string.IsNullOrEmpty(filtroWhere) ? " where" : " and")} idRodada = {idRodada}";
                }

                var pos = (pagina - 1) * tamPagina;

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = $" offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";
                }

                var sql = $"select * from campeonatorodada{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<CampeonatoRodadaModel>(sql).ToList();
            }

            return ret;
        }

        public static CampeonatoRodadaModel RecuperarPeloId(int id)
        {
            CampeonatoRodadaModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.CampeonatoRodada.Find(id);
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
                    var local = new CampeonatoRodadaModel { Id = id };
                    db.CampeonatoRodada.Attach(local);
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
                    db.CampeonatoRodada.Add(this);
                }
                else
                {
                    db.CampeonatoRodada.Attach(this);
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