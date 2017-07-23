using System;

namespace iKalista.Modules
{
    [Flags]
    internal enum ModuleType
    {
        PreAttack = 1 << 0,
        PostAttack = 1 << 1,
        OnUpdate = 1 << 2,
        OnProcessSpell = 1 << 3,
        OnUnkillableMinion = 1 << 4
    }
}