using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LuaInterface
{
    public class TypeChecker
    {
        public static bool IsValueType(Type t)
        {
            return !t.IsEnum && t.IsValueType;
        }
    }
}