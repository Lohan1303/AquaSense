using AquaSense.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            object imgByte = model.ImagemEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] p = new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("login_usuario", model.LoginUsuario),
                new SqlParameter("nome_pessoa", model.NomePessoa),
                new SqlParameter("senha", model.Senha),
                new SqlParameter("imagem", imgByte),
                new SqlParameter("adm", model.Adm),
            };

            return p;
        }

        public override int Insert(UsuarioViewModel model)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    ConjuntoHabitacionalViewModel conj = new ConjuntoHabitacionalViewModel()
                    {
                        Nome = model.NomeConjuntoHabitacional
                    };
                    ConjuntoHabitacionalDAO conjuntoDao = new ConjuntoHabitacionalDAO();
                    conj.IdUsuarioAdm = HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model), ChaveIdentity);
                    conjuntoDao.Insert(conj);
                    transacao.Complete();
                }
                return model.Id;
            }
            catch 
            {
                return 0;
            }

        }

        public override void Update(UsuarioViewModel model)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    ConjuntoHabitacionalViewModel conj = new ConjuntoHabitacionalViewModel()
                    {
                        Nome = model.NomeConjuntoHabitacional
                    };
                    conj.IdUsuarioAdm = HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model));
                    ConjuntoHabitacionalDAO conjuntoDAO = new ConjuntoHabitacionalDAO();
                    conjuntoDAO.Update(conj);

                    transacao.Complete();
                }
            }
            catch (Exception erro)
            {
                
            }
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel u = new UsuarioViewModel()
            {
                Id = Convert.ToInt32(registro["id_usuario"]),
                LoginUsuario = registro["login_usuario"].ToString(),
                NomePessoa = registro["nome_pessoa"].ToString(),
                Adm = Convert.ToBoolean(registro["adm"])
            };

            if (registro["senha"] != DBNull.Value)
                u.Senha = registro["senha"].ToString();

            if (registro["imagem"] != DBNull.Value)
                u.ImagemEmByte = registro["imagem"] as byte[];
            return u;
        }

        public UsuarioViewModel ConsultaAcesso(string login, string senha)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login_usuario", login),
                new SqlParameter("senha", senha),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_Acesso", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        protected override void SetTabela()
        {
            Tabela = "Usuario";
            ChaveIdentity = true;
        }
    }
}
