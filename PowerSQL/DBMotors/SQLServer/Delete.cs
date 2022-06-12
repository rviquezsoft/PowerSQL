using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    public class Delete<T> : DBMotorBase<T>, IDBActionNonQuery
    {
        private readonly IPrintExceptions exceptions;
        private readonly IGetPropertiesService propertiesService;
        private readonly ISQLParametersService sqlParameterService;

        public Delete(string _connection,
             IPrintExceptions _exceptions,
            IGetPropertiesService _propertiesService,
            ISQLParametersService _sqlParameterService) : base(_connection)
        {
            exceptions = _exceptions;
            propertiesService = _propertiesService;
            sqlParameterService = _sqlParameterService;
        }

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
        public Task<int> buildAndExecute(params (string name, object val)[] pars)
        {
            throw new NotImplementedException();
        }

        public Task<int> execute(string query, params (string name, object val)[] pars)
        {
            throw new NotImplementedException();
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
