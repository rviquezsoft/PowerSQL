using System.Threading.Tasks;

namespace PowerSQL.Interfaces
{
    public interface IDeserealizeService<T>
    {
        Task<T> deserealize(string json);
    }
}