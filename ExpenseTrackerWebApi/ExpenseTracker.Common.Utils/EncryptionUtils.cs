using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Common.Utils
{
    public class EncryptionUtils
    {
        public static string GetHash(string input)
        {
            SHA256 sha = SHA256.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha.ComputeHash(StringUtils.StringToByteArray(input));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}
