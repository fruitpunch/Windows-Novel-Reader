using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

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
            catch (Exception e)
            {
                Console.WriteLine("Error getting URL: " + url);
                Console.WriteLine(e.ToString());
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
                Console.WriteLine("Error getting URL: " + url);
                Console.WriteLine(e.ToString());
                return null;
            }
            return lines.ToArray();
        }

        public static bool DownloadImage(string url, string destination)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] data = client.DownloadData(url);
                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        using (var image = Image.FromStream(mem))
                        {
                            image.Save(destination, ImageFormat.Png);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error getting URL: " + url);
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
            return true;
        }
    }
}
