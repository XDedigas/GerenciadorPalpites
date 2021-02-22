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
        public string NomeCampeonato { get; set; }
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

        public static List<BolaoModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<BolaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where lower(b.nome) like '%{filtro.ToLower()}%' or lower(c.nome) like '%{filtro.ToLower()}%'";
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
                    if (ordem.ToLower().StartsWith("campeonato"))
                    {
                        sql =
                            "select b.id as id, b.Nome as Nome, b.idCampeonato as idCampeonato, b.Senha as Senha, b.Publico as Publico, c.Nome as NomeCampeonato" +
                            " from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                            filtroWhere +
                            $" order by c.nome{ordem.ToLower().Replace("campeonato", "")}" +
                            paginacao;
                    }
                    else
                    {
                        sql =
                            "select b.id as id, b.Nome as Nome, b.idCampeonato as idCampeonato, b.Senha as Senha, b.Publico as Publico, c.Nome as NomeCampeonato" +
                            " from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                            filtroWhere +
                            $" order by b.{ordem}" +
                            paginacao;
                    }
                }
                else
                    sql =
                            "select b.id as id, b.Nome as Nome, b.idCampeonato as idCampeonato, b.Senha as Senha, b.Publico as Publico, c.Nome as NomeCampeonato" +
                            " from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                            filtroWhere +
                            $" order by b.nome" +
                            paginacao;

                ret = db.Database.Connection.Query<BolaoModel>(sql).ToList();
            }

            return ret;
        }

        public static List<BolaoModel> RecuperarListaMeusBoloes(string nomeUsuario, int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<BolaoModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" and (lower(b.Nome) like '%{filtro.ToLower()}%' or lower(c.Nome) like '%{filtro.ToLower()}%')";
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
                    if (ordem.ToLower().StartsWith("campeonato"))
                    {
                        sql =
                        $"select b.id as id, b.Nome as Nome, c.Nome as NomeCampeonato from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                        " where b.id in (" +
                            $"select idBolao from usuariobolao left join Usuario u on(u.id = usuariobolao.idUsuario) where u.Login = '{nomeUsuario}'" +
                        ")" +
                        filtroWhere +
                        $" order by c.nome{ordem.ToLower().Replace("campeonato", "")}" +
                        paginacao;
                    }
                    else
                    {
                        sql =
                           $"select b.id as id, b.Nome as Nome, c.Nome as NomeCampeonato from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                           " where b.id in (" +
                               $"select idBolao from usuariobolao left join Usuario u on(u.id = usuariobolao.idUsuario) where u.Login = '{nomeUsuario}'" +
                           ")" +
                           filtroWhere +
                           $" order by b.{ordem}" +
                           paginacao;
                    }
                }
                else
                    sql =
                        $"select b.id as id, b.Nome as Nome, c.Nome as NomeCampeonato from Bolao b left join Campeonato c on b.idCampeonato = c.id" +
                        " where b.id in (" +
                            $"select idBolao from usuariobolao left join Usuario u on(u.id = usuariobolao.idUsuario) where u.Login = '{nomeUsuario}'" +
                        ")" +
                        filtroWhere +
                        " order by b.nome" +
                        paginacao;

                ret = db.Database.Connection.Query<BolaoModel>(sql).ToList();
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