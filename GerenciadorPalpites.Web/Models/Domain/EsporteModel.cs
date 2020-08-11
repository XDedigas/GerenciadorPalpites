using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class EsporteModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Esporte.Count();
            }

            return ret;
        }

        public static List<EsporteModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<EsporteModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(nome) like '%{filtro.ToLower()}%'";
                }

                var pos = (pagina - 1) * tamPagina;

                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = $" offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";
                }

                var sql = $"select * from esporte{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "nome")}{paginacao}";

                ret = db.Database.Connection.Query<EsporteModel>(sql).ToList();
            }

            return ret;
        }

        public static EsporteModel RecuperarPeloId(int id)
        {
            EsporteModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Esporte.Find(id);
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
                    var marcas = new EsporteModel { Id = id };
                    db.Esporte.Attach(marcas);
                    db.Entry(marcas).State = EntityState.Deleted;
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
                    db.Esporte.Add(this);
                }
                else
                {
                    db.Esporte.Attach(this);
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