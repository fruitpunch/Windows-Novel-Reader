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
                default:
                    return null;
            }
        }
    }
}
