using System.Collections.Generic;
using DataManagementSystem.Business;

namespace AxinomCommon.IServices
{
    public interface IEncryptionServices
    {
        /**
         * encrypts with AES 128 ECB to a base64 string
         */
        string EncryptToString(string plainText);
        /**
         * encrypts with AES 128 ECB to bytes
         */
        string EncryptToString(byte[] bytes);
        /**
         * decrypts with AES 128 ECB from a base64 string
         */
        string DecryptToString(string encryptedText);
        /**
         * decrypts with AES 128 ECB to bytes
         */
        byte[] DecryptToBytes(string encryptedText);
    }
}
