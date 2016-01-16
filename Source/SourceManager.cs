using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public enum SourceLocation { Web69, WebPiaoTian };

    public class SourceManager
    {

        public static Source GetSource(string novelTitle, int sourceID, SourceLocation s)
        {
            Source rtnSource;
            switch (s)
            {
                case SourceLocation.Web69:
                    rtnSource = new SourceWeb69(novelTitle, sourceID);
                    break;
                case SourceLocation.WebPiaoTian:
                    rtnSource = new SourcePiaoTian(novelTitle, sourceID);
                    break;
                default:
                    rtnSource = null;
                    break;
            }
            return rtnSource;
        }
    }
}
