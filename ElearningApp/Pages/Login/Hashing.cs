using System.Security.Cryptography;
using System.Text;

namespace ElearningApp.Pages.Login
{
    public class Hashing
    {
        public string PassHash(string data)
        {
            SHA1 hashing = SHA1.Create();
            byte[] hashdata = hashing.ComputeHash(Encoding.Default.GetBytes(data));
            string base64Encoded = Convert.ToBase64String(hashdata);

            return base64Encoded;
        }
    }
}
