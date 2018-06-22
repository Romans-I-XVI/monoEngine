using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Engine
{
    public static class Utilities
    {
        public static float FitAreaToArea(float source_width, float source_height, float dest_width, float dest_height)
        {
            if (dest_width / dest_height < source_width / source_height)
                return dest_width / source_width;
            else if (dest_width / dest_height > source_width / source_height)
                return dest_height / source_height;
            else
                return dest_width / source_width;
        }

        public static float FitAreaToArea(Vector2 source_area, Vector2 dest_area)
        {
            return FitAreaToArea(source_area.X, source_area.Y, dest_area.X, dest_area.Y);
        }

        public static void Try(Action action)
        {
            if (action != null)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("An error occurred: '{0}'", ex);
                }
            }
        }

        public static string GetSmallestInDictionary(Dictionary<string, int> dic)
        {
            string smallest_key = null;

            foreach (string key in dic.Keys)
            {
                if (smallest_key != null)
                {
                    if (dic[key] < dic[smallest_key])
                    {
                        smallest_key = key;
                    }
                }
                else
                {
                    smallest_key = key;
                }
            }

            return smallest_key;
        }
    }
}
