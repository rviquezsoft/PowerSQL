using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface ISqlServerSelectService<T>
    {
        Task<List<T>> select(string query, params (string name, object val)[] pars);
        Task<List<T>> selectAll(string schema = "dbo");
    }
}