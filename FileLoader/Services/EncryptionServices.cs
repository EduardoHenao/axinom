using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FileLoader.Services
{
    public class EncryptionServices : IEncryptionServices
    {
        private readonly byte[] encriptionKey;

        public EncryptionServices(IConfiguration configuration)
        {
            string encriptionKeyAsString = string.IsNullOrEmpty(configuration["EncryptionKey"]) ? "default key" : configuration["EncryptionKey"];
            encriptionKey = System.Text.Encoding.UTF8.GetBytes(encriptionKeyAsString);
        }

        public string EncryptToString(string plainText)
        {
            byte[] encrypted;
            byte[] IV;
            byte[] combinedIvCt;

            using (Aes aes = Aes.Create())
            {
                aes.Key = encriptionKey;
                aes.Mode = CipherMode.CBC;

                aes.GenerateIV();
                IV = aes.IV;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                combinedIvCt = new byte[IV.Length + encrypted.Length];
                Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
                Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
            }

            return System.Text.Encoding.UTF8.GetString(combinedIvCt);
        }

        public string DecryptToString(string encryptedText)
        {
            string plaintext = null;
            byte[] encryptedTextAsArray = System.Text.Encoding.UTF8.GetBytes(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = encriptionKey;
                aes.Mode = CipherMode.CBC;

                byte[] IV = new byte[aes.BlockSize / 8];
                byte[] cipherText = new byte[encryptedTextAsArray.Length - IV.Length];

                Array.Copy(encryptedTextAsArray, IV, IV.Length);
                Array.Copy(encryptedTextAsArray, IV.Length, cipherText, 0, cipherText.Length);

                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }

    public interface IEncryptionServices
    {
        string EncryptToString(string plainText);
        string DecryptToString(string encryptedText);
    }
}
