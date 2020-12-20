using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO;
using WpfBasicElements.AbstractClasses;

namespace Sound
{
    public class Effects: IEffects
    {
        public MediaPlayer AttackEffectsMediaPlayer { get; set; } = new MediaPlayer();
        public MediaPlayer DefenseEffectsMediaPlayer { get; set; } = new MediaPlayer();
        public double VolumenEffekte { get; set; }
        public void Play(Uri uri, MediaPlayer mediaPlayerEffekte)
        {
            if (mediaPlayerEffekte != null) mediaPlayerEffekte.Close();
            mediaPlayerEffekte.Open(uri);
            mediaPlayerEffekte.Play();
            mediaPlayerEffekte.Volume = VolumenEffekte;
        }
    }

    public class UriEffect
    {
        private List<Uri> UriList = new List<Uri>();

        public Uri EffectUri
        {
            get
            {
                Random EffectRandom = new Random();
                int RandomEffectNumber = EffectRandom.Next(0, UriList.Count);
                return UriList[RandomEffectNumber];
            }
        }

        public UriEffect(string first5LettersOfSoundFiles, string directoryPath)
        {
            string[] pathsEffekte = Directory.GetFileSystemEntries(Environment.CurrentDirectory + directoryPath);
            foreach (string path in pathsEffekte)
            {
                FileInfo file = new FileInfo(path);
                if (file.Extension == ".wav" || file.Extension == ".m4v" || file.Extension == ".mp3" || file.Extension == ".mp4" || file.Extension == ".mp4v" || file.Extension == ".asx"
                    || file.Extension == ".wax" || file.Extension == ".wvx" || file.Extension == ".wmx" || file.Extension == ".wpl" || file.Extension == "asf" || file.Extension == ".wma"
                    || file.Extension == ".wmv" || file.Extension == ".wm" || file.Extension == ".mpg" || file.Extension == ".mpeg" || file.Extension == ".m1v" || file.Extension == ".mp2"
                    || file.Extension == ".mpa" || file.Extension == ".mpe" || file.Extension == ".m3u" || file.Extension == ".mid" || file.Extension == ".midi" || file.Extension == ".rmi"
                    || file.Extension == ".aif" || file.Extension == ".aifc" || file.Extension == ".aiff" || file.Extension == ".cda" || file.Extension == ".m4a" || file.Extension == ".3g2"
                    || file.Extension == ".3gp2" || file.Extension == ".3gp" || file.Extension == ".3gpp" || file.Extension == ".aac" || file.Extension == ".adt" || file.Extension == ".adts")
                {
                    if (file.Name.Substring(0, 5) == first5LettersOfSoundFiles)
                    {
                        UriList.Add(new Uri(path));
                    }
                }
            }

        }

    }
}
