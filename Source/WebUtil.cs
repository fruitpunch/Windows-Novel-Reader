using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public static class WebUtil
    {
        //Download the webpage and write each line into string array.
        public static string[] GetUrlContents(string url)
        {
            List<string> lines = new List<string>();
            WebClient client = new WebClient();
            using (var stream = client.OpenRead(url))
            using (var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding(936)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);

            }
            return lines.ToArray();
        }
    }
}
