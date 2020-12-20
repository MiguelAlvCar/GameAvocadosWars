using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.IO;

namespace WpfBasicElements.AbstractClasses
{
    public interface IMusic
    {
        void Load(string directoryPath, Action<Exception> RespondToException);

        MediaPlayer MusicMediaPlayer { get; set; }
        double MusicVolume { get; set; }
    }

    public interface IEffects
    {
        MediaPlayer AttackEffectsMediaPlayer { get; set; }
        MediaPlayer DefenseEffectsMediaPlayer { get; set; }
        double VolumenEffekte { get; set; }

        void Play(Uri uri, MediaPlayer mediaPlayerEffekte);

    }
}
