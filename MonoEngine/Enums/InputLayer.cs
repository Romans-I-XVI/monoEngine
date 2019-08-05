using System;

namespace MonoEngine
{
    [Flags]
    public enum InputLayer
    {
        One = 1 << 0,
        Two = 1 << 1,
        Three = 1 << 2,
        Four = 1 << 3,
        Five = 1 << 4,
    }
}
