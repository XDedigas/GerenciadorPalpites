using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class EstatisticasModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdTimeA { get; set; }
        public int IdTimeB { get; set; }
        public int VitoriasTimeA { get; set; }
        public int VitoriasTimeB { get; set; }
        public int Empates { get; set; }
        public int Total { get; set; }
        public string NomeTimeA { get; set; }
        public string NomeTimeB { get; set; }
        public virtual TimeModel TimeA { get; set; }
        public virtual TimeModel TimeB { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Estatisticas.Count();
            }

            return ret;
        }

        public static List<EstatisticasModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<EstatisticasModel>();

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

                var sql = $"select * from Estatisticas{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<EstatisticasModel>(sql).ToList();
            }

            return ret;
        }

        public static EstatisticasModel RecuperarPeloId(int id)
        {
            EstatisticasModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Estatisticas.Find(id);
            }

            return ret;
        }

        public static EstatisticasModel RecuperarPeloIdsTimes(int idTime1, int idTime2)
        {
            EstatisticasModel ret = null;

            using (var db = new ContextoBD())
            {
                var sql = $"select e.* from Estatisticas e where (e.idTimeA = {idTime1} and e.idTimeB = {idTime2}) or (e.idTimeA = {idTime2} and e.idTimeB = {idTime1})";
                ret = db.Database.Connection.Query<EstatisticasModel>(sql).FirstOrDefault();

                if (ret == null)
                {
                    ret = new EstatisticasModel()
                    {
                        VitoriasTimeA = 0,
                        VitoriasTimeB = 0,
                        Empates = 0,
                        NomeTimeA = TimeModel.RecuperarNomeTimePeloID(idTime1),
                        NomeTimeB = TimeModel.RecuperarNomeTimePeloID(idTime2)
                    };
                }
                else
                {
                    ret.NomeTimeA = TimeModel.RecuperarNomeTimePeloID(ret.IdTimeA);
                    ret.NomeTimeB = TimeModel.RecuperarNomeTimePeloID(ret.IdTimeB);
                }
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
                    var local = new EstatisticasModel { Id = id };
                    db.Estatisticas.Attach(local);
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
                    db.Estatisticas.Add(this);
                }
                else
                {
                    db.Estatisticas.Attach(this);
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