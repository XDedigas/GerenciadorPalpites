using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class TimeModel
    {
        #region Atributos

        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdPais { get; set; }
        public string NomePais { get; set; }
        public virtual PaisModel Pais { get; set; }
        public int IdEstado { get; set; }
        public virtual EstadoModel Estado { get; set; }
        public int IdCidade { get; set; }
        public virtual CidadeModel Cidade { get; set; }
        public bool Ativo { get; set; }

        #endregion

        #region Métodos

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Time.Count();
            }

            return ret;
        }

        public static List<TimeModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<TimeModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" where lower(Time.Nome) like '%{0}%'", filtro.ToLower());
                }

                var pos = (pagina - 1) * tamPagina;
                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                }

                var sql = "";

                if (!string.IsNullOrEmpty(ordem))
                {
                    if (ordem.ToLower().StartsWith("pais"))
                    {
                        sql = "select Time.id as id, Time.Nome as nome, Pais.Nome as NomePais, idPais as IdPais, idEstado as IdEstado, idCidade as IdCidade from Time left join Pais on Time.idPais = Pais.id" +
                              filtroWhere +
                              $" order by Pais.Nome{ordem.ToLower().Replace("pais", "")}" +
                              paginacao;
                    }
                    else
                        sql =
                    "select Time.id as id, Time.Nome as nome, Pais.Nome as NomePais, idPais as IdPais, idEstado as IdEstado, idCidade as IdCidade from Time left join Pais on Time.idPais = Pais.id" +
                    filtroWhere +
                    $" order by Time.{ordem}" +
                    paginacao;
                }
                else
                    sql =
                    "select Time.id as id, Time.Nome as nome, Pais.Nome as NomePais, idPais as IdPais, idEstado as IdEstado, idCidade as IdCidade from Time left join Pais on Time.idPais = Pais.id" +
                    filtroWhere +
                    " order by nome" +
                    paginacao;

                ret = db.Database.Connection.Query<TimeModel>(sql).ToList();
            }

            return ret;
        }

        public static TimeModel RecuperarPeloId(int id)
        {
            TimeModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Time.Find(id);
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
                    var time = new TimeModel { Id = id };
                    db.Time.Attach(time);
                    db.Entry(time).State = EntityState.Deleted;
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
                    db.Time.Add(this);
                }
                else
                {
                    db.Time.Attach(this);
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