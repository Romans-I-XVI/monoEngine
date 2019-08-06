using System;

namespace MonoEngine
{
    [Flags]
    public enum InputLayer
    {
        Zero = 0,
        One = 1 << 0,
        Two = 1 << 1,
        Three = 1 << 2,
        Four = 1 << 3,
        Five = 1 << 4,
        DebuggerTerminal = 1 << 5
    }
}
