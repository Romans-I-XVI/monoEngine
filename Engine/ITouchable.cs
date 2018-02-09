using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    interface ITouchable
    {
        void onTouchPressed(TouchLocation touch);
        void onTouch(TouchCollection touch);
        void onTouchReleased(TouchLocation touch);
    }
}
