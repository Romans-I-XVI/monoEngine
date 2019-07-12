using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine
{
    interface ITouchable
    {
        void onTouchPressed(TouchLocation touch);
        void onTouch(TouchCollection touch);
        void onTouchReleased(TouchLocation touch);
    }
}
