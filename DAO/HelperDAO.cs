using System;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    static class HelperDAO
    {
        public static void ExecutaSQL(string sql, SqlParameter[] p)
        {
            SqlConnection conexao = ConexaoBD.GetConexao();
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexao);
                if (p != null)
                    comando.Parameters.AddRange(p);
                comando.ExecuteNonQuery();
            }
            finally
            {
                conexao.Close();
            }
        }

        public static DataTable ExecutaSelect(string sql, SqlParameter[] p)
        {
            SqlConnection cx = ConexaoBD.GetConexao();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cx);
                if (p != null)
                    adapter.SelectCommand.Parameters.AddRange(p);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                return tabela;
            }
            finally
            {
                cx.Close();
            }
        }

        public static object NullAsDbNull(object valor)
        {
            if (valor == null)
                return DBNull.Value;
            else
                return valor;
        }

        public static int ExecutaProc(string nomeProc,
                                      SqlParameter[] parametros,
                                      bool consultaUltimoIdentity = false)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);
                    comando.ExecuteNonQuery();
                    if (consultaUltimoIdentity)
                    {
                        string sql = "select isnull(@@IDENTITY,0)";
                        comando.CommandType = CommandType.Text;
                        comando.CommandText = sql;
                        int pedidoId = Convert.ToInt32(comando.ExecuteScalar());
                        conexao.Close();
                        return pedidoId;
                    }
                    else
                        return 0;
                }
            }
        }

        public static DataTable ExecutaProcSelectX(string nomeProc, SqlParameter parametro)
        {
            SqlParameter[] p =
            {
                parametro
            };
            return ExecutaProcSelect(nomeProc, p);
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }
        }
    }
}
