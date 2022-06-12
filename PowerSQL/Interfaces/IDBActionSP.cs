using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IDBActionSP
    {
        Task<int> execute(string query, params (string name, object val)[] pars);
        Task<List<SqlParameter>> executeGetOutputParameters(string query, params (string name, object val, ParameterDirection inputOrOutput)[] pars);
    }
}
