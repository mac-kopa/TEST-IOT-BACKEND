using System.IO;
using System.Threading.Tasks;

namespace WeatherMeasurement.Repositories.Interfaces
{
    public interface IBlobRepository
    {
        Task<Stream> GetFileStreamAsync(string path, string fileName);
    }
}
