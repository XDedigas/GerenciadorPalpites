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
        public string NomeTimeCasa { get; set; }
        public int IdTimeFora { get; set; }
        public string NomeTimeFora { get; set; }
        public string NomeCampeonato { get; set; }
        public int IdCampeonato { get; set; }
        public DateTime Data { get; set; }
        public string DataFormatada { get; set; }
        public virtual CampeonatoModel Campeonato { get; set; }
        public virtual TimeModel TimeCasa { get; set; }
        public virtual TimeModel TimeFora { get; set; }
        public int PlacarTimeCasa { get; set; }
        public int PlacarTimeFora { get; set; }
        public string NomeRodada { get; set; }
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

        public static List<PartidaModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "", bool somenteAtivos = false, long idTime = -1, long idCampeonato = -1)
        {
            var ret = new List<PartidaModel>();

            using (var db = new ContextoBD())
            {
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = $" where (lower(timeCasa.Nome) like '%{filtro.ToLower()}%' or lower(timeFora.Nome) like '%{filtro.ToLower()}%' or lower(c.Nome) like '%{filtro.ToLower()}%')";
                    if (idTime > 0 && idCampeonato > 0)
                    {
                        filtroWhere += $" and (timeCasa.id = {idTime} or timeFora.id = {idTime}) and c.id = {idCampeonato}";
                    }
                    else if (idTime > 0)
                    {
                        filtroWhere += $" and (timeCasa.id = {idTime} or timeFora.id = {idTime})";
                    }
                    else if (idCampeonato > 0)
                    {
                        filtroWhere += $" and c.id = {idCampeonato}";
                    }
                }
                else
                {
                    if (idTime > 0 && idCampeonato > 0)
                    {
                        filtroWhere += $" where (timeCasa.id = {idTime} or timeFora.id = {idTime}) and c.id = {idCampeonato}";
                    }
                    else if (idTime > 0)
                    {
                        filtroWhere += $" where (timeCasa.id = {idTime} or timeFora.id = {idTime})";
                    }
                    else if (idCampeonato > 0)
                    {
                        filtroWhere += $" where c.id = {idCampeonato}";
                    }
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
                    if (ordem.StartsWith("timeCasa"))
                    {
                        sql =
                        "select top 1000 p.id as Id, p.data as Data, p.idTimeCasa as IdTimeCasa, timeCasa.nome as NomeTimeCasa, p.placarTimeCasa as PlacarTimeCasa, p.idTimeFora as IdTimeFora, timeFora.nome as NomeTimeFora, p.placarTimeFora as PlacarTimeFora, p.idCampeonato as IdCampeonato, c.Nome as NomeCampeonato" +
                        " from Partida p" +
                        " left join Time timeCasa on (p.idTimeCasa = timeCasa.id)" +
                        " left join Time timeFora on (p.idTimeFora = timeFora.id)" +
                        " left join Campeonato c on (p.idCampeonato = c.id)" +
                        filtroWhere +
                        $" order by timeCasa.nome{ordem.ToLower().Replace("timecasa", "")}" +
                        paginacao;
                    }
                    else if (ordem.StartsWith("timeFora"))
                    {
                        sql =
                        "select top 1000 p.id as Id, p.data as Data, p.idTimeCasa as IdTimeCasa, timeCasa.nome as NomeTimeCasa, p.placarTimeCasa as PlacarTimeCasa, p.idTimeFora as IdTimeFora, timeFora.nome as NomeTimeFora, p.placarTimeFora as PlacarTimeFora, p.idCampeonato as IdCampeonato, c.Nome as NomeCampeonato" +
                        " from Partida p" +
                        " left join Time timeCasa on (p.idTimeCasa = timeCasa.id)" +
                        " left join Time timeFora on (p.idTimeFora = timeFora.id)" +
                        " left join Campeonato c on (p.idCampeonato = c.id)" +
                        filtroWhere +
                        $" order by timeFora.nome{ordem.ToLower().Replace("timefora", "")}" +
                        paginacao;
                    }
                    else if (ordem.StartsWith("campeonato"))
                    {
                        sql =
                        "select top 1000 p.id as Id, p.data as Data, p.idTimeCasa as IdTimeCasa, timeCasa.nome as NomeTimeCasa, p.placarTimeCasa as PlacarTimeCasa, p.idTimeFora as IdTimeFora, timeFora.nome as NomeTimeFora, p.placarTimeFora as PlacarTimeFora, p.idCampeonato as IdCampeonato, c.Nome as NomeCampeonato" +
                        " from Partida p" +
                        " left join Time timeCasa on (p.idTimeCasa = timeCasa.id)" +
                        " left join Time timeFora on (p.idTimeFora = timeFora.id)" +
                        " left join Campeonato c on (p.idCampeonato = c.id)" +
                        filtroWhere +
                        $" order by c.nome{ordem.ToLower().Replace("campeonato", "")}" +
                        paginacao;
                    }
                    else
                    {
                        sql =
                        "select top 1000 p.id as Id, p.data as Data, p.idTimeCasa as IdTimeCasa, timeCasa.nome as NomeTimeCasa, p.placarTimeCasa as PlacarTimeCasa, p.idTimeFora as IdTimeFora, timeFora.nome as NomeTimeFora, p.placarTimeFora as PlacarTimeFora, p.idCampeonato as IdCampeonato, c.Nome as NomeCampeonato" +
                        " from Partida p" +
                        " left join Time timeCasa on (p.idTimeCasa = timeCasa.id)" +
                        " left join Time timeFora on (p.idTimeFora = timeFora.id)" +
                        " left join Campeonato c on (p.idCampeonato = c.id)" +
                        filtroWhere +
                        $" order by p.{ordem}" +
                        paginacao;
                    }
                }
                else
                    sql =
                        "select top 1000 p.id as Id, p.data as Data, p.idTimeCasa as IdTimeCasa, timeCasa.nome as NomeTimeCasa, p.placarTimeCasa as PlacarTimeCasa, p.idTimeFora as IdTimeFora, timeFora.nome as NomeTimeFora, p.placarTimeFora as PlacarTimeFora, p.idCampeonato as IdCampeonato, c.Nome as NomeCampeonato" +
                        " from Partida p" +
                        " left join Time timeCasa on (p.idTimeCasa = timeCasa.id)" +
                        " left join Time timeFora on (p.idTimeFora = timeFora.id)" +
                        " left join Campeonato c on (p.idCampeonato = c.id)" +
                        filtroWhere +
                        " order by p.data" +
                        paginacao;

                ret = db.Database.Connection.Query<PartidaModel>(sql).ToList();
            }

            return ret;
        }

        public static List<PartidaModel> RecuperarPartidasParaPalpite(long idCampeonato)
        {
            var ret = new List<PartidaModel>();

            using (var db = new ContextoBD())
            {
                var sql = "";
                if (idCampeonato == 30 || idCampeonato == 31)
                {
                    sql = "select Partida.id as Id, Partida.data as Data, timeCasa.id as IdTimeCasa, timeCasa.Nome as NomeTimeCasa, timeFora.id as IdTimeFora, timeFora.Nome as NomeTimeFora, Rodada.descricao as NomeRodada from Partida" +
                    " left join rodadapartida on Partida.id = rodadapartida.idPartida" +
                    " left join Rodada on rodadapartida.idRodada = Rodada.id" +
                    " left join Time as timeCasa on Partida.idTimeCasa = timeCasa.id" +
                    " left join Time as timeFora on Partida.idTimeFora = timeFora.id" +
                    " where placarTimeCasa = -1" +
                    " and placarTimeFora = -1" +
                    $" and data between '{DateTime.Now.AddMinutes(10).ToString("yyyyMMdd HH:mm:ss")}' and '{DateTime.Now.AddDays(1).ToString("yyyyMMdd HH:mm:ss")}'" +
                    //Código para testes
                    //$" and data between '{new DateTime(2021, 3, 1).ToString("yyyyMMdd HH:mm:ss")}' and '{new DateTime(2021, 3, 1).AddDays(7).ToString("yyyyMMdd HH:mm:ss")}'" +
                    $" and Partida.idCampeonato = {idCampeonato}" +
                    " order by data asc";
                }
                else 
                {
                    sql = "select Partida.id as Id, Partida.data as Data, timeCasa.id as IdTimeCasa, timeCasa.Nome as NomeTimeCasa, timeFora.id as IdTimeFora, timeFora.Nome as NomeTimeFora, Rodada.descricao as NomeRodada from Partida" +
                    " left join rodadapartida on Partida.id = rodadapartida.idPartida" +
                    " left join Rodada on rodadapartida.idRodada = Rodada.id" +
                    " left join Time as timeCasa on Partida.idTimeCasa = timeCasa.id" +
                    " left join Time as timeFora on Partida.idTimeFora = timeFora.id" +
                    " where placarTimeCasa = -1" +
                    " and placarTimeFora = -1" +
                    $" and data between '{DateTime.Now.AddMinutes(10).ToString("yyyyMMdd HH:mm:ss")}' and '{DateTime.Now.AddDays(7).ToString("yyyyMMdd HH:mm:ss")}'" +
                    //Código para testes
                    //$" and data between '{new DateTime(2021, 3, 1).ToString("yyyyMMdd HH:mm:ss")}' and '{new DateTime(2021, 3, 1).AddDays(7).ToString("yyyyMMdd HH:mm:ss")}'" +
                    $" and Partida.idCampeonato = {idCampeonato}" +
                    " order by data asc";
                }                    

                ret = db.Database.Connection.Query<PartidaModel>(sql).ToList();
            }

            return ret;
        }

        public static List<PartidaModel> RecuperarUltimasPartidasEntreTimes(int idPrimeiroTime, int idSegundoTime)
        {
            var ret = new List<PartidaModel>();

            using (var db = new ContextoBD())
            {
                var sql =
                    "select top 5 Partida.data as Data, timeCasa.Nome as NomeTimeCasa, placarTimeCasa as PlacarTimeCasa, placarTimeFora as PlacarTimeFora, timeFora.Nome as NomeTimeFora, Campeonato.Nome as NomeCampeonato" +
                    " from Partida" +
                    " left join Campeonato on Partida.idCampeonato = Campeonato.id" +
                    " left join Time as timeCasa on Partida.idTimeCasa = timeCasa.id" +
                    " left join Time as timeFora on Partida.idTimeFora = timeFora.id" +
                    $" where (idTimeCasa = {idPrimeiroTime} or idTimeFora = {idPrimeiroTime}) and (idTimeCasa = {idSegundoTime} or idTimeFora = {idSegundoTime})" +
                    " and placarTimeCasa != -1" +
                    " and placarTimeFora != -1" +
                    " order by data desc";

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