using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class SourceManager
    {
        public enum Sources { web69 };

        public static Source GetSource(string novelTitle, int sourceID, Sources s)
        {
            Source rtnSource;
            switch (s)
            {
                case Sources.web69:
                    rtnSource = new SourceWeb69(novelTitle, sourceID);
                    break;
                default:
                    rtnSource = null;
                    break;
            }
            return rtnSource;
        }
    }
}
