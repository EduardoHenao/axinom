using AxinomCommon.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace AxinomCommon.Services
{
    /*
     * The sole purpose of this class is to convert strings encrypted in AES 128bit ECB
     * into either base64 encoded strings or bytes. (and vice-versa)
     */
    public class EncryptionServices : IEncryptionServices
    {
        private readonly byte[] _encriptionKey;
        private const string DefaultEncriptionKey = "001C2233A455667C";
        private const CipherMode CipherMode = System.Security.Cryptography.CipherMode.ECB;
        private const PaddingMode Padding = PaddingMode.PKCS7;

        public EncryptionServices(IConfiguration configuration)
        {
            var confKey = configuration["EncryptionKey"];
            string encriptionKeyAsString = string.IsNullOrEmpty(confKey) ? DefaultEncriptionKey : confKey; // if  not in conf use DefaultEncriptionKey
            _encriptionKey = System.Text.Encoding.UTF8.GetBytes(encriptionKeyAsString);
        }

        /**
         * encrypts with AES 128 ECB to a base64 string
         */
        public string EncryptToString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            byte[] encrypted;
            byte[] IV; // initialization vector
            byte[] combinedIvCt;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode;
                aes.BlockSize = 128;
                aes.Padding = Padding;

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

            string base64 = System.Convert.ToBase64String(combinedIvCt);
            return base64;
        }

        /**
         * encrypts with AES 128 ECB to bytes
         */
        public string EncryptToString(byte[] bytes)
        {
            if (bytes.Length == 0) return string.Empty;
            byte[] encrypted;
            byte[] IV; // initialization vector
            byte[] combinedIvCt;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode;
                aes.BlockSize = 128;
                aes.Padding = Padding;

                aes.GenerateIV();
                IV = aes.IV;// store initialization vector

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.FlushFinalBlock();
                        encrypted = memoryStream.ToArray();
                    }
                }

                combinedIvCt = new byte[IV.Length + encrypted.Length];
                Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
                Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
            }

            string base64 = System.Convert.ToBase64String(combinedIvCt);
            return base64;
        }

        /**
         * decrypts with AES 128 ECB from a base64 string
         */
        public string DecryptToString(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) return string.Empty;
            string plaintext = null;
            byte[] encryptedTextAsArray = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode;
                aes.BlockSize = 128;
                aes.Padding = Padding;

                byte[] IV = new byte[aes.BlockSize / 8];
                byte[] cipherText = new byte[encryptedTextAsArray.Length - IV.Length];

                Array.Copy(encryptedTextAsArray, IV, IV.Length);
                Array.Copy(encryptedTextAsArray, IV.Length, cipherText, 0, cipherText.Length);

                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(cipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            plaintext = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /**
         * decrypts with AES 128 ECB to bytes
         */
        public byte[] DecryptToBytes(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) return null;
            byte[] encryptedTextAsArray = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encriptionKey;
                aes.Mode = CipherMode;
                aes.BlockSize = 128;
                aes.Padding = Padding;

                byte[] IV = new byte[aes.BlockSize / 8];
                byte[] cipherText = new byte[encryptedTextAsArray.Length - IV.Length];

                Array.Copy(encryptedTextAsArray, IV, IV.Length);
                Array.Copy(encryptedTextAsArray, IV.Length, cipherText, 0, cipherText.Length);

                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherText, 0, cipherText.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
