using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class BolaoModel
    {
        #region Atributos
        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdCampeonato { get; set; }
        public string Senha { get; set; }
        public bool Publico { get; set; }
        public virtual CampeonatoModel Campeonato { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Bolao.Count();
            }

            return ret;
        }

        public static List<BolaoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<BolaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(b.nome) like '%{filtro.ToLower()}%'";
                }

                var pos = (pagina - 1) * tamPagina;

                var sql = $"select b.Id as id,b.Nome as Nome, b.idCampeonato as idCampeonato, b.Senha as Senha, b.Publico as Publico " +
                    $"from bolao b{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "b.Nome")} offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";

                ret = db.Database.Connection.Query<BolaoModel>(sql).ToList();

                foreach (var item in ret)
                {
                    item.Campeonato = CampeonatoModel.RecuperarPeloId(item.IdCampeonato);
                }
            }

            return ret;
        }

        public static List<BolaoModel> RecuperarListaMeusBoloes(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<BolaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where u.Login = '{filtro}'";
                }

                var pos = (pagina - 1) * tamPagina;

                var sql = $"select * from bolao where id in(select idBolao from usuariobolao left join Usuario u on(u.id = usuariobolao.idUsuario){filtroWhere}) order by {(!string.IsNullOrEmpty(ordem) ? ordem : "Id")} offset {(pos > 0 ? pos - 1 : 0)} rows fetch next {tamPagina} rows only";

                ret = db.Database.Connection.Query<BolaoModel>(sql).ToList();

                foreach (var item in ret)
                {
                    item.Campeonato = CampeonatoModel.RecuperarPeloId(item.IdCampeonato);
                }
            }

            return ret;
        }
        public static List<BolaoModel> RecuperarBolaoPeloID(int id)
        {
            var ret = new BolaoModel();

            using (var db = new ContextoBD())
            {
                ret = db.Bolao.Find(id);
            }

            List<BolaoModel> listBolao = new List<BolaoModel>();

            listBolao.Add(ret);

            return listBolao;
        }

        public static BolaoModel RecuperarPeloId(int id)
        {
            BolaoModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Bolao.Find(id);
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
                    var grupo = new BolaoModel { Id = id };
                    db.Bolao.Attach(grupo);
                    db.Entry(grupo).State = EntityState.Deleted;
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
                    db.Bolao.Add(this);
                }
                else
                {
                    db.Bolao.Attach(this);
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