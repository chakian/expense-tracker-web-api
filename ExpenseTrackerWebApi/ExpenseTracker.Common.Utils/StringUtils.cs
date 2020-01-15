using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Common.Utils
{
    public class StringUtils
    {
        public static byte[] StringToByteArray(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }
        public static string ByteArrayToString(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
    }
}
