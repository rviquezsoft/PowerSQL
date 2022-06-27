using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using PowerSQL.Exceptions;
using PowerSQL.Interfaces;

namespace PowerSQL.DBMotors.Postgres
{
	public class Select<T> : DBMotorBase<T>, IDBActionQuery<T>
    {
        private readonly IPrintExceptions exceptions;
        private readonly IGetPropertiesService propertiesService;
        private readonly INpgsqlParametersService sqlParameterService;
        private readonly ISerializeService serializeService;
        private readonly IDeserealizeService<T> deserealizeService;

        public Select(string _connection,
            IPrintExceptions _exceptions, IGetPropertiesService _propertiesService,
            INpgsqlParametersService _sqlParameterService, ISerializeService _serializeService,
            IDeserealizeService<T> _deserealizeService) : base(_connection)
        {
            exceptions = _exceptions;
            propertiesService = _propertiesService;
            sqlParameterService = _sqlParameterService;
            serializeService = _serializeService;
            deserealizeService = _deserealizeService;
        }

        private string build(string schema = "dbo")
        {
            string query = "";
            try
            {

                var props = propertiesService.getProps();
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
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return query;
        }
        public async Task<List<T>> buildAndExecute()
        {
            return await execute(build(), null);
        }

        public async Task<List<T>> execute(string query, params (string name, object val)[] pars)
        {
            List<T> list = null;

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
                using (var sql = new NpgsqlConnection(connection))
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
                    list = await processCommand(sql, query, pars);
                }
                return list;
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }

            return list;
        }

        private async Task<List<T>> processCommand(NpgsqlConnection sql,
            string query, params (string name, object val)[] pars)
        {
            try
            {
                using (var cmd = new NpgsqlCommand(query, sql))
                {
                    //add parameters
                    if (pars != null && pars.Length > 0)
                    {
                        await sqlParameterService.addParameters(cmd, pars);
                    }
                    return await processReader(cmd);
                }
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return null;
        }
        private async Task<List<T>> processReader(NpgsqlCommand cmd)
        {
            List<T> list = null;
            IDictionary<string, object> props = new ExpandoObject();
            try
            {
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
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }

            return list;
        }
    }
}

