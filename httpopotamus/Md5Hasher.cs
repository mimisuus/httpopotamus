using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace httpopotamus
{
    class Md5Hasher
    {
        public static string Md5_hash(string input)
        {
            MD5 m = MD5.Create();
            byte[] i = Encoding.ASCII.GetBytes(input);
            byte[] c = m.ComputeHash(i);
            StringBuilder stringbuilder = new StringBuilder();
            for (int x = 0; x < c.Length; x++)
            {
                stringbuilder.Append(c[x].ToString("X2"));
            }
            return stringbuilder.ToString();
        }
    }
}
