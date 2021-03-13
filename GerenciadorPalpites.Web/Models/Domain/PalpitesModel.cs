using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class PalpitesModel
    {
        #region Atributos
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdBolao { get; set; }
        public int IdTimeCasa { get; set; }
        [NotMapped]
        public string NomeTimeCasa { get; set; }
        public int IdTimeFora { get; set; }
        [NotMapped]
        public string NomeTimeFora { get; set; }
        public int IdPartida { get; set; }
        public int PalpiteTimeCasa { get; set; }
        public int PalpiteTimeFora { get; set; }
        [NotMapped]
        public string NomeRodada { get; set; }
        [NotMapped]
        public DateTime Data { get; set; }
        [NotMapped]
        public List<PartidaModel> UltimasPartidas { get; set; }
        public bool Contabilizado { get; set; }
        public virtual TimeModel TimeCasa { get; set; }
        public virtual TimeModel TimeFora { get; set; }
        public virtual PartidaModel Partida { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
        public virtual BolaoModel Bolao { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Palpites.Count();
            }

            return ret;
        }

        public static List<PalpitesModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<PalpitesModel>();

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

                var sql = $"select * from Palpites{filtroWhere} order by {(!string.IsNullOrEmpty(ordem) ? ordem : "id")}{paginacao}";

                ret = db.Database.Connection.Query<PalpitesModel>(sql).ToList();
            }

            return ret;
        }

        public static PalpitesModel RecuperarPalpiteUsuarioPeloIdPartida(string usuario, int idPartida, long idBolao)
        {
            PalpitesModel ret = null;

            using (var db = new ContextoBD())
            {
                var sql =
                    $"select top 1 Palpites.id as Id, idUsuario as IdUsuario, idBolao as IdBolao, palpiteTimeCasa as PalpiteTimeCasa, palpiteTimeFora as PalpiteTimeFora, idTimeCasa as IdTimeCasa, idTimeFora as IdTimeFora, idPartida as IdPartida" +
                    " from Palpites" +
                    $" left join Usuario u on(u.id = Palpites.idUsuario) where u.Nome = '{usuario.ToLower()}'" +
                    $" and Palpites.idPartida = {idPartida}" +
                    $" and Palpites.idBolao = {idBolao}";

                ret = db.Database.Connection.Query<PalpitesModel>(sql).FirstOrDefault();
            }

            return ret;
        }

        public static PalpitesModel RecuperarPeloId(int id)
        {
            PalpitesModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Palpites.Find(id);
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
                    var local = new PalpitesModel { Id = id };
                    db.Palpites.Attach(local);
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
                    db.Palpites.Add(this);
                }
                else
                {
                    db.Palpites.Attach(this);
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