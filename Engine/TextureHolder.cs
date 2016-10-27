using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//This should reference what the current project is.
using MonoGameTiles;

namespace Engine
{
    public static class TextureHolder
    {
        private static Dictionary<AvailableTextures, Texture2D> _textures = new Dictionary<AvailableTextures, Texture2D>();
        private static bool IsInitialized = false;

        public static Texture2D Get(AvailableTextures texture)
        {
            if (IsInitialized)
            {
                return _textures[texture];
            }
            else
            {
                throw new Exception("You forgot to initialize TextureManager");
            }
        }

        public static void Init(Game game)
        {
            if (!IsInitialized)
            {
                foreach (AvailableTextures available_texture in Enum.GetValues(typeof(AvailableTextures)))
                {
                    string enum_string = available_texture.ToString();
                    _textures.Add(available_texture, game.Content.Load<Texture2D>(enum_string));
                }
                IsInitialized = true;
            }
        }
    }
}
