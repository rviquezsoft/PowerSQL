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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Insert<T> : DBMotorBase<T>, IDBActionNonQuery
    {
        private readonly IPrintExceptions exceptions;
        private readonly IGetPropertiesService propertiesService;
        private readonly ISQLParametersService sqlParameterService;

        public Insert(string _connection,
            IPrintExceptions _exceptions,
            IGetPropertiesService _propertiesService,
            ISQLParametersService _sqlParameterService) : base(_connection)
        {
            exceptions = _exceptions;
            propertiesService = _propertiesService;
            sqlParameterService = _sqlParameterService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        private string build(PropertyInfo[] props)
        {
            string Columns = "";
            string Parameters = "";
            try
            {
                for (int i = 0; i < props.Length; i++)
                {
                    Columns += (i == props.Length - 1) ? $"{props[i].Name}" : $"{props[i].Name},";
                    Parameters += (i == props.Length - 1) ? $"@{props[i].Name}" : $"@{props[i].Name},";
                }
            }
            catch (Exception e)
            {
                exceptions.printException(e);
            }
            return $"insert into " +
                    $"{typeof(T).Name}({Columns})values(" +
                        $"{Parameters});";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public async Task<int> buildAndExecute(params (string name, object val)[] pars)
        {
            return await execute(build(propertiesService.getProps()), pars);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="query"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
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
