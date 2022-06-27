using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace PowerSQL.DBMotors
{
    public interface INpgsqlParametersService
    {
        Task<NpgsqlCommand> addParameters(NpgsqlCommand command, params (string name, object val)[] pars);
        Task<NpgsqlCommand> addParametersSP(NpgsqlCommand command, params (string name, object val, ParameterDirection inputOrOutput)[] pars);
    }
}