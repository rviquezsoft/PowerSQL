using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IDBActionQuery<T>
    {
        Task<List<T>> execute(string query, params (string name, object val)[] pars);
        Task<List<T>> buildAndExecute();
    }
}
