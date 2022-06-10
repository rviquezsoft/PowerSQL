using Newtonsoft.Json;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    public class SqlServerSelectService<T>
        : DBMotorBase<T>, ISqlServerSelectService<T>
    {
        private readonly ISerializeService serializeService;
        private readonly ISQLParametersService sqlParameterService;
        private readonly IDeserealizeService<T> deserealizeService;
        private readonly IGetPropertiesService getPropertiesService;
        private readonly IPrintExceptions exceptions;
        public SqlServerSelectService(string _connection,
            ISerializeService _serializeService,
            IDeserealizeService<T> _deserealizeService,
            ISQLParametersService _sqlParameterService,
            IGetPropertiesService _getPropertiesService,
            IPrintExceptions _exceptions) :
            base(_connection)
        {
            serializeService = _serializeService;
            deserealizeService = _deserealizeService;
            sqlParameterService = _sqlParameterService;
            getPropertiesService = _getPropertiesService;
            exceptions = _exceptions;
        }

        /// <summary>
        /// Run a custom query into database
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<T>> select(string query, params (string name, object val)[] pars)
        {
            return await buildQuery(query, pars);
        }
        /// <summary>
        /// Run a select * from T name, 
        /// you can override name of  schema as needed
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> selectAll(string schema = "dbo")
        {
            string query = "";
            var props = getPropertiesService.getProps();
            if (props == null || props.Length < 1)
            {
                return null;
            }
            string columns = "";
            foreach (var item in props.ToList())
            {
                columns += $"{item.Name},";
            }
            int last = columns.LastIndexOf(',');
            string withoutLast = columns.Substring(0, last);
            query = $"SELECT {withoutLast} FROM {schema}.{props.FirstOrDefault().DeclaringType.Name};";

            return await buildQuery(query);
        }

        private async Task<List<T>> buildQuery(string query, params (string name, object val)[] pars)
        {
            List<T> list = null;
            IDictionary<string, object> props = new ExpandoObject();
            try
            {
                //nothing to do
                if (serializeService == null ||
                    deserealizeService == null ||
                    string.IsNullOrWhiteSpace(connection) ||
                    string.IsNullOrWhiteSpace(query))
                {
                    return list;
                }
                using (var sql = new SqlConnection(connection))
                {
                    if (sql == null)
                    {
                        return list;
                    }
                    await sql.OpenAsync();
                    if (sql.State != ConnectionState.Open)
                    {
                        return list;
                    }
                    using (var cmd = new SqlCommand(query, sql))
                    {
                        //add parameters
                        if (pars != null && pars.Length > 0)
                        {
                            await sqlParameterService.addParameters(cmd, pars);
                        }
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader == null)
                            {
                                return list;
                            }
                            //process records
                            list = new List<T>();
                            foreach (var item in reader.Cast<DbDataRecord>())
                            {
                                props = new ExpandoObject();
                                for (int i = 0; i < item.FieldCount; i++)
                                {
                                    if (item.GetValue(i) != null)
                                    {
                                        props[$"{item.GetName(i) ?? ""}"] = item.GetValue(i);
                                    }
                                }

                                string json = await serializeService.serialize(props);

                                T converted = await deserealizeService.deserealize(json);

                                list.Add(converted);
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }

            return list;
        }

       
    }
}
