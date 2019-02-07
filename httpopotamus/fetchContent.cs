using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace httpopotamus
{
    public static class FetchContent
    {
        public static String GetPageHash(String url)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String content = reader.ReadToEnd();
            String Hash = Md5Hasher.Md5_hash(content);
            reader.Close();
            dataStream.Close();
            response.Close();
            return Hash;
        }
    }
}
