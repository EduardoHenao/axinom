using FileLoader.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FileLoader.Services
{
    public class EncryptionServices : IEncryptionServices
    {
        private readonly byte[] _encriptionKey;
        private const string _defaultEncriptionKey = "001C2233A455667C";

        public EncryptionServices(IConfiguration configuration)
        {
            var confKey = configuration["EncryptionKey"];
            string encriptionKeyAsString = string.IsNullOrEmpty(confKey) ? _defaultEncriptionKey : confKey; // if  not in conf, defautl 'default key'
            _encriptionKey = System.Text.Encoding.UTF8.GetBytes(encriptionKeyAsString);
        }

        public string EncryptToString(string plainText)
        {
            byte[] encrypted;
            byte[] IV; // initialization vector
            byte[] combinedIvCt;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode.ECB;

                aes.GenerateIV();
                IV = aes.IV;// store initialization vector

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }

                combinedIvCt = new byte[IV.Length + encrypted.Length];
                Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
                Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
            }

            return System.Text.Encoding.UTF8.GetString(combinedIvCt);
        }
        public string EncryptToString(byte[] bytes)
        {
            byte[] encrypted;
            byte[] IV; // initialization vector
            byte[] combinedIvCt;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode.ECB;

                aes.GenerateIV();
                IV = aes.IV;// store initialization vector

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        encrypted = memoryStream.ToArray();
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
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode.ECB;

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
}
