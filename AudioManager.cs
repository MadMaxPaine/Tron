using System;
using Microsoft.Xna.Framework.Audio;

namespace Tron
{ static class AudioManager
    {
        private static readonly AudioEngine Engine;
        private static readonly SoundBank SoundBank;
        private static WaveBank _waveBank;
        public static Cue Level;
        public static Cue Menu;
        public static Cue Md;
        public static Cue Bp;
        public static Cue Hd;
        public static Cue Em;
        public static Cue Death;


        static AudioManager()
        {
            Engine = new AudioEngine(@"Content\Audio\TronAudio.xgs");
            SoundBank = new SoundBank(Engine, @"Content\Audio\Sound Bank.xsb");
            _waveBank = new WaveBank(Engine, @"Content\Audio\Wave Bank.xwb");
        }
        public static void Update()
        {
            Engine.Update();
        }
        public static void PlayLoserTheme()
        {
            if (Death == null)
            {
                Death = SoundBank.GetCue("death");
                Death.Play();
            }

        }
        public static void PlayTitleTheme()
        {
            if (Em == null)
            {
                Em = SoundBank.GetCue("EM");
                Em.Play();
            }

        }
        public static void PlayMenuTheme()
        {
            if (Menu == null)
            {
                Menu = SoundBank.GetCue("Menu");
                Menu.Play();
            }
            if (Menu != null)
            {
                Menu.Resume();
            }
        }
        public static void StartLevelTheme()
        {
            if (Level == null)
            {
                Level = SoundBank.GetCue("level");
                Level.Play();
            }
            if (Level != null)
            {
                Level.Resume();
            }
        }        
        public static void PlayMonsterDies()
        {
            if (Md == null)
            {
                Md = SoundBank.GetCue("md");
                Md.Play();
            }

        }
        public static void PlayBonusPicked()
        {
            if (Bp == null)
            {
                Bp = SoundBank.GetCue("bp");
                Bp.Play();
            }

        }
        public static void PlayHeroDies()
        {
            if (Hd == null)
            {
                Hd = SoundBank.GetCue("hd");
                Hd.Play();
            }

        }

        
        public static void StopLoserTheme()
        {
            if (Death != null)
            {
                Death.Stop(AudioStopOptions.Immediate);
            }
        }       
        public static void StopTitleTheme()
        {
            if (Em != null)
            {
                Em.Stop(AudioStopOptions.Immediate);
            }
        }       
        public static void StopMenuTheme()
        {
            if (Menu != null)
            {
                Menu.Stop(AudioStopOptions.Immediate);
            }
        }        
        public static void StopLevelTheme()
        {
            if (Level != null)
            {
                Level.Stop(AudioStopOptions.Immediate);
            }
        }
        
        
    }
}