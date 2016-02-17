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
using Microsoft.Speech.Synthesis;
using Microsoft.Speech.AudioFormat;

namespace TTS
{
    class Program
    {
        static int textLength = 0;
        static string fileName = "";
        static int synthesizerChoice = 0; //0 = None, 1 = System, 2 = Microsoft
        static System.Speech.AudioFormat.SpeechAudioFormatInfo sysAF;
        static Microsoft.Speech.AudioFormat.SpeechAudioFormatInfo msftAF;

        //Post update of progress for System Speech Synthesizer
        static void SynthProgressUpdate(object sender, System.Speech.Synthesis.SpeakProgressEventArgs e)
        {
            int percent = (int) (100.0f * (float)e.CharacterPosition/ (float)textLength);
            Console.Write("\r " + fileName + " Progress: {0}%   ", percent);
        }

        //Post update of progress for Microsoft Speech Synthesizer
        static void SynthProgressUpdate(object sender, Microsoft.Speech.Synthesis.SpeakProgressEventArgs e)
        {
            int percent = (int)(100.0f * (float)e.CharacterPosition / (float)textLength);
            Console.Write("\r " + fileName + " Progress: {0}%   ", percent);
        }

        //Show a list of installed languages for both System and Microsoft
        static void ShowInstalledLanguage()
        {
            using (System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer())
            {
                try
                {
                    foreach (System.Speech.Synthesis.InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        System.Speech.Synthesis.VoiceInfo info = voice.VoiceInfo;
                        Console.WriteLine(" Name:          " + info.Name);
                        Console.WriteLine(" Culture:       " + info.Culture);
                        Console.WriteLine(" Age:           " + info.Age);
                        Console.WriteLine(" Gender:        " + info.Gender);
                        Console.WriteLine(" ID:            " + info.Id);
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                }
            }

            using (Microsoft.Speech.Synthesis.SpeechSynthesizer synth = new Microsoft.Speech.Synthesis.SpeechSynthesizer())
            {
                try
                {
                    foreach (Microsoft.Speech.Synthesis.InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        Microsoft.Speech.Synthesis.VoiceInfo info = voice.VoiceInfo;
                        Console.WriteLine(" Name:          " + info.Name);
                        Console.WriteLine(" Culture:       " + info.Culture);
                        Console.WriteLine(" Age:           " + info.Age);
                        Console.WriteLine(" Gender:        " + info.Gender);
                        Console.WriteLine(" ID:            " + info.Id);
                        Console.WriteLine();
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        //Validate whether the string entered for voice selection is valid
        static Tuple<string, string> ValidateVoiceSelection(string inputVoiceString)
        {
            
            using (System.Speech.Synthesis.SpeechSynthesizer msftSynth = new System.Speech.Synthesis.SpeechSynthesizer())
            {
                try
                {
                    foreach (System.Speech.Synthesis.InstalledVoice voice in msftSynth.GetInstalledVoices())
                    {
                        if (inputVoiceString.Equals(voice.VoiceInfo.Id) || inputVoiceString.Equals(voice.VoiceInfo.Name))
                        {
                            synthesizerChoice = 1;
                            return new Tuple<string, string>(voice.VoiceInfo.Name, voice.VoiceInfo.Culture.ToString());
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            using (Microsoft.Speech.Synthesis.SpeechSynthesizer sysSynth = new Microsoft.Speech.Synthesis.SpeechSynthesizer())
            {
                try
                {
                    foreach (Microsoft.Speech.Synthesis.InstalledVoice voice in sysSynth.GetInstalledVoices())
                    {
                        if (inputVoiceString.Equals(voice.VoiceInfo.Id) || inputVoiceString.Equals(voice.VoiceInfo.Name))
                        {
                            synthesizerChoice = 2;
                            return new Tuple<string, string>(voice.VoiceInfo.Name, voice.VoiceInfo.Culture.ToString());
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;

        }

        //Replaces strings indicated by user before speaking
        static string TextReplace(string input, Dictionary<string, string> replacementDictionary)
        {
            List<Tuple<string, string>> sortedReplacement = new List<Tuple<string, string>>();

            foreach (KeyValuePair<string, string> kvp in replacementDictionary)
            {
                int i;
                for (i = 0; i < sortedReplacement.Count; i++)
                {
                    if(kvp.Key.Length >= sortedReplacement[i].Item1.Length)
                        break;
                }
                sortedReplacement.Insert(i, new Tuple<string,string>(kvp.Key, kvp.Value));
            }

            foreach(Tuple<string, string> t in sortedReplacement)
            {
                if (t.Item1.Length == 0)
                    continue;
                else if(t.Item2.Length == 0)
                    input = input.Replace(t.Item1, "");
                else
                    input = input.Replace(t.Item1, t.Item2);
            }

            return input;
        }

        private static bool SetAudioFormat(string format)
        {
            
            int bitPerSample;
            System.Speech.AudioFormat.AudioBitsPerSample sysAudioBitsPerSample;
            Microsoft.Speech.AudioFormat.AudioBitsPerSample msftAudioBitsPerSample;
            System.Speech.AudioFormat.AudioChannel sysAudioChannel;
            Microsoft.Speech.AudioFormat.AudioChannel msftAudioChannel;
            decimal rawInput;

            string[] parts = Regex.Split(format, "_");

            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid Audio Format");
                return false;
            }

            if (Decimal.TryParse(parts[0], out rawInput) && (rawInput % 1) == 0 && rawInput > 0)
            {
                bitPerSample = (int) rawInput;
            }
            else
            {
                Console.WriteLine("Invalid Audio Format: SamplePerSecond must be a positive integer");
                return false;
            }


            if (Decimal.TryParse(parts[1], out rawInput) && (rawInput % 1) == 0 && rawInput > 0)
            {
                if (rawInput == 8.0M)
                {
                    sysAudioBitsPerSample = System.Speech.AudioFormat.AudioBitsPerSample.Eight;
                    msftAudioBitsPerSample = Microsoft.Speech.AudioFormat.AudioBitsPerSample.Eight;
                }
                else if (rawInput == 16.0M)
                {
                    sysAudioBitsPerSample = System.Speech.AudioFormat.AudioBitsPerSample.Sixteen;
                    msftAudioBitsPerSample = Microsoft.Speech.AudioFormat.AudioBitsPerSample.Sixteen;
                }
                else
                {
                    Console.WriteLine("Invalid Audio Format: BitsPerSample must be either 8 or 16");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid Audio Format: BitsPerSample must be either 8 or 16");
                return false;
            }

            if (parts[2].Equals("Mono"))
            {
                sysAudioChannel = System.Speech.AudioFormat.AudioChannel.Mono;
                msftAudioChannel = Microsoft.Speech.AudioFormat.AudioChannel.Mono;
            }
            else if (parts[2].Equals("Stereo"))
            {
                sysAudioChannel = System.Speech.AudioFormat.AudioChannel.Stereo;
                msftAudioChannel = Microsoft.Speech.AudioFormat.AudioChannel.Stereo;
            }
            else
            {
                Console.WriteLine("Invalid Audio Format: AudioChannel must be either Mono or Stereo");
                return false;
            }

            sysAF = new System.Speech.AudioFormat.SpeechAudioFormatInfo(bitPerSample, sysAudioBitsPerSample, sysAudioChannel);
            msftAF = new Microsoft.Speech.AudioFormat.SpeechAudioFormatInfo(bitPerSample, msftAudioBitsPerSample, msftAudioChannel);

            return true;

        }


        //Print the help info
        static void PrintHelp()
        {
            Console.WriteLine("Format: TTS.exe [VoiceID] \"Text Input\"");
            Console.WriteLine("Optional Parameter:");
            Console.WriteLine(" -l                                      : Lists installed languages");
            Console.WriteLine(" -i [input_text_file_location]           : Read from file(Cannot be set together with command line text input)");
            Console.WriteLine(" -o [output_file_location]               : Output speech to file(mp3 format by default)");
            Console.WriteLine(" -utf8                                   : You know for sure that the input file encoding is in utf-8.");
            Console.WriteLine(" -replace [replacement_text_location]    : Has a file with a list of string you want to replace. Must be utf8 encoded the with format: A,B where string A is replaced with string B");
            Console.WriteLine(" -delete [delete_text_location]          : Specify what to delete");
            Console.WriteLine(" -rate [-10 to 10]                       : Indicate how fast to speak");
            Console.WriteLine(" -wav [Audio Format]                     : Force output file to be .wav. Audio Format: [BitPerSample]_[AudioBitsSample:8|16]_[AudioChannel:Mono|Stereo]. Example: 24000_8_Mono");
            
        }

        static void Main(string[] args)
        {

            Boolean isUTF8 = false;
            Boolean wav = false;
            string speakText = null;
            string inputFileLocation = null;
            string outputFileLocation = null;
            string replacementFileLocation = null;
            string deleteSpecificationLocation = null;
            int rate = 0;
            Tuple<string, string> voiceAndCulture = null;
            
            //A lot of arguement checking
            if (args.Length > 0)
            {
                if((voiceAndCulture = ValidateVoiceSelection(args[0])) != null)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        if (args[i].Equals("-utf8"))
                            isUTF8 = true;
                        else if (args[i].Equals("-wav"))
                        {
                            if (i + 1 < args.Length)
                            {
                                if (!SetAudioFormat(args[i + 1]))
                                    return;
                                i++;
                                wav = true;
                            }
                            else
                            {
                                Console.WriteLine("Missing wav format");
                                return;
                            }
                        }
                        else if (args[i].Equals("-replace"))
                        {
                            if (i + 1 < args.Length)
                            {
                                replacementFileLocation = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("Missing replacement file name");
                                PrintHelp();
                                return;
                            }
                        }
                        else if (args[i].Equals("-delete"))
                        {
                            if (i + 1 < args.Length)
                            {
                                deleteSpecificationLocation = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("Missing DeleteSpecification file name");
                                PrintHelp();
                                return;
                            }
                        }
                        else if (args[i].Equals("-rate"))
                        {
                            if (i + 1 < args.Length)
                            {
                                string s = args[i + 1];
                                i++;

                                if (!Int32.TryParse(s, out rate) || rate < -10 || rate > 10)
                                {
                                    Console.WriteLine("Rate must be an integer between -10 and 10");
                                    PrintHelp();
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Missing replacement file name");
                                PrintHelp();
                                return;
                            }
                        }
                        else if (args[i].Equals("-i"))
                        {
                            if (speakText != null)
                            {
                                Console.WriteLine("Cannot use both file and command line for text input");
                                return;
                            }
                            else if (i + 1 < args.Length)
                            {
                                inputFileLocation = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("Missing input file name");
                                return;
                            }
                        }
                        else if (args[i].Equals("-o"))
                        {
                            if (i + 1 < args.Length)
                            {
                                outputFileLocation = args[i + 1];
                                i++;
                            }
                            else
                            {
                                Console.WriteLine("Missing output file name");
                                return;
                            }
                        }
                        else
                        {
                            if (i == 1)
                                speakText = args[1];
                            else
                            {
                                Console.WriteLine("Unknown command: " + args[i]);
                                PrintHelp();
                                return;
                            }
                        }

                    }
                }
                else if (args[0].Equals("-l"))
                {
                    ShowInstalledLanguage();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid speaker name/ID: " + args[0]);
                    Console.WriteLine("Type \"TTS.exe -l\" to show list of installed language/voice");
                    return;
                }
            }
            else
            {
                PrintHelp();
                return;
            }

            if (voiceAndCulture == null)
            {
                Console.WriteLine("Invalid voice ID");
                PrintHelp();
                return;
            }

            if (speakText == null && inputFileLocation == null)
            {
                Console.WriteLine("No text input");
                PrintHelp();
                return;
            }
            
            string voiceName = voiceAndCulture.Item1;
            string culture = voiceAndCulture.Item2;
             
            //Read the replacement text
            Dictionary<string, string> replacementDictionary = new Dictionary<string, string>();


            //============================== For developement only =======================================
            /*
            String voiceName = "VW Hui";
            String culture = "zh-CN";
            replacementFileLocation = "replacement.txt";
            inputFileLocation = "1.txt";
            //outputFileLocation = "out.mp3";
            isUTF8 = true;
            rate = 3;
            */
            //============================================================================================


            if (replacementFileLocation != null)
            {
                try
                {
                    //using (StreamReader sr = new StreamReader(inputFileLocation, Encoding.GetEncoding(pt.TextInfo.ANSICodePage), true))
                    using (StreamReader sr = new StreamReader(replacementFileLocation))
                    {
                        string line;
                        while((line = sr.ReadLine()) != null)
                        {
                            string[] parts = Regex.Split(line, ",");
                            if (parts.Length == 2)
                            {
                                replacementDictionary[parts[0]] = parts[1];
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid replacement file location");
                    PrintHelp();
                    return;
                }
            }
            
            //Read text file for input
            if (inputFileLocation != null)
            {
                try
                {
                    if (isUTF8)
                    {
                        using (StreamReader sr = new StreamReader(inputFileLocation))
                        {
                            speakText = sr.ReadToEnd();
                        }
                    }
                    else
                    {
                        CultureInfo pt = CultureInfo.GetCultureInfo(culture);
                        using (StreamReader sr = new StreamReader(inputFileLocation, Encoding.GetEncoding(pt.TextInfo.ANSICodePage), true))
                        {
                            speakText = sr.ReadToEnd();
                        }
                    }
                    fileName = Path.GetFileName(inputFileLocation);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input file location.");
                    PrintHelp();
                    return;
                }
            }

            speakText = TextReplace(speakText, replacementDictionary);
            textLength = speakText.Length;

            MandarinPromptSpeaker mps = new MandarinPromptSpeaker(fileName, voiceName, rate, outputFileLocation, deleteSpecificationLocation);
            mps.ParseText(speakText);
            //mps.Speak(voiceName, rate, outputFileLocation);

            /*
            if (synthesizerChoice == 1)     //Use System Speech Synthesizer
            {
                using (System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer())
                {
                    synth.SelectVoice(voiceName);
                    synth.Rate = rate;
                    synth.Volume = 100;

                    if (outputFileLocation != null)
                    {
                        synth.SpeakProgress += new EventHandler<System.Speech.Synthesis.SpeakProgressEventArgs>(SynthProgressUpdate);

                        if (wav)
                        {
                            synth.SetOutputToWaveFile(outputFileLocation, sysAF);
                            synth.Speak(speakText);
                            Console.WriteLine("\r " + fileName + " Progress: 100%   ");
                        }
                        else
                        {
                            MemoryStream ms = new MemoryStream();
                            synth.SetOutputToWaveStream(ms);
                            synth.Speak(speakText);
                            Console.WriteLine("\r " + fileName + " Progress: 100%   ");
                            ms.Seek(0, SeekOrigin.Begin);

                            //Convert from wav to mp3 to save space
                            using (var rdr = new WaveFileReader(ms))
                            using (var wtr = new LameMP3FileWriter(outputFileLocation, rdr.WaveFormat, LAMEPreset.VBR_90))
                            {
                                rdr.CopyTo(wtr);
                            }
                        }
                    }
                    else
                    {
                        synth.SetOutputToDefaultAudioDevice();
                        synth.Speak(speakText);
                    }
                }
            }
            else if (synthesizerChoice == 2)    //Use Microsoft Speech Sythnesizer
            {
                using (Microsoft.Speech.Synthesis.SpeechSynthesizer synth = new Microsoft.Speech.Synthesis.SpeechSynthesizer())
                {
                    synth.SelectVoice(voiceName);
                    synth.Rate = rate;
                    synth.Volume = 100;

                    if (outputFileLocation != null)
                    {
                        synth.SpeakProgress += new EventHandler<Microsoft.Speech.Synthesis.SpeakProgressEventArgs>(SynthProgressUpdate);
                        if (wav)
                        {
                            synth.SetOutputToWaveFile(outputFileLocation, msftAF);
                            synth.Speak(speakText);
                            Console.WriteLine("\r " + fileName + " Progress: 100%   ");
                        }
                        else
                        {
                            MemoryStream ms = new MemoryStream();
                            synth.SetOutputToWaveStream(ms);
                            synth.Speak(speakText);
                            Console.WriteLine("\r " + fileName + " Progress: 100%   ");
                            ms.Seek(0, SeekOrigin.Begin);

                            //Convert from wav to mp3 to save space
                            using (var rdr = new WaveFileReader(ms))
                            using (var wtr = new LameMP3FileWriter(outputFileLocation, rdr.WaveFormat, LAMEPreset.VBR_90))
                            {
                                rdr.CopyTo(wtr);
                            }
                        }
                    }
                    else
                    {
                        synth.SetOutputToDefaultAudioDevice();
                        synth.Speak(speakText);
                    }
                }
            }
            
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey(true);
             */
        }
    }
}
