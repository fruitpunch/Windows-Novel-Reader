using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Source
{

    struct SourceInfo
    {
        private Type _type { get; set; }
        private string _name { get; set; }
        private string _fullName { get; set; }
        private ISource _source { get; set; }

        public Type Type
        {
            get { return _type; }
        }
        public string Name
        {
            get { return _name; }
        }

        public string FullName
        {
            get { return _fullName; }
        }

        public ISource Source
        {
            get { return _source; }
        }

        public SourceInfo(Type type)
        {
            this._type = type;
            this._name = type.Name;
            this._fullName = type.FullName;
            this._source = Activator.CreateInstance(type, new object[] { null, null}) as ISource;
        }
    }

    public class SourceManager
    {

        private static Dictionary<string, SourceInfo> sourceDictionary { get; set; }

        public static List<string> _sourceLocation { get; set; }

        public static List<string> SourceLocation
        {
            get { return _sourceLocation; }
        }


        public static bool LoadSourcePack()
        {
            sourceDictionary = new Dictionary<string, SourceInfo>();
            _sourceLocation = new List<string>();
            string[] sourcePackDLLs = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "SourcePack"), "*.dll");

            foreach(string dllName in sourcePackDLLs)
            {
                
                var assembly = Assembly.LoadFrom(dllName);
            
                if (assembly == null)
                {
                    Console.WriteLine("Invalid dll");
                    continue;
                }
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(ISource).IsAssignableFrom(type))
                        sourceDictionary[type.FullName] = new SourceInfo(type);

                }
            }
            foreach (KeyValuePair<string, SourceInfo> kvp in sourceDictionary)
            {
                SourceLocation.Add(kvp.Key);
            }
            return true;
        }


        public static ISource GetSource(string sourceLocation, string sourceID)
        {
            if (sourceDictionary.ContainsKey(sourceLocation))
            {
                object[] args = new object[] { sourceID, null };
                return Activator.CreateInstance(sourceDictionary[sourceLocation].Type, args) as ISource;
            }
            return null;
        }

        public static string GetSourceURL(string sourceLocation)
        {
            if (sourceDictionary.ContainsKey(sourceLocation))
            {
                return sourceDictionary[sourceLocation].Source.Url;
            }
            return null;
        }
    }
}
