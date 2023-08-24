using System;

namespace CharacterModule
{
    [Flags]
    public enum Stats
    {
        None = 0x0,
        Invincible = 0x1,
        Spawning = 0x2
    }
}