using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors
{
    public class SQLParametersService : ISQLParametersService
    {
        private readonly IPrintExceptions exceptions;

        public SQLParametersService(IPrintExceptions _exceptions)
        {
            exceptions = _exceptions;
        }
        /// <summary>
        /// Add parameters values to a SQLCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="stringPars"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public Task<SqlCommand> addParameters(SqlCommand command,
            params (string name, object val)[] pars)
        {
            try
            {
                if (command == null || pars == null || pars.Length < 1)
                {
                    return Task.FromResult(command);
                }
                for (int i = 0; i < pars.Length; i++)
                {
                    if (pars == null)
                    {
                        command.Parameters.AddWithValue(pars[i].name, DBNull.Value);
                        continue;
                    }
                    command.Parameters.AddWithValue(pars[i].name, pars[i].val);
                }
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return Task.FromResult(command);
        }

        public Task<SqlCommand> addParametersSP(SqlCommand command,
           params (string name, object val,ParameterDirection inputOrOutput)[] pars)
        {
            try
            {
                if (command == null || pars == null || pars.Length < 1)
                {
                    return Task.FromResult(command);
                }
                for (int i = 0; i < pars.Length; i++)
                {
                    if (pars == null)
                    {
                        command.Parameters.AddWithValue(pars[i].name, DBNull.Value).Direction = pars[i].inputOrOutput;
                        continue;
                    }
                    command.Parameters.AddWithValue(pars[i].name, pars[i].val).Direction = pars[i].inputOrOutput;
                }
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }
            return Task.FromResult(command);
        }


    }
}
