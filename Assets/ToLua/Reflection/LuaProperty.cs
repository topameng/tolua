/*
Copyright (c) 2015-2016 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Globalization;
using System.Reflection;

namespace LuaInterface
{
    //代表一个反射属性
    public class LuaProperty
    {
        PropertyInfo property = null;
        Type kclass = null;        

        [NoToLuaAttribute]
        public LuaProperty(PropertyInfo prop, Type t)
        {
            property = prop;
            kclass = t;            
        }

        public int Get(IntPtr L)
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 2, kclass, typeof(object[])))
            {
                object arg0 = ToLua.ToVarObject(L, 2);
                object[] arg1 = ToLua.CheckObjectArray(L, 3);
                object o = property.GetValue(arg0, arg1);                
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 6 && TypeChecker.CheckTypes(L, 2, kclass, typeof(uint), typeof(Binder), typeof(object[]), typeof(CultureInfo)))
            {
                object arg0 = ToLua.ToVarObject(L, 2);
                BindingFlags arg1 = (BindingFlags)LuaDLL.lua_tonumber(L, 3);
                Binder arg2 = (Binder)ToLua.ToObject(L, 4);
                object[] arg3 = ToLua.CheckObjectArray(L, 5);
                CultureInfo arg4 = (CultureInfo)ToLua.ToObject(L, 6);
                object o = property.GetValue(arg0, arg1, arg2, arg3, arg4);
                ToLua.Push(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaInterface.LuaProperty.Get");
            }
        }

        public int Set(IntPtr L)
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 4 && TypeChecker.CheckTypes(L, 2, kclass, typeof(object), typeof(object[])))
            {
                object arg0 = ToLua.ToVarObject(L, 2);
                object arg1 = ToLua.ToVarObject(L, 3);
                object[] arg2 = ToLua.CheckObjectArray(L, 4);
                arg1 = Convert.ChangeType(arg1, property.PropertyType);
                property.SetValue(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 7 && TypeChecker.CheckTypes(L, 2, kclass, typeof(object), typeof(uint), typeof(Binder), typeof(object[]), typeof(CultureInfo)))
            {
                object arg0 = ToLua.ToVarObject(L, 2);
                object arg1 = ToLua.ToVarObject(L, 3);
                BindingFlags arg2 = (BindingFlags)LuaDLL.lua_tonumber(L, 4);
                Binder arg3 = (Binder)ToLua.ToObject(L, 5);
                object[] arg4 = ToLua.CheckObjectArray(L, 6);
                CultureInfo arg5 = (CultureInfo)ToLua.ToObject(L, 7);
                arg1 = Convert.ChangeType(arg1, property.PropertyType);
                property.SetValue(arg0, arg1, arg2, arg3, arg4, arg5);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaInterface.LuaProperty.Set");
            }            
        }
    }
}
