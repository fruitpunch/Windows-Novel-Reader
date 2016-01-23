using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using NAudio.Wave;
using NAudio.Lame;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace TTS
{
    class MandarinPromptSpeaker 
    {
        private enum DeleteOperation { None, DeleteLine, DeleteAllAfter };
        private string inputText;
        private PromptBuilder pb;
        private Dictionary<string, string> replacementDictionary;
        private Dictionary<string, DeleteOperation> deleteDictionary;
        private string fileName = null;
        private SpeechSynthesizer synth;
        private MemoryStream ms;
        private string outputFileLocation;

        private int defaultRate = 0;
        private static readonly int defaultVolume = 70;

        public MandarinPromptSpeaker(string fileName, string voiceName, int rate, string outputFileLocation, string deleteSpecificationLocation)
        {
            this.fileName = fileName;
            this.pb = new PromptBuilder();
            this.pb.Culture = new CultureInfo("zh-CN");
            this.outputFileLocation = outputFileLocation;
            this.defaultRate = rate;

            this.replacementDictionary = new Dictionary<string, string>();
            if(File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "MandarinSoundReplacement.txt"))){
                using (StreamReader sr = new StreamReader(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "MandarinSoundReplacement.txt")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = Regex.Split(line, ",");
                        if (parts.Length == 2)
                        {
                            replacementDictionary[parts[0]] = parts[1];
                        }
                    }
                }
            }

            this.deleteDictionary = new Dictionary<string, DeleteOperation>();
            if(File.Exists(deleteSpecificationLocation))
            {
                using (StreamReader sr = new StreamReader(deleteSpecificationLocation))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = Regex.Split(line, ",");
                        if (parts.Length == 2)
                        {
                            if (parts[1].Equals("DeleteLine"))
                                deleteDictionary[parts[0]] = DeleteOperation.DeleteLine;
                            else if (parts[1].Equals("DeleteAllAfter"))
                                deleteDictionary[parts[0]] = DeleteOperation.DeleteAllAfter;
                        }
                    }
                }
            }


            this.synth = new SpeechSynthesizer();
            this.synth.SelectVoice(voiceName);

            if (outputFileLocation != null)
            {
                this.ms = new MemoryStream();
                this.synth.SetOutputToWaveStream(ms);
            }
            else
                this.synth.SetOutputToDefaultAudioDevice();

        }

        //Parse the text input.
        public void ParseText(string text)
        {
            inputText = text.ToLower(new CultureInfo("en-US", false));
            inputText = TextReplace(inputText);
            inputText = inputText.Replace("“", "\"");
            inputText = inputText.Replace("”", "\"");
            string[] lines = Regex.Split(inputText, "\n");

            List<string> sentences = new List<string>();
            List<string> symbols = new List<string>();
            List<bool> quotes = new List<bool>();

            //First split the text by sentences.
            foreach (string line in lines)
            {

                if (line.Length == 1 && line.GetHashCode() == -842352733)
                    continue;
                else if (line.Length == 0)
                    continue;

                //If specified string exist in the string, delete the line or break off all.
                DeleteOperation operation = DeleteOperation.None;
                foreach (KeyValuePair<string, DeleteOperation> kvp in deleteDictionary)
                {
                    if (line.Contains(kvp.Key))
                    {
                        operation = kvp.Value;
                        break;
                    }
                }
                if (operation == DeleteOperation.DeleteAllAfter)
                    break;
                else if (operation == DeleteOperation.DeleteLine)
                    continue;

                string[] quoteSplit = Regex.Split(line, "\"");
                //Split each line by quote and non quote.
                for (int q = 0; q < quoteSplit.Length; q++)
                {
                    bool quoted = q % 2 == 1 ? true : false;
                    string[] parts = Regex.Split(quoteSplit[q], "([，…。：？！])");

                    string sentence = "";
                    string symbol = "";
                    //Split each quote/nonquote strings by specified punctuation.
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i].Length == 0)
                            continue;
                        sentence = parts[i];

                        while (i + 1 < parts.Length)
                        {
                            if (parts[i + 1].GetHashCode() == 757602046)
                            {
                                i++;
                                continue;
                            }
                            if ("，…。：？！".Contains(parts[i + 1]))
                            {
                                symbol += parts[i + 1];
                                i++;
                                continue;
                            }
                            break;
                        }
                        if (symbol.Length == 0)
                            symbol = "。";

                        sentences.Add(sentence);
                        symbols.Add(symbol);
                        quotes.Add(quoted);
                        sentence = "";
                        symbol = "";
                    }

                }
            }

            //Build prompt for each string.
            for (int i = 0; i < sentences.Count; i++)
            {
                BuildPrompt(sentences[i], symbols[i], quotes[i]);
                int percent = (int)(100.0f * (float)(i + 1) / (float)sentences.Count);
                Console.WriteLine(percent);
            }

            if (outputFileLocation != null)
            {
                ms.Seek(0, SeekOrigin.Begin);

                //Convert from wav to mp3 to save space
                using (WaveFileReader rdr = new WaveFileReader(ms))
                using (LameMP3FileWriter wtr = new LameMP3FileWriter(outputFileLocation, rdr.WaveFormat, LAMEPreset.VBR_90))
                {
                    rdr.CopyTo(wtr);
                }
            }
        }

        //Takes the string, punctuation and boolean of whether the string is in quote.
        //Use the punctuation and quote to determine how each string is spoken(speed and volume) and the length of break between each string.
        private void BuildPrompt(string sentence, string symbol, bool quoted)
        {
            string distinct = new String(sentence.Distinct().ToArray());
            bool isDistinct = distinct.Length == 1 ? true : false;
            bool shortened = isDistinct && sentence.Length > 2 ? true : false;

            string str = sentence + symbol;

            if (symbol.Contains("！"))
            {
                if(symbol.Contains("？"))
                    Speak(str, PromptRate.Fast, PromptVolume.Loud, quoted);
                else if(shortened)
                    Speak(str, PromptRate.Medium, PromptVolume.Loud, quoted);
                else if(isDistinct)
                    Speak(str, PromptRate.ExtraFast, PromptVolume.Loud, quoted);
                else
                    Speak(str, PromptRate.Fast, PromptVolume.Loud, quoted);
                AppendBreak(75);
            }
            else if (symbol.Contains("？"))
            {
                if (shortened)
                    Speak(str, PromptRate.Medium, PromptVolume.Loud, quoted);
                else
                    Speak(str, PromptRate.Medium, PromptVolume.Medium, quoted);
                AppendBreak(200);
            }
            else if (symbol.Contains("…"))
            {
                if (shortened)
                    Speak(str, PromptRate.Medium, PromptVolume.Soft, quoted);
                else
                    Speak(str, PromptRate.Medium, PromptVolume.ExtraSoft, quoted);
                AppendBreak(300);
            }
            else if (symbol.Contains("。"))
            {
                if (isDistinct && !shortened)
                    Speak(str, PromptRate.Fast, PromptVolume.ExtraLoud, quoted);
                else if (isDistinct)
                    Speak(str, PromptRate.Fast, PromptVolume.Loud, quoted);
                else if (shortened)
                    Speak(str, PromptRate.Medium, PromptVolume.Loud, quoted);
                else
                    Speak(str, PromptRate.Medium, PromptVolume.Medium, quoted);
                AppendBreak(150);
            }
            else
            {
                if (isDistinct && !shortened)
                    Speak(str, PromptRate.Fast, PromptVolume.ExtraLoud, quoted);
                else if (isDistinct)
                    Speak(str, PromptRate.Fast, PromptVolume.Loud, quoted);
                else if (shortened)
                    Speak(str, PromptRate.Medium, PromptVolume.Loud, quoted);
                else
                    Speak(str, PromptRate.Medium, PromptVolume.Medium, quoted);
                AppendBreak(50);
            }
        }

        //Speak the string with the specified rate and volume.
        private void Speak(string str, PromptRate rate, PromptVolume volume, bool quoted)
        {
            int speakRate = defaultRate;
            int speakVolume = defaultVolume;

            switch (rate)
            {
                case PromptRate.ExtraSlow:
                    speakRate -= 2;
                    break;
                case PromptRate.Slow:
                    speakRate -= 1;
                    break;
                case PromptRate.Medium:
                    speakRate += 0;
                    break;
                case PromptRate.Fast:
                    speakRate += 1;
                    break;
                case PromptRate.ExtraFast:
                    speakRate += 2;
                    break;
            }

            switch (volume)
            {
                case PromptVolume.Silent:
                    speakVolume -= 50;
                    break;
                case PromptVolume.ExtraSoft:
                    speakVolume -= 20;
                    break;
                case PromptVolume.Soft:
                    speakVolume -= 10;
                    break;
                case PromptVolume.NotSet:
                    speakVolume += 0;
                    break;
                case PromptVolume.Default:
                    speakVolume += 0;
                    break;
                case PromptVolume.Medium:
                    speakVolume += 0;
                    break;
                case PromptVolume.Loud:
                    speakVolume += 10;
                    break;
                case PromptVolume.ExtraLoud:
                    speakVolume += 20;
                    break;
            }

            if (quoted)
                speakVolume += 10;

            if (speakVolume > 100)
                speakVolume = 100;
            else if (speakVolume < 0)
                speakVolume = 0;

            if (speakRate > 10)
                speakRate = 10;
            else if (speakRate < -10)
                speakRate = -10;

            synth.Volume = speakVolume;
            synth.Rate = speakRate;
            synth.Speak(str);
        }

        //Append a break between each string.
        private void AppendBreak(int ms)
        {
            int ns = ms * 10000;
            pb.ClearContent();
            pb.AppendBreak(new TimeSpan(ns));
            synth.Speak(pb);
            pb.ClearContent();
        }

        //Replace each string with the specific string specified.
        //Replace the longest string first to reduce conflict.
        private string TextReplace(string input)
        {
            List<Tuple<string, string>> sortedReplacement = new List<Tuple<string, string>>();

            foreach (KeyValuePair<string, string> kvp in replacementDictionary)
            {
                int i;
                for (i = 0; i < sortedReplacement.Count; i++)
                {
                    if (kvp.Key.Length >= sortedReplacement[i].Item1.Length)
                        break;
                }
                sortedReplacement.Insert(i, new Tuple<string, string>(kvp.Key, kvp.Value));
            }

            foreach (Tuple<string, string> t in sortedReplacement)
            {
                if (t.Item1.Length == 0)
                    continue;
                else if (t.Item2.Length == 0)
                    input = input.Replace(t.Item1, "");
                else
                    input = input.Replace(t.Item1, t.Item2);
            }

            return input;
        }

    }
}
