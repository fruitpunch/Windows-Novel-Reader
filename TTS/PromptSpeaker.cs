using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTS
{
    interface PromptSpeaker
    {
        void ParseText(string text);
        void Speak(string voiceName, int rate, string outputFileLocation);
    }
}
