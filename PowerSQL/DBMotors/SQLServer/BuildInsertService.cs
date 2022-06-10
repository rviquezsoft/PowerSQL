using PowerSQL.Exceptions;
using PowerSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    public class BuildInsertService<T> : IBuildInsertService
    {
        private readonly IPrintExceptions exceptions;

        public BuildInsertService(IPrintExceptions _exceptions)
        {
            exceptions = _exceptions;
        }
        /// <summary>
        /// Returns an insert statement with columns and parameters
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public string buildInsert(PropertyInfo[] props)
        {
            string Columns = "";
            string Parameters = "";
            try
            {
                for (int i = 0; i < props.Length; i++)
                {
                    Columns += (i == props.Length - 1)?$"{props[i].Name}": $"{props[i].Name},";
                    Parameters += (i == props.Length - 1)?$"@{props[i].Name}": $"@{props[i].Name},";
                }
            }
            catch (Exception e)
            {
                exceptions.printException(e);
            }
            return $"insert into " +
                    $"{typeof(T).Name}({Columns})values(" +
                        $"{Parameters});";
        }
    }
}
