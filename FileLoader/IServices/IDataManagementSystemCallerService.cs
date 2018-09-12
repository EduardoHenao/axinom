using System.Threading.Tasks;

namespace FileLoader.IServices
{
    public interface IDataManagementSystemCallerServices
    {
        Task<bool> PostAsync(string jsonString);
    }
}
