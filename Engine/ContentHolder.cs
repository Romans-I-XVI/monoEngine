using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.BitmapFonts;

//This should reference what the current project is.
using MonoGameTiles;

namespace Engine
{
    public static class ContentHolder
    {
        private static readonly Dictionary<AvailableTextures, Texture2D> _textures = new Dictionary<AvailableTextures, Texture2D>();
        private static readonly Dictionary<AvailableMusic, Song> _songs = new Dictionary<AvailableMusic, Song>();
        private static readonly Dictionary<AvailableSounds, SoundEffect> _sounds = new Dictionary<AvailableSounds, SoundEffect>();
        private static readonly Dictionary<AvailableFonts, BitmapFont> _bitmap_fonts = new Dictionary<AvailableFonts, BitmapFont>();
        private static bool IsInitialized = false;

        public static Texture2D Get(AvailableTextures texture)
        {
            if (IsInitialized)
            {
                return _textures[texture];
            }
            else
            {
                throw new Exception("You forgot to initialize ContentManager");
            }
        }

        public static BitmapFont Get(AvailableFonts font)
        {
            if (IsInitialized)
            {
                return _bitmap_fonts[font];
            }
            else
            {
                throw new Exception("You forgot to initialize the ContentManager");
            }
        }

        public static Song Get(AvailableMusic song)
        {
            if (IsInitialized)
            {
                return _songs[song];
            }
            else
            {
                throw new Exception("You forgot to initialize the ContentManager");
            }
        }

        public static SoundEffect Get(AvailableSounds sound)
        {
            if (IsInitialized)
            {
                return _sounds[sound];
            }
            else
            {
                throw new Exception("You forgot to initialize the ContentManager");
            }
        }

        public static void Init(Game game)
        {
            if (!IsInitialized)
            {
                foreach (AvailableTextures available_texture in Enum.GetValues(typeof(AvailableTextures)))
                {
                    string enum_string = available_texture.ToString();
                    _textures.Add(available_texture, game.Content.Load<Texture2D>("textures/"+enum_string));
                }
                foreach (AvailableFonts available_font in Enum.GetValues(typeof(AvailableFonts)))
                {
                    string enum_string = available_font.ToString();
                    _bitmap_fonts.Add(available_font, game.Content.Load<BitmapFont>("fonts/" + enum_string));
                }
                foreach (AvailableMusic available_song in Enum.GetValues(typeof(AvailableMusic)))
                {
                    string enum_string = available_song.ToString();
                    _songs.Add(available_song, game.Content.Load<Song>("music/" + enum_string));
                }
                foreach (AvailableSounds available_sound in Enum.GetValues(typeof(AvailableSounds)))
                {
                    string enum_string = available_sound.ToString();
                    _sounds.Add(available_sound, game.Content.Load<SoundEffect>("sounds/" + enum_string));
                }
                IsInitialized = true;
            }
        }
    }
}
