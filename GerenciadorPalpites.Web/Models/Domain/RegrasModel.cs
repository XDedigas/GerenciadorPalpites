using Dapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GerenciadorPalpites.Web.Models
{
    public class RegrasModel
    {
        #region Atributos

        public int Id { get; set; }
        public float Pontuacao1 { get; set; }
        public float Pontuacao2 { get; set; }
        public float Pontuacao3 { get; set; }

        #endregion

        #region Métodos

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var db = new ContextoBD())
            {
                ret = db.Regras.Count();
            }

            return ret;
        }

        public static RegrasModel RecuperarPeloId(int id)
        {
            RegrasModel ret = null;

            using (var db = new ContextoBD())
            {
                ret = db.Regras.Find(id);
            }

            return ret;
        }

        public int RecuperarIDPelosValores(RegrasViewModel model) 
        {
            using (var db = new ContextoBD())
            {
                var sql = $"select id from Regras where Pontuacao1 = {model.Pontuacao1} and Pontuacao2 = {model.Pontuacao2} and Pontuacao3 = {model.Pontuacao3}";
                var ret = db.Database.Connection.Query<RegrasModel>(sql).FirstOrDefault();
                if (ret == null)
                {
                    return 0;
                }
                return ret.Id;
            }
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var db = new ContextoBD())
            {
                if (model == null)
                {
                    db.Regras.Add(this);
                }
                else
                {
                    db.Regras.Attach(this);
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