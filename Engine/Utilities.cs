﻿using System;
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
    }
}