using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoreProcedure<T> : DBMotorBase<T>, IDBActionSP
    {
        private readonly IPrintExceptions exceptions;
        private readonly IGetPropertiesService propertiesService;
        private readonly ISQLParametersService sqlParameterService;
        public StoreProcedure(string _connection,
            IPrintExceptions _exceptions,
            IGetPropertiesService _propertiesService,
            ISQLParametersService _sqlParameterService) : base(_connection)
        {
            exceptions = _exceptions;  
            propertiesService = _propertiesService;    
            sqlParameterService = _sqlParameterService;    
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

        public async Task<List<SqlParameter>> executeGetOutputParameters(string query, params (string name, object val, ParameterDirection inputOrOutput)[] pars)
        {
            List<SqlParameter> result = null;
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
                    result = await processCommandSP(sql, query, pars);
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

        private async Task<List<SqlParameter>> processCommandSP(SqlConnection sql,
          string query, params (string name, object val, ParameterDirection inputOrOutput)[] pars)
        {
            List<SqlParameter> result = null;
            try
            {
                using (var cmd = new SqlCommand(query, sql))
                {
                    //add parameters
                    if (pars != null && pars.Length > 0)
                    {
                        await sqlParameterService.addParametersSP(cmd, pars);
                    }
                    //return await cmd.ExecuteNonQueryAsync();
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
