using System.Reflection;
using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IGetPropertiesService
    {
        PropertyInfo[] getProps();
    }
}