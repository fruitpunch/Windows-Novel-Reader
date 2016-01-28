using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public enum SourceLocation { Web69, WebPiaoTian, WuxiaWorld };
    /*
    public class PlugInFactory<T>
    {
        public T CreatePlugin(string path)
        {
            foreach(string file in Directory.GetFiles(path, "*.dll"))
            {
                foreach (Type assemblyType in Assembly.LoadFrom(file).GetTypes())
                {
                    Type interfaceType = assemblyType.GetInterface(typeof(T).FullName);
                    if (interfaceType != null)
                    {
                        Console.WriteLine("Interface: " + assemblyType.FullName);
                        return (T)Activator.CreateInstance(assemblyType);
                    }
                }
            }
            return default(T);
        }
    }
    */
    public class SourceManager
    {
        /*
        //public static List<string> locations;

        public static void LoadSources()
        {
            //locations = new List<string>();
            //PlugInFactory<NovelSource> loader = new PlugInFactory<NovelSource>();
            //NovelSource instance = loader.CreatePlugin(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceDLL"));
        }
        */
        public static NovelSource GetSource(SourceLocation s, string sourceID)
        {
            NovelSource rtnSource;
            switch (s)
            {
                case SourceLocation.Web69:
                    rtnSource = new SourceWeb69(sourceID);
                    break;
                case SourceLocation.WebPiaoTian:
                    rtnSource = new SourcePiaoTian(sourceID);
                    break;
                case SourceLocation.WuxiaWorld:
                    rtnSource = new SourceWuxiaWorld(sourceID);
                    break;
                default:
                    rtnSource = null;
                    break;
            }
            return rtnSource;
        }

        public static string GetSourceURL(SourceLocation s)
        {
            switch (s)
            {
                case SourceLocation.Web69:
                    return new SourceWeb69(null).BaseURL;
                case SourceLocation.WebPiaoTian:
                    return new SourcePiaoTian(null).BaseURL;
                case SourceLocation.WuxiaWorld:
                    return new SourceWuxiaWorld(null).BaseURL;
                default:
                    return null;
            }
        }
    }
}
