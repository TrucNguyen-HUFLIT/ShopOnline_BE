using System.Security.Cryptography;
using System.Text;

namespace ShopOnline.Core.Helpers
{
    public static class HashPasswordHelper
    {
        public static IHashPasswordStrategy HashPasswordStrategy { get; set; }

        public static string DoHash(string password) => HashPasswordStrategy.DoHash(password);
    }

    public interface IHashPasswordStrategy
    {
        string DoHash(string password);
    }

    //for Customer
    public class HashMD5Strategy : IHashPasswordStrategy
    {
        public string DoHash(string password)
        {
            MD5 mh = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new();
            for (int i = 0; i < hash.Length; i++)
            {
                // can be "x2" if you want lowercase
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    //for Shipper
    public class HashSHA1Strategy : IHashPasswordStrategy
    {
        public string DoHash(string password)
        {
            SHA1 sHA1 = SHA1.Create();
            byte[] hash = sHA1.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }

    //for Admin & Staff
    public class HashSHA256Strategy : IHashPasswordStrategy
    {
        public string DoHash(string password)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
