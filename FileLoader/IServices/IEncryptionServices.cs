namespace FileLoader.IServices
{
    public interface IEncryptionServices
    {
        string EncryptToString(string plainText);
        string EncryptToString(byte[] bytes);
        string DecryptToString(string encryptedText);
    }
}
