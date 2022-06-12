using FastMember;
using PowerSQL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BulkInsert<T> : DBMotorBase<T>, IBulkInsert<T> where T:new()
    {
        private readonly IPrintExceptions exceptions;
        public BulkInsert(string _connection,
            IPrintExceptions _exceptions) : base(_connection)
        {
            exceptions = _exceptions;
        }
        /// <summary>
        /// Insert a lot of data into any table 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public async Task<bool> BulkData(IEnumerable<T> data,string tablename="")
        {
            bool process = false;
            try
            {
                if (string.IsNullOrWhiteSpace(tablename))
                {
                    tablename =typeof(T).Name;
                }
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        using (var reader = ObjectReader.Create(data))
                        {
                            sqlBulkCopy.DestinationTableName = tablename;
                            await sqlBulkCopy.WriteToServerAsync(reader);
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exceptions.printException(ex);
            }

            return process;
        }
    }
}
