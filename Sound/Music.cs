using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO;
using WpfBasicElements.AbstractClasses;

namespace Sound
{
    public class Music: IMusic
    {
        public void Load(string directoryPath, Action<Exception> RespondToException)
        {
            string[] paths = Directory.GetFileSystemEntries(System.Environment.CurrentDirectory + directoryPath);
            SongUris = new List<Uri>();
            foreach (string path in paths)
            {
                FileInfo file = new FileInfo(path);
                if (file.Extension == ".wav" || file.Extension == ".m4v" || file.Extension == ".mp3" || file.Extension == ".mp4" || file.Extension == ".mp4v" || file.Extension == ".asx"
                    || file.Extension == ".wax" || file.Extension == ".wvx" || file.Extension == ".wmx" || file.Extension == ".wpl" || file.Extension == "asf" || file.Extension == ".wma"
                    || file.Extension == ".wmv" || file.Extension == ".wm" || file.Extension == ".mpg" || file.Extension == ".mpeg" || file.Extension == ".m1v" || file.Extension == ".mp2"
                    || file.Extension == ".mpa" || file.Extension == ".mpe" || file.Extension == ".m3u" || file.Extension == ".mid" || file.Extension == ".midi" || file.Extension == ".rmi"
                    || file.Extension == ".aif" || file.Extension == ".aifc" || file.Extension == ".aiff" || file.Extension == ".cda" || file.Extension == ".m4a" || file.Extension == ".3g2"
                    || file.Extension == ".3gp2" || file.Extension == ".3gp" || file.Extension == ".3gpp" || file.Extension == ".aac" || file.Extension == ".adt" || file.Extension == ".adts")
                {
                    Uri uri = new System.Uri(path);
                    SongUris.Add(uri);
                }
            }
            if (MusicMediaPlayer == null)
            {
                MusicMediaPlayer = new MediaPlayer();
                Random zufallMusik = new Random();
                MusicCounter = zufallMusik.Next(0, SongUris.Count);
                try
                {
                    MusicMediaPlayer.Open((Uri)SongUris[MusicCounter]);
                    MusicMediaPlayer.Play();
                    MusicMediaPlayer.Volume = MusicVolume;
                    MusicMediaPlayer.MediaEnded += NextSong;
                }
                catch (Exception e)
                {
                    RespondToException(e);
                }
            }
        }

        private int MusicCounter;
        private List<Uri> SongUris;
        public MediaPlayer MusicMediaPlayer { get; set; }
        public double MusicVolume { get; set; }

        private void NextSong(object sender, EventArgs a)
        {
            try
            {
                MusicMediaPlayer.Close();
                if (SongUris.Count > MusicCounter + 1) MusicCounter++;
                else MusicCounter = 0;
                MusicMediaPlayer.Open((Uri)SongUris[MusicCounter]);
                MusicMediaPlayer.Play();
                MusicMediaPlayer.Volume = MusicVolume;
            }
            catch { }
        }
    }
}
