using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerSQL.DBMotors.SQLServer
{
    public interface IBulkInsert<T>
    {
        Task<bool> BulkData(IEnumerable<T> data, string tablename="");
    }
}