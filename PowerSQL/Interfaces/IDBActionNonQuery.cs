using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IDBActionNonQuery
    {
        Task<int> execute(string query, params (string name, object val)[] pars);
        Task<int> buildAndExecute(params (string name, object val)[] pars);
    }
}
