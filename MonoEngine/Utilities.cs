using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace MonoEngine
{
    public static class Utilities
    {
        public static float FitAreaToArea(float sourceWidth, float sourceHeight, float destWidth, float destHeight)
        {
            if (destWidth / destHeight < sourceWidth / sourceHeight)
                return destWidth / sourceWidth;
            else if (destWidth / destHeight > sourceWidth / sourceHeight)
                return destHeight / sourceHeight;
            else
                return destWidth / sourceWidth;
        }

        public static float FitAreaToArea(Vector2 sourceArea, Vector2 destArea)
        {
            return FitAreaToArea(sourceArea.X, sourceArea.Y, destArea.X, destArea.Y);
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
            string smallestKey = null;

            foreach (string key in dic.Keys)
            {
                if (smallestKey != null)
                {
                    if (dic[key] < dic[smallestKey])
                    {
                        smallestKey = key;
                    }
                }
                else
                {
                    smallestKey = key;
                }
            }

            return smallestKey;
        }
    }
}
