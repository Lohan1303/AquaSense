using AquaSense.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    public class ConjuntoHabitacionalDAO : PadraoDAO<ConjuntoHabitacionalViewModel>
    {
        protected override SqlParameter[] CriaParametros(ConjuntoHabitacionalViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("id", model.Id),
                new SqlParameter("id_usuario_adm", model.IdUsuarioAdm),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("cnpj", model.Cnpj),
                new SqlParameter("endereco", model.Endereco)
            };

            return parameters;
        }

        protected override ConjuntoHabitacionalViewModel MontaModel(DataRow registro)
        {
            ConjuntoHabitacionalViewModel ConjuntoHabitacional = new ConjuntoHabitacionalViewModel();
            ConjuntoHabitacional.Id = Convert.ToInt32(registro["id_Conjunto_Habitacional"]);
            ConjuntoHabitacional.IdUsuarioAdm = Convert.ToInt32(registro["id_usuario_adm"]);
            ConjuntoHabitacional.Endereco = registro["endereco"].ToString();
            ConjuntoHabitacional.Nome = registro["nome"].ToString();
            ConjuntoHabitacional.Cnpj = registro["cpj"].ToString();
            return ConjuntoHabitacional;
        }

        public ConjuntoHabitacionalViewModel ConsultaConjuntoHabitacionalPorUsuario(int id_usuario)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id_usuario_adm", id_usuario),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_ConjuntoHabitacionalPorUsuario", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        protected override void SetTabela()
        {
            Tabela = "ConjuntoHabitacional";
            ChaveIdentity = true;
        }
    }
}
