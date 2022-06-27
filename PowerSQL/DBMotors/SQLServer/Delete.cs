using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    public class Delete<T> : DBMotorBase<T>, IDBActionNonQuery
    {
        private readonly IPrintExceptions exceptions;
        private readonly ISQLParametersService sqlParameterService;

        public Delete(string _connection,
             IPrintExceptions _exceptions,
            ISQLParametersService _sqlParameterService) : base(_connection)
        {
            exceptions = _exceptions;
            sqlParameterService = _sqlParameterService;
        }

        private string build(PropertyInfo[] props)
        {
            return $"delete from " +
                    $"{typeof(T).Name};";
        }
        public async Task<int> buildAndExecute(params (string name, object val)[] pars)
        {
            return await execute(build(null), pars);
        }

        public async Task<int> execute(string query, params (string name, object val)[] pars)
        {
            int result = 0;
            try
            {

                //nothing to do
                if (string.IsNullOrWhiteSpace(connection) ||
                    string.IsNullOrWhiteSpace(query))
                {
                    return result;
                }
                using (var sql = new SqlConnection(connection))
                {
                    if (sql == null)
                    {
                        return result;
                    }
                    await sql.OpenAsync();
                    if (sql.State != ConnectionState.Open)
                    {
                        return result;
                    }
                    result = await processCommand(sql, query, pars);
                }
                return result;

            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return result;
        }

        private async Task<int> processCommand(SqlConnection sql,
      string query, params (string name, object val)[] pars)
        {
            int result = 0;
            try
            {
                using (var cmd = new SqlCommand(query, sql))
                {
                    //add parameters
                    if (pars != null && pars.Length > 0)
                    {
                        await sqlParameterService.addParameters(cmd, pars);
                    }
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return result;
        }
    }
}
