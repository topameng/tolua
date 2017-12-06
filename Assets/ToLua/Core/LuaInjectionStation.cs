using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaInterface
{
    enum InjectType
    {
        Replace = 1,
        Before,
        After,
        ReplaceWithPreInvokeBase,
        ReplaceWithPostInvokeBase
    }

    public static class LuaInjectionStation
    {
        public const byte INVALID_INJECTION_FLAG = 0;
        const int cacheSize = char.MaxValue;

        static byte[] injectionCache = new byte[cacheSize];

        public static byte GetInjectFlag(int index)
        {
            byte result = INVALID_INJECTION_FLAG;

            if (index >= injectionCache.Length)
            {
                ExpandCache(index);
            }
            else
            {
                result = injectionCache[index];
                if (result == 0)
                {
                    result = LuaState.GetInjectFlag(index);
                    /// Delay injection not supported
                    injectionCache[index] = result;
                }
            }

            return result;
        }

        static void ExpandCache(int size)
        {
            byte[] invalidCache = injectionCache;
            injectionCache = new byte[size];
            Array.ConstrainedCopy(invalidCache, 0, injectionCache, 0, invalidCache.Length);
        }
    }
}
