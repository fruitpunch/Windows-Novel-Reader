using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    //This class contains the utility function used for getting source for novels.
    public static class WebUtil
    {
        //Download the webpage and write each line into string array.
        public static string[] GetUrlContents(string url)
        {
            List<string> lines = new List<string>();
            WebClient client = new WebClient();
            try
            {
                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream, client.Encoding))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        lines.Add(line);

                }
            }
            catch (Exception)
            {
                return null;
            }
            return lines.ToArray();
        }

        public static string[] GetUrlContentsUTF8(string url)
        {
            List<string> lines = new List<string>();
            WebClient client = new WebClient();
            try
            {
                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        lines.Add(line);

                }
            }
            catch (Exception e)
            {
                return null;
            }
            return lines.ToArray();
        }
    }
}
