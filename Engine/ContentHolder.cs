using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//This should reference what the current project is.
using MonoGameTiles;

namespace Engine
{
    public static class ContentHolder
    {
        private static readonly Dictionary<AvailableTextures, Texture2D> _textures = new Dictionary<AvailableTextures, Texture2D>();
        private static readonly Dictionary<AvailableFonts, SpriteFont> _fonts = new Dictionary<AvailableFonts, SpriteFont>();
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

        public static SpriteFont Get(AvailableFonts font)
        {
            if (IsInitialized)
            {
                return _fonts[font];
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
                    _fonts.Add(available_font, game.Content.Load<SpriteFont>("fonts/"+enum_string));
                }
                IsInitialized = true;
            }
        }
    }
}
