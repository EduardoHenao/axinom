using System.Threading.Tasks;

namespace ControlPanel.IServices
{
    public interface IDataManagementSystemCallerServices
    {
        Task<bool> PostAsync(string remoteUrl, string jsonString, string encryptedUser, string encryptedPassword);
    }
}
