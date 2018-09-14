using System.Threading.Tasks;

namespace FileLoader.IServices
{
    public interface IDataManagementSystemCallerServices
    {
        Task<bool> PostAsync(string remoteUrl, string jsonString, string encryptedUser, string encryptedPassword);
    }
}
