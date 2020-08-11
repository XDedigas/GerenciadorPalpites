using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class EstadioModel
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
                ret = db.Estadios.Count();
            }

            return ret;
        }

        public static List<EstadioModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<EstadioModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(nome) like '%{filtro.ToLower()}%'";
                }

                var pos = (pagina - 1) * tamPagina;

                var sql = $"select * from estadio{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "nome")} offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";

                ret = db.Database.Connection.Query<EstadioModel>(sql).ToList();
            }

            return ret;
        }

        public static List<EstadioModel> RecuperarListaAtivos()
        {
            var ret = new List<EstadioModel>();

            using (var db = new ContextoBD())
            {
                ret = db.Estadios.OrderBy(x => x.Nome).ToList();
            }

            return ret;
        }

        public static EstadioModel RecuperarPeloId(int id)
        {
            EstadioModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Estadios.Where(x => x.Id == id).SingleOrDefault();
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
                    var estadio = new EstadioModel { Id = id };
                    db.Estadios.Attach(estadio);
                    db.Entry(estadio).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = true;
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                var model = db.Estadios.Where(x => x.Id == this.Id).SingleOrDefault();

                if (model == null)
                {
                    db.Estadios.Add(this);
                }
                else
                {
                    model.Nome = this.Nome;
                }

                db.SaveChanges();
                ret = this.Id;
            }

            return ret;
        }

        #endregion
    }
}