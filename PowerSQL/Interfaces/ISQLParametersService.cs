using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface ISQLParametersService
    {
        Task<SqlCommand> addParameters(SqlCommand command, params (string name, object val)[] pars);
        Task<SqlCommand> addParametersSP(SqlCommand command, params (string name, object val, ParameterDirection inputOrOutput)[] pars);
    }
}