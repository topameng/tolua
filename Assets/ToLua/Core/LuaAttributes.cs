using System;

namespace LuaInterface
{
    [AttributeUsage(AttributeTargets.Method)]    
    public class MonoPInvokeCallbackAttribute : Attribute
    {
        public MonoPInvokeCallbackAttribute(Type type)
        {
        }
    }

    public class NoToLuaAttribute : System.Attribute
    {
        public NoToLuaAttribute()
        {

        }
    }

    public class UseDefinedAttribute : System.Attribute
    {
        public UseDefinedAttribute()
        {

        }
    }
}