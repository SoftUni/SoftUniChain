using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NodeDotNet.Core.Utilities
{
    public static class Crypto
    {
        public static String Sha256(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
