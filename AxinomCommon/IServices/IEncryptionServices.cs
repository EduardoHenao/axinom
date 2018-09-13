using System.Collections.Generic;
using DataManagementSystem.Business;

namespace AxinomCommon.IServices
{
    public interface IEncryptionServices
    {
        string EncryptToString(string plainText);
        string EncryptToString(byte[] bytes);
        string DecryptToString(string encryptedText);
        byte[] DecryptToBytes(string encryptedText);
    }
}
