using FastMember;
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
    public class BulkInsert<T>:DBMotorBase<T>
    {
        public BulkInsert(string _connection) : base(_connection)
        {
        }
        /// <summary>
        /// Insert a lot of data into any table 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public async Task BulkData(IEnumerable<T> data, string tablename)
        {
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
        }
    }
}
