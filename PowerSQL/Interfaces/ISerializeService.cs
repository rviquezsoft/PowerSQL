using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface ISerializeService
    {
        Task<string> serialize(object obj);
    }
}