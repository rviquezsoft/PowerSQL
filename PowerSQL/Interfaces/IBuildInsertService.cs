using System.Reflection;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IBuildInsertService
    {
        string buildInsert(PropertyInfo[] props);
    }
}