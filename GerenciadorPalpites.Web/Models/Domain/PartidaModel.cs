using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class PartidaModel
    {
        #region Atributos

        public int Id { get; set; }
        public int IdTimeCasa { get; set; }
        public int IdTimeFora { get; set; }
        public int IdCampeonato { get; set; }
        public DateTime Data { get; set; }
        public virtual CampeonatoModel Campeonato { get; set; }
        public virtual TimeModel TimeCasa { get; set; }
        public virtual TimeModel TimeFora { get; set; }
        public int PlacarTimeCasa { get; set; }
        public int PlacarTimeFora { get; set; }

        #endregion

        #region Métodos

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Partidas.Count();
            }

            return ret;
        }

        public static List<PartidaModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", bool somenteAtivos = false)
        {
            var ret = new List<PartidaModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                    filtroWhere = string.Format("where (lower(a.nome) like '%{0}%' or (lower(b.nome) like '%{0}%') ", filtro.ToLower());
                
                var pos = (pagina - 1) * tamPagina;
                var paginacao = "";
                if (pagina > 0 && tamPagina > 0)
                {
                    paginacao = string.Format(" offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);
                }

                var sql =
                    "select partida.*,a.* from partida left join Campeonato a on(a.id = Partida.idCampeonato) " + filtroWhere + "order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "a.nome") +
                    paginacao;

                ret = db.Database.Connection.Query<PartidaModel>(sql).ToList();
            }

            return ret;
        }

        public static PartidaModel RecuperarPeloId(int id)
        {
            PartidaModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Partidas.Find(id);
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
                    var partida = new PartidaModel { Id = id };
                    db.Partidas.Attach(partida);
                    db.Entry(partida).State = EntityState.Deleted;
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
                    db.Partidas.Add(this);
                }
                else
                {
                    db.Partidas.Attach(this);
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