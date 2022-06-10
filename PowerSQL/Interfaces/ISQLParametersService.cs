using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface ISQLParametersService
    {
        Task<SqlCommand> addParameters(SqlCommand command, params (string name, object val)[] pars);
    }
}