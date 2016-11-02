using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

//This should reference what the current project is.
using MonoGameTiles;

namespace Engine
{
    public static class ContentHolder
    {
        private static readonly Dictionary<AvailableTextures, Texture2D> _textures = new Dictionary<AvailableTextures, Texture2D>();
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
                IsInitialized = true;
            }
        }
    }
}
