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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace LuaInterface
{
    public class LuaState : IDisposable
    {
        public ObjectTranslator translator = new ObjectTranslator();                 

        public int ArrayMetatable { get; private set; }
        public int DelegateMetatable { get; private set; }
        public int TypeMetatable { get; private set; }
        public int EnumMetatable { get; private set; }
        public int IterMetatable { get; private set; }
        public int OutMetatable { get; private set; }
        public int EventMetatable { get; private set; }

        //function ref                
        public int PackBounds { get; private set; }
        public int UnpackBounds { get; private set; }
        public int PackRay { get; private set; }
        public int UnpackRay { get; private set; }
        public int PackRaycastHit { get; private set; }
        public int PackTouch { get; private set; }

        public bool LogGC 
        {
            get
            {
                return beLogGC;
            }

            set
            {
                beLogGC = value;
                translator.LogGC = value;
            }
        }

        protected IntPtr L;
        Dictionary<string, WeakReference> funcMap = new Dictionary<string, WeakReference>();
        Dictionary<int, WeakReference> funcRefMap = new Dictionary<int, WeakReference>();

        List<GCRef> gcList = new List<GCRef>();
        List<LuaFunction> subList = new List<LuaFunction>();

        Dictionary<Type, int> metaMap = new Dictionary<Type, int>();
        Dictionary<Enum, object> enumMap = new Dictionary<Enum, object>();

        Dictionary<int, Type> typeMap = new Dictionary<int, Type>();

#if !MULTI_STATE
        private static LuaState mainState = null;
#endif        
        private static Dictionary<IntPtr, LuaState> stateMap = new Dictionary<IntPtr, LuaState>();

        private int beginCount = 0;
        private bool beLogGC = false;

        public LuaState()
        {
#if !MULTI_STATE
            mainState = this;
#endif                       
            LuaException.Init();     
            L = LuaDLL.luaL_newstate();                                                                        
            LuaDLL.tolua_openlibs(L);
            LuaDLL.tolua_openint64(L);    
            LuaStatic.OpenLibs(L);                                                                                                  
            stateMap.Add(L, this);                       
            OpenBaseLibs();            
            LuaDLL.lua_settop(L, 0);

            if (!LuaFileUtils.Instance.beZip)
            {
                AddSearchPath(LuaConst.luaDir);
                AddSearchPath(LuaConst.toluaDir);
            }
        }

        void OpenBaseLibs()
        {            
            BeginModule(null);
            BeginModule("System");
            System_ObjectWrap.Register(this);
            System_NullObjectWrap.Register(this);            
            System_StringWrap.Register(this);
            System_DelegateWrap.Register(this);
            System_EnumWrap.Register(this);
            System_ArrayWrap.Register(this);
            System_TypeWrap.Register(this);
            LuaInterface_LuaOutWrap.Register(this);
            LuaInterface_EventObjectWrap.Register(this);
            BeginModule("Collections");
            System_Collections_IEnumeratorWrap.Register(this);
            EndModule();
            EndModule();
            BeginModule("UnityEngine");
            UnityEngine_ObjectWrap.Register(this);            
            EndModule();
            EndModule();
            
            ToLua.OpenLibs(L);
            LuaUnityLibs.OpenLibs(L);
            ArrayMetatable = metaMap[typeof(System.Array)];
            TypeMetatable = metaMap[typeof(System.Type)];
            DelegateMetatable = metaMap[typeof(System.Delegate)];
            EnumMetatable = metaMap[typeof(System.Enum)];
            IterMetatable = metaMap[typeof(IEnumerator)];
            EventMetatable = metaMap[typeof(EventObject)];
        }

        void OpenBaseLuaLibs()
        {
            DoFile("tolua.lua");
            LuaUnityLibs.OpenLuaLibs(L);
        }

        public void Start()
        {
            Debugger.Log("LuaState start");
            OpenBaseLuaLibs();
            PackBounds = GetFuncRef("Bounds.New");           
            UnpackBounds = GetFuncRef("Bounds.Get");            
            PackRay = GetFuncRef("Ray.New");
            UnpackRay = GetFuncRef("Ray.Get");
            PackRaycastHit = GetFuncRef("RaycastHit.New");
            PackTouch = GetFuncRef("Touch.New");
        }

        public int OpenLibs(LuaCSFunction open)
        {
            int ret = open(L);            
            return ret;
        }

        public bool BeginModule(string name)
        {
            if (LuaDLL.tolua_beginmodule(L, name))
            {
                ++beginCount;
                return true;
            }

            LuaDLL.lua_settop(L, 0);
            throw new LuaException(string.Format("create table {0} fail", name));            
        }

        public void EndModule()
        {
            --beginCount;
            LuaDLL.lua_pop(L, 1);
        }

        void BindTypeRef(int reference, Type t)
        {
            metaMap.Add(t, reference);
            typeMap.Add(reference, t);
        }

        public Type GetClassType(int reference)
        {
            Type t = null;
            typeMap.TryGetValue(reference, out t);
            return t;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int Collect(IntPtr L)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, 1);

            if (udata != -1)
            {
                ObjectTranslator translator = LuaState.GetTranslator(L);
                translator.RemoveObject(udata);
            }

            return 0;
        }

        public int BeginClass(Type t, Type baseType, string name = null)
        {
            if (beginCount == 0)
            {
                throw new LuaException("must call BeginModule first");
            }

            int baseMetaRef = 0;
            int reference = 0;            

            if (name == null)
            {
                name = t.Name;
            }

            if (baseType != null && !metaMap.TryGetValue(baseType, out baseMetaRef))
            {
                LuaDLL.lua_createtable(L, 0, 0);
                baseMetaRef = LuaDLL.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);                
                BindTypeRef(baseMetaRef, baseType);
            }

            if (metaMap.TryGetValue(t, out reference))
            {
                LuaDLL.tolua_beginclass(L, name, baseMetaRef, reference);
                RegFunction("__gc", Collect);
            }
            else
            {
                reference = LuaDLL.tolua_beginclass(L, name, baseMetaRef);
                RegFunction("__gc", Collect);                
                BindTypeRef(reference, t);
            }

            return reference;
        }

        public void EndClass()
        {
            LuaDLL.tolua_endclass(L);
        }

        public int BeginEnum(Type t)
        {
            if (beginCount == 0)
            {
                throw new LuaException("must call BeginModule first");
            }

            int reference = LuaDLL.tolua_beginenum(L, t.Name);
            RegFunction("__gc", Collect);            
            BindTypeRef(reference, t);
            return reference;
        }

        public void EndEnum()
        {
            LuaDLL.tolua_endenum(L);
        }

        public void BeginStaticLibs(string name)
        {
            if (beginCount == 0)
            {
                throw new LuaException("must call BeginModule first");
            }

            LuaDLL.tolua_beginstaticclass(L, name);
        }

        public void EndStaticLibs()
        {
            LuaDLL.tolua_endstaticclass(L);
        }

        public void RegFunction(string name, LuaCSFunction func)
        {
            IntPtr fn = Marshal.GetFunctionPointerForDelegate(func);
            LuaDLL.tolua_function(L, name, fn);
        }

        public void RegVar(string name, LuaCSFunction get, LuaCSFunction set)
        {            
            IntPtr fget = IntPtr.Zero;
            IntPtr fset = IntPtr.Zero;

            if (get != null)
            {
                fget = Marshal.GetFunctionPointerForDelegate(get);
            }

            if (set != null)
            {
                fset = Marshal.GetFunctionPointerForDelegate(set);
            }

            LuaDLL.tolua_variable(L, name, fget, fset);
        }

        public void RegConstant(string name, double d)
        {
            LuaDLL.tolua_constant(L, name, d);
        }

        int GetFuncRef(string name)
        {
            if (PushLuaFunction(name, false))
            {
                return LuaDLL.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
            }

            throw new LuaException("get lua function reference failed: " + name);                         
        }

        public static LuaState Get(IntPtr ptr)
        {
#if !MULTI_STATE
            return mainState;
#else

            if (map.Count <= 1)
            {
                return mainState;
            }

            LuaState state = null;

            if (map.TryGetValue(ptr, out state))
            {
                return state;
            }
            else
            {                
                return Get(LuaDLL.tolua_getmainstate(ptr));
            }
#endif
        }

        public static ObjectTranslator GetTranslator(IntPtr ptr)
        {
#if !MULTI_STATE
            return mainState.translator;
#else

            if (map.Count <= 1)
            {
                return mainState.translator;
            }

            return Get(ptr).translator;
#endif
        }

        public object[] DoString(string chunk, string chunkName = "LuaState.DoString")
        {
            byte[] buffer = Encoding.UTF8.GetBytes(chunk);
            return LuaLoadBuffer(buffer, chunkName);
        }        

        public object[] DoFile(string fileName)
        {
            if (!Path.HasExtension(fileName))
            {
                fileName += ".lua";
            }

            byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

            if (buffer == null)
            {
                throw new LuaException(string.Format("Load lua file failed: {0}", fileName));
            }

            return LuaLoadBuffer(buffer, fileName);
        }

        //注意fileName与lua文件中require一致。 不要加扩展名
        public void Require(string fileName)
        {
            int top = LuaDLL.lua_gettop(L);
            string error = null;

            if (LuaRequire(fileName) != 0)
            {
                error = LuaDLL.lua_tostring(L, -1);
            }

            LuaDLL.lua_settop(L, top);

            if (error != null)
            {
                throw new LuaException(error);
            }
        }

        public void AddSearchPath(string fullPath)
        {
            if (!Path.IsPathRooted(fullPath))
            {
                throw new LuaException(fullPath + " is not a full path");
            }

            fullPath.Replace('\\', '/');

            if (LuaFileUtils.Instance.AddSearchPath(fullPath))
            {
                LuaDLL.lua_getglobal(L, "package");
                LuaDLL.lua_getfield(L, -1, "path");
                string current = LuaDLL.lua_tostring(L, -1);
                string str = string.Format("{0};{1}/?.lua", current, fullPath);
                LuaDLL.lua_pushstring(L, str);
                LuaDLL.lua_setfield(L, -3, "path");
                LuaDLL.lua_pop(L, 2);
            }
        }

        public void RemoveSeachPath(string fullPath)
        {
            if (!Path.IsPathRooted(fullPath))
            {
                throw new LuaException(fullPath + " is not a full path");
            }

            fullPath.Replace('\\', '/');
            LuaFileUtils.Instance.RemoveSearchPath(fullPath);
            LuaDLL.lua_getglobal(L, "package");
            LuaDLL.lua_getfield(L, -1, "path");
            string current = LuaDLL.lua_tostring(L, -1);
            current = current.Replace(fullPath + "/?.lua", "");            
            LuaDLL.lua_pushstring(L, current);
            LuaDLL.lua_setfield(L, -3, "path");
            LuaDLL.lua_pop(L, 2);
        }        

        public int BeginPCall(int reference)
        {                        
            return LuaDLL.tolua_beginpcall(L, reference);
        }

        public void PCall(int args, int oldTop)
        {            
            if (LuaDLL.lua_pcall(L, args, LuaDLL.LUA_MULTRET, oldTop) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);

                Exception last = LuaException.luaStack;
                LuaException.luaStack = null;
                throw new LuaException(error, last);
            }            
        }

        public void EndPCall(int oldTop)
        {
            LuaDLL.lua_settop(L, oldTop - 1);            
        }

        public void PushArgs(object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Push(args[i]);
            }
        }

        //压入一个存在的或不存在的table, 但不增加引用计数
        bool PushLuaTable(string fullPath, bool checkMap = true)
        {
            if (checkMap)
            {
                WeakReference weak = null;

                if (funcMap.TryGetValue(fullPath, out weak))
                {
                    if (weak.IsAlive)
                    {
                        LuaTable table = weak.Target as LuaTable;
                        Push(table);
                        return true;
                    }
                    else
                    {
                        funcMap.Remove(fullPath);
                    }
                }
            }

            if (!LuaDLL.tolua_pushluatable(L, fullPath))
            {                
                return false;
            }

            return true;
        }

        bool PushLuaFunction(string fullPath, bool checkMap = true)
        {
            if (checkMap)
            {
                WeakReference weak = null;

                if (funcMap.TryGetValue(fullPath, out weak))
                {
                    if (weak.IsAlive)
                    {
                        LuaFunction func = weak.Target as LuaFunction;

                        if (func.IsAlive())
                        {
                            func.AddRef();
                            return true;
                        }
                    }

                    funcMap.Remove(fullPath);
                }
            }

            int oldTop = LuaDLL.lua_gettop(L);
            int pos = fullPath.LastIndexOf('.');

            if (pos > 0)
            {
                string tableName = fullPath.Substring(0, pos);

                if (PushLuaTable(tableName))
                {
                    string funcName = fullPath.Substring(pos + 1);
                    LuaDLL.lua_pushstring(L, funcName);
                    LuaDLL.lua_rawget(L, -2);
                }

                LuaTypes type = LuaDLL.lua_type(L, -1);

                if (type != LuaTypes.LUA_TFUNCTION)
                {
                    LuaDLL.lua_settop(L, oldTop);
                    return false;
                }

                LuaDLL.lua_insert(L, oldTop + 1);
                LuaDLL.lua_settop(L, oldTop + 1);                
            }
            else
            {
                LuaDLL.lua_getglobal(L, fullPath);                
                LuaTypes type = LuaDLL.lua_type(L, -1);

                if (type != LuaTypes.LUA_TFUNCTION)
                {
                    LuaDLL.lua_settop(L, oldTop);                    
                    return false;
                }                
            }

            return true;
        }

        void RemoveFromGCList(int reference)
        {            
            lock (gcList)
            {
                int index = gcList.FindIndex((gc) => { return gc.reference == reference; });

                if (index >= 0)
                {
                    gcList.RemoveAt(index);
                }
            }
        }

        public LuaFunction GetFunction(string name, bool beLogMiss = true)
        {
            WeakReference weak = null;

            if (funcMap.TryGetValue(name, out weak))
            {
                if (weak.IsAlive)
                {
                    LuaFunction func = weak.Target as LuaFunction;

                    if (func.IsAlive())
                    {
                        func.AddRef();
                        RemoveFromGCList(func.GetReference());
                        return func;
                    }
                }

                funcMap.Remove(name);
            }

            if (PushLuaFunction(name, false))
            {
                int reference = LuaDLL.toluaL_ref(L);                

                if (funcRefMap.TryGetValue(reference, out weak))
                {
                    if (weak.IsAlive)
                    {
                        LuaFunction func = weak.Target as LuaFunction;

                        if (func.IsAlive())
                        {
                            funcMap.Add(name, weak);
                            func.AddRef();
                            RemoveFromGCList(reference);
                            return func;
                        }
                    }

                    funcRefMap.Remove(reference);
                }
                
                LuaFunction fun = new LuaFunction(reference, this);
                fun.name = name;
                funcMap.Add(name, new WeakReference(fun));
                funcRefMap.Add(reference, new WeakReference(fun));
                RemoveFromGCList(reference);
                if (LogGC) Debugger.Log("Alloc LuaFunction name {0}, id {1}", name, reference);                
                return fun;
            }

            if (beLogMiss)
            {
                Debugger.Log("Lua function {0} not exists", name);                
            }

            return null;
        }

        LuaBaseRef TryGetLuaRef(int reference)
        {            
            WeakReference weak = null;

            if (funcRefMap.TryGetValue(reference, out weak))
            {
                if (weak.IsAlive)
                {
                    LuaBaseRef luaRef = (LuaBaseRef)weak.Target;

                    if (luaRef.IsAlive())
                    {
                        luaRef.AddRef();
                        return luaRef;
                    }
                }                

                funcRefMap.Remove(reference);
            }

            return null;
        }

        public LuaFunction GetFunction(int reference)
        {
            LuaFunction func = TryGetLuaRef(reference) as LuaFunction;

            if (func == null)
            {                
                func = new LuaFunction(reference, this);
                funcRefMap.Add(reference, new WeakReference(func));
                if (LogGC) Debugger.Log("Alloc LuaFunction name , id {0}", reference);      
            }

            RemoveFromGCList(reference);
            return func;
        }

        public LuaTable GetTable(string fullPath, bool beLogMiss = true)
        {
            WeakReference weak = null;

            if (funcMap.TryGetValue(fullPath, out weak))
            {
                if (weak.IsAlive)
                {
                    LuaTable table = weak.Target as LuaTable;

                    if (table.IsAlive())
                    {
                        table.AddRef();
                        RemoveFromGCList(table.GetReference());
                        return table;
                    }
                }

                funcMap.Remove(fullPath);
            }

            if (PushLuaTable(fullPath, false))
            {
                int reference = LuaDLL.toluaL_ref(L);                
                LuaTable table = null;

                if (funcRefMap.TryGetValue(reference, out weak))
                {
                    if (weak.IsAlive)
                    {
                        table = weak.Target as LuaTable;

                        if (table.IsAlive())
                        {
                            funcMap.Add(fullPath, weak);
                            table.AddRef();
                            RemoveFromGCList(reference);
                            return table;
                        }
                    }

                    funcRefMap.Remove(reference);
                }

                table = new LuaTable(reference, this);
                table.name = fullPath;
                funcMap.Add(fullPath, new WeakReference(table));
                funcRefMap.Add(reference, new WeakReference(table));
                if (LogGC) Debugger.Log("Alloc LuaTable name {0}, id {1}", fullPath, reference);     
                RemoveFromGCList(reference);
                return table;
            }

            if (beLogMiss)
            {
                Debugger.LogWarning("Lua table {0} not exists", fullPath);
            }

            return null;
        }

        public LuaTable GetTable(int reference)
        {
            LuaTable table = TryGetLuaRef(reference) as LuaTable;

            if (table == null)
            {                
                table = new LuaTable(reference, this);
                funcRefMap.Add(reference, new WeakReference(table));
            }

            RemoveFromGCList(reference);
            return table;
        }

        public LuaThread GetLuaThread(int reference)
        {
            LuaThread thread = TryGetLuaRef(reference) as LuaThread;

            if (thread == null)
            {                
                thread = new LuaThread(reference, this);
                funcRefMap.Add(reference, new WeakReference(thread));
            }

            RemoveFromGCList(reference);
            return thread;
        }

        public bool CheckTop()
        {
            int n = LuaDLL.lua_gettop(L);

            if (n != 0)
            {
                Debugger.LogWarning("Lua stack top is {0}", n);
                return false;
            }

            return true;
        }

        public void Push(bool b)
        {
            LuaDLL.lua_pushboolean(L, b);
        }

        public void Push(double d)
        {
            LuaDLL.lua_pushnumber(L, d);
        }

        public void PushInt64(LuaInteger64 n)
        {
            LuaDLL.tolua_pushint64(L, n);
        }

        public void Push(string str)
        {
            LuaDLL.lua_pushstring(L, str);
        }

        public void Push(IntPtr p)
        {
            LuaDLL.lua_pushlightuserdata(L, p);
        }

        public void Push(Vector3 v3)
        {            
            LuaDLL.tolua_pushvec3(L, v3.x, v3.y, v3.z);
        }

        public void Push(Vector2 v2)
        {
            LuaDLL.tolua_pushvec2(L, v2.x, v2.y);
        }

        public void Push(Vector4 v4)
        {
            LuaDLL.tolua_pushvec4(L, v4.x, v4.y, v4.z, v4.w);
        }

        public void Push(Color clr)
        {
            LuaDLL.tolua_pushclr(L, clr.r, clr.g, clr.b, clr.a);
        }

        public void Push(Quaternion q)
        {
            LuaDLL.tolua_pushquat(L, q.x, q.y, q.z, q.w);
        }

        //void RemoveTrace(int pos, TracePCall trace)
        //{
        //    if (trace == TracePCall.Trace)
        //    {
        //        LuaDLL.lua_remove(L, pos);
        //    }
        //}         
       
        void _Call(int nArgs, int top)
        {
            try
            {
                PCall(nArgs, top);
                LuaDLL.lua_remove(L, top);
            }
            catch (Exception e)
            {
                EndPCall(top);
                throw e;
            }            
        }

        public void Push(Ray ray)
        {                        
            int oldTop = BeginPCall(PackRay);            
            Push(ray.direction);            
            Push(ray.origin);
            _Call(2, oldTop);            
            
        }

        public void Push(Bounds bound)
        {            
            int oldTop = BeginPCall(PackBounds);
            Push(bound.center);
            Push(bound.size);
            _Call(2, oldTop);            
        }

        public void Push(RaycastHit hit)
        {            
            int oldTop = BeginPCall(PackRaycastHit);
            Push(hit.collider);
            LuaDLL.lua_pushnumber(L, hit.distance);
            Push(hit.normal);
            Push(hit.point);
            Push(hit.rigidbody);
            Push(hit.transform);
            _Call(6, oldTop);            
        }

        public void Push(Touch touch)
        {            
            int oldTop = BeginPCall(PackTouch);
            LuaDLL.lua_pushinteger(L, touch.fingerId);
            Push(touch.position);
            Push(touch.rawPosition);
            Push(touch.deltaPosition);
            LuaDLL.lua_pushnumber(L, touch.deltaTime);
            LuaDLL.lua_pushinteger(L, touch.tapCount);
            LuaDLL.lua_pushinteger(L, (int)touch.phase);
            _Call(7, oldTop);            
        }

        public void PushLayerMask(LayerMask mask)
        {
            LuaDLL.tolua_pushlayermask(L, mask.value);
        }

        public void Push(LuaByteBuffer bb)
        {
            LuaDLL.lua_pushlstring(L, bb.buffer, bb.buffer.Length);
        }

        public void PushByteBuffer(byte[] buffer)
        {
            LuaDLL.lua_pushlstring(L, buffer, buffer.Length);
        }

        public void Push(Array array)
        {
            if (array == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserData(array, ArrayMetatable);
            }
        }

        public void Push(LuaBaseRef lbr)
        {
            if (lbr == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                LuaDLL.lua_getref(L, lbr.GetReference());
            }
        }

        public void Push(LuaThread thread)
        {
            if (thread == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                LuaDLL.lua_getref(L, thread.GetReference());
            }
        }

        public void Push(Type t)
        {
            PushUserData(t, TypeMetatable);         
        }

        public void Push(Delegate ev)
        {
            if (ev == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserData(ev, DelegateMetatable);
            }
        }

        public object GetEnumObj(Enum e)
        {
            object o = null;

            if (!enumMap.TryGetValue(e, out o))
            {
                o = e;
                enumMap.Add(e, o);
            }

            return o;
        }

        public void Push(Enum e)
        {
            object o = GetEnumObj(e);
            PushUserData(o, EnumMetatable);
        }

        public void Push(IEnumerator iter)
        {
            if (iter == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                //int reference = GetMetaReference(iter.GetType());

                //if (reference > 0)
                //{
                //    PushUserData(iter, reference);
                //}
                //else
                //{
                    PushUserData(iter, IterMetatable);
                //}
            }
        }

        public void Push(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            int reference;

            if (metaMap.TryGetValue(obj.GetType(), out reference))
            {
                PushUserData(obj, reference);
            }
            else
            {
                //类型未注册
                LuaDLL.lua_pushnil(L);
                Debugger.LogError("Type {0} not register to lua", obj.GetType());          
            }
        }

        public void Push(UnityEngine.TrackedReference tracker)
        {
            if (tracker == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            int reference;

            if (metaMap.TryGetValue(tracker.GetType(), out reference))
            {
                PushUserData(tracker, reference);
            }
            else
            {
                //类型未注册
                LuaDLL.lua_pushnil(L);
                Debugger.LogError("Type {0} not register to lua", tracker.GetType());
            }
        }

        public void PushValue(ValueType vt)
        {
            if (vt == null)
            {
                LuaDLL.lua_pushnil(L);
            }

            int reference;

            if (metaMap.TryGetValue(vt.GetType(), out reference))
            {
                int index = translator.AddObject(vt);
                LuaDLL.tolua_pushnewudata(L, reference, index);
            }
            else
            {
                //类型未注册
                LuaDLL.lua_pushnil(L);                
                Debugger.LogError("Type {0} not register to lua", vt.GetType());
            }
        }

        static Type monoType = typeof(Type).GetType();

        public void Push(object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            Type t = obj.GetType();

            if (t.IsValueType)
            {
                if (t == typeof(bool))
                {
                    bool b = (bool)obj;
                    LuaDLL.lua_pushboolean(L, b);
                }
                else if (t.IsEnum)
                {
                    Push((System.Enum)obj);
                }
                else if (t.IsPrimitive)
                {
                    double d = Convert.ToDouble(obj);
                    LuaDLL.lua_pushnumber(L, d);
                }
                else if (t == typeof(Vector3))
                {
                    Push((Vector3)obj);
                }
                else if (t == typeof(Vector2))
                {
                    Push((Vector2)obj);
                }
                else if (t == typeof(Vector4))
                {
                    Push((Vector4)obj);
                }
                else if (t == typeof(Quaternion))
                {
                    Push((Quaternion)obj);
                }
                else if (t == typeof(Color))
                {
                    Push((Color)obj);
                }
                else if (t == typeof(RaycastHit))
                {
                    Push((RaycastHit)obj);
                }
                else if (t == typeof(Touch))
                {
                    Push((Touch)obj);
                }
                else if (t == typeof(Ray))
                {
                    Push((Ray)obj);
                }
                else if (t == typeof(LayerMask))
                {
                    Push((LayerMask)obj);
                }
                else
                {                    
                    PushValue((ValueType)obj);                    
                }
            }
            else
            {
                if (t.IsArray)
                {
                    Push((Array)obj);
                }
                else if (t == typeof(string))
                {
                    LuaDLL.lua_pushstring(L, (string)obj);
                }
                else if (t.IsSubclassOf(typeof(LuaBaseRef)))
                {
                    Push((LuaBaseRef)obj);
                }
                else if (obj is UnityEngine.Object)
                {
                    Push((UnityEngine.Object)obj);
                }
                else if (obj is UnityEngine.TrackedReference)
                {
                    Push((UnityEngine.TrackedReference)obj);
                }
                else if (t.IsSubclassOf(typeof(Delegate)))
                {
                    Push((Delegate)obj);
                }
                else if (t == typeof(LuaByteBuffer))
                {
                    LuaByteBuffer lbb = (LuaByteBuffer)obj;
                    LuaDLL.lua_pushlstring(L, lbb.buffer, lbb.buffer.Length);
                }
                else if (t == monoType)
                {
                    Push((Type)obj);
                }
                else
                {
                    int reference;

                    if (metaMap.TryGetValue(obj.GetType(), out reference))
                    {                        
                        PushUserData(obj, reference);
                    }                                   
                }
            }
        }

        void PushUserData(object o, int reference)
        {
            int index;

            if (translator.Getudata(o, out index))
            {
                if (LuaDLL.tolua_pushudata(L, index))
                {
                    return;
                }                
            }

            index = translator.AddObject(o);
            LuaDLL.tolua_pushnewudata(L, reference, index);
        }

        public void PushObject(object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            int reference;

            if (metaMap.TryGetValue(obj.GetType(), out reference))
            {
                PushUserData(obj, reference);
            }
            else
            {
                //类型未注册
                LuaDLL.lua_pushnil(L);                
                Debugger.LogError("Type {0} not register to lua", obj.GetType());                
            }
        }

        object CheckObjectByType(int stackPos, Type type)
        {            
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    if (obj.GetType() == type)
                    {
                        return obj;
                    }

                    LuaDLL.luaL_typerror(L, stackPos, type.FullName, obj.GetType().FullName);
                }

                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        public double CheckNumber(int stackPos)
        {            
            return LuaDLL.luaL_checknumber(L, stackPos);
        }

        public bool CheckBoolean(int stackPos)
        {            
            return LuaDLL.luaL_checkboolean(L, stackPos);
        }

        Vector3 ToVector3(int stackPos)
        {
            float x, y, z;
            LuaDLL.tolua_getvec3(L, stackPos, out x, out y, out z);
            return new Vector3(x, y, z);
        }

        public Vector3 CheckVector3(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector3)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector3", type.ToString());
                return Vector3.zero;
            }
            
            float x, y, z;
            LuaDLL.tolua_getvec3(L, stackPos, out x, out y, out z);
            return new Vector3(x, y, z);
        }

        public Quaternion CheckQuaternion(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector4)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Quaternion", type.ToString());
                return Quaternion.identity;
            }

            float x, y, z, w;
            LuaDLL.tolua_getquat(L, stackPos, out x, out y, out z, out w);
            return new Quaternion(x, y, z, w);
        }

        public Vector2 CheckVector2(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector2)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector2", type.ToString());                
                return Vector2.zero;
            }

            float x, y;
            LuaDLL.tolua_getvec2(L, stackPos, out x, out y);
            return new Vector2(x, y);
        }

        public Vector4 CheckVector4(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Vector4)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Vector4", type.ToString());                    
                return Vector4.zero;
            }

            float x, y, z, w;
            LuaDLL.tolua_getvec4(L, stackPos, out x, out y, out z, out w);
            return new Vector4(x, y, z, w);
        }

        public Color CheckColor(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Color)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Color", type.ToString());    
                return Color.black;
            }

            float r, g, b, a;
            LuaDLL.tolua_getclr(L, stackPos, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        public Ray CheckRay(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Ray)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Ray", type.ToString());
                return new Ray();
            }
            
            int oldTop = BeginPCall(UnpackRay);
            LuaDLL.lua_pushvalue(L, stackPos);

            try
            {
                PCall(1, oldTop);
                Vector3 origin = ToVector3(oldTop + 1);
                Vector3 dir = ToVector3(oldTop + 2);
                EndPCall(oldTop);                
                return new Ray(origin, dir);
            }
            catch(Exception e)
            {
                EndPCall(oldTop);
                throw e;
            }
        }

        public Bounds CheckBounds(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.Bounds)
            {
                LuaDLL.luaL_typerror(L, stackPos, "Bounds", type.ToString());    
                return new Bounds();
            }
            
            int oldTop = BeginPCall(UnpackBounds);
            LuaDLL.lua_pushvalue(L, stackPos);

            try
            {
                PCall(1, oldTop);
                Vector3 center = ToVector3(oldTop + 1);
                Vector3 size = ToVector3(oldTop + 2);
                EndPCall(oldTop);
                return new Bounds(center, size);
            }
            catch(Exception e)
            {
                EndPCall(oldTop);
                throw e;
            }
        }

        public LayerMask CheckLayerMask(int stackPos)
        {            
            LuaValueType type = LuaDLL.tolua_getvaluetype(L, stackPos);

            if (type != LuaValueType.LayerMask)
            {
                LuaDLL.luaL_typerror(L, stackPos, "LayerMask", type.ToString());
                return 0;
            }
            
            return LuaDLL.tolua_getlayermask(L, stackPos);
        }

        public LuaInteger64 CheckInteger64(int stackPos)
        {            
            if (!LuaDLL.tolua_isint64(L, stackPos))
            {
                LuaDLL.luaL_typerror(L, stackPos, "int64");
                return 0;
            }
            
            return LuaDLL.tolua_toint64(L, stackPos);            
        }        

        public string CheckString(int stackPos)
        {            
            if (LuaDLL.lua_isstring(L, stackPos) == 1)
            {
                return LuaDLL.lua_tostring(L, stackPos);
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) == 1)
            {
                return (string)CheckObjectByType(stackPos, typeof(string));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string");
            return null;
        }

        public Delegate CheckDelegate(int stackPos)
        {            
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    Type type = obj.GetType();

                    if (type.IsSubclassOf(typeof(System.MulticastDelegate)))
                    {
                        return (Delegate)obj;
                    }

                    LuaDLL.luaL_typerror(L, stackPos, "Delegate", type.FullName);
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "Delegate");
            return null;
        }

        public char[] CheckCharBuffer(int stackPos)
        {            
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {
                int len;
                IntPtr source = LuaDLL.lua_tolstring(L, stackPos, out len);
                char[] buffer = new char[len];
                Marshal.Copy(source, buffer, 0, len);
                return buffer;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (char[])CheckObjectByType(stackPos, typeof(char[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or char[]");            
            return null;
        }

        public byte[] CheckByteBuffer(int stackPos)
        {            
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {
                int len;
                IntPtr source = LuaDLL.lua_tolstring(L, stackPos, out len);
                byte[] buffer = new byte[len];
                Marshal.Copy(source, buffer, 0, len);
                return buffer;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (byte[])CheckObjectByType(stackPos, typeof(byte[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or byte[]");     
            return null;
        }

        public T[] CheckNumberArray<T>(int stackPos)
        {            
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                T ret = default(T);
                List<T> list = new List<T>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (luatype != LuaTypes.LUA_TNUMBER)
                    {
                        break;
                    }

                    ret = (T)Convert.ChangeType(LuaDLL.lua_tonumber(L, -1), typeof(T));
                    list.Add(ret);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (T[])CheckObjectByType(stackPos, typeof(T[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);     
            return null;
        }

        string GetTypeError(int stackPos, string t1, string t2 = null)
        {
            if (t2 == null)
            {
                t2 = LuaDLL.luaL_typename(L, stackPos);
            }

            return string.Format("bad argument #{0}({1} expected, got {2})", stackPos - 1, t1, t2);            
        }

        public object CheckObject(int stackPos, Type type)
        {            
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                object obj = translator.GetObject(udata);
                Type objType = obj.GetType();

                if (type == objType || type.IsAssignableFrom(objType))
                {
                    return obj;
                }

                LuaDLL.luaL_typerror(L, stackPos, type.FullName, objType.FullName);
                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos) && !TypeChecker.IsValueType(type))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;            
        }

        public object[] CheckObjects(int oldTop)
        {
            int newTop = LuaDLL.lua_gettop(L);

            if (oldTop == newTop)
            {
                return null;
            }
            else
            {
                List<object> returnValues = new List<object>();

                for (int i = oldTop + 1; i <= newTop; i++)
                {
                    returnValues.Add(ToVariant(i));
                }

                return returnValues.ToArray();
            }
        }

        public LuaFunction CheckLuaFunction(int stackPos)
        {            
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TFUNCTION)
            {
                LuaDLL.luaL_typerror(L, stackPos, "function");
                return null;
            }

            return ToLuaFunction(stackPos);
        }

        public LuaTable CheckLuaTable(int stackPos)
        {            
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTABLE)
            {
                LuaDLL.luaL_typerror(L, stackPos, "table");
                return null;
            }

            return ToLuaTable(stackPos);
        }

        public LuaThread CheckLuaThread(int stackPos)
        {            
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTHREAD)
            {
                LuaDLL.luaL_typerror(L, stackPos, "thread");
                return null;
            }
            
            return ToLuaThread(stackPos);
        }

        object ToObject(int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                return translator.GetObject(udata);
            }

            return null;
        }

        LuaFunction ToLuaFunction(int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);            
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return GetFunction(reference);
        }

        LuaTable ToLuaTable(int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return GetTable(reference);
        }

        LuaThread ToLuaThread(int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return GetLuaThread(reference);
        }

        public object ToVariant(int stackPos)
        {
            return ToLua.ToVarObject(L, stackPos);
        }    

        public void CollectRef(int reference, string name, bool isGCThread = false)
        {
            if (!isGCThread)
            {                
                Collect(reference, name, false);
            }
            else
            {
                lock (gcList)
                {
                    gcList.Add(new GCRef(reference, name));
                }
            }
        }

        public void DelayDispose(LuaFunction func)
        {
            subList.Add(func);
        }

        public int Collect()
        {
            int count = gcList.Count;

            if (count > 0)
            {
                lock (gcList)
                {
                    for (int i = 0; i < gcList.Count; i++)
                    {
                        int reference = gcList[i].reference;
                        string name = gcList[i].name;
                        Collect(reference, name, true);
                    }

                    gcList.Clear();
                    return count;
                }
            }

            for (int i = 0; i < subList.Count; i++)
            {
                subList[i].Dispose();
            }

            subList.Clear();
            translator.Collect();
            return 0;
        }

        public object this[string fullPath]
        {
            get
            {
                int oldTop = LuaDLL.lua_gettop(L);
                int pos = fullPath.LastIndexOf('.');
                object obj = null;

                if (pos > 0)
                {
                    string tableName = fullPath.Substring(0, pos);

                    if (PushLuaTable(tableName))
                    {
                        string name = fullPath.Substring(pos + 1);
                        LuaDLL.lua_pushstring(L, name);
                        LuaDLL.lua_rawget(L, -2);
                        obj = ToVariant(-1);
                    }    
                    else
                    {
                        LuaDLL.lua_settop(L, oldTop);
                        return null;
                    }
                }
                else
                {
                    LuaDLL.lua_getglobal(L, fullPath);
                    obj = ToVariant(-1);
                }

                LuaDLL.lua_settop(L, oldTop);
                return obj;
            }

            set
            {
                int oldTop = LuaDLL.lua_gettop(L);
                int pos = fullPath.LastIndexOf('.');

                if (pos > 0)
                {
                    string tableName = fullPath.Substring(0, pos);
                    IntPtr p = LuaDLL.luaL_findtable(L, LuaIndexes.LUA_GLOBALSINDEX, tableName);

                    if (p == IntPtr.Zero)
                    {
                        string name = fullPath.Substring(pos + 1);
                        LuaDLL.lua_pushstring(L, name);
                        Push(value);
                        LuaDLL.lua_settable(L, -3);
                    }
                    else
                    {
                        LuaDLL.lua_settop(L, oldTop);
                        int len = LuaDLL.tolua_strlen(p);
                        string str = LuaDLL.lua_ptrtostring(p, len);
                        throw new LuaException(string.Format("{0} not a Lua table", str));
                    }
                }
                else
                {
                    Push(value);
                    LuaDLL.lua_setglobal(L, fullPath);                    
                }

                LuaDLL.lua_settop(L, oldTop);
            }
        }

        //慎用
        public void ReLoad(string moduleFileName)
        {
            LuaDLL.lua_getglobal(L, "package");                  
            LuaDLL.lua_getfield(L, -1, "loaded");                
            LuaDLL.lua_pushstring(L, moduleFileName);
            LuaDLL.lua_gettable(L, -2);                          

            if (!LuaDLL.lua_isnil(L, -1))
            {
                LuaDLL.lua_pushstring(L, moduleFileName);        
                LuaDLL.lua_pushnil(L);                           
                LuaDLL.lua_settable(L, -4);                      
            }

            LuaDLL.lua_pop(L, 3);
            string require = string.Format("require '{0}'", moduleFileName);
            DoString(require, "ReLoad");
        }

        public int GetMetaReference(Type t)
        {
            int reference = 0;
            metaMap.TryGetValue(t, out reference);
            return reference;
        }

        /*--------------------------------对于LuaDLL函数的简单封装------------------------------------------*/
        #region SIMPLE_LUA_FUNCTION
        public int LuaGetTop()
        {
            return LuaDLL.lua_gettop(L);
        }

        public void LuaSetTop(int newTop)
        {
            LuaDLL.lua_settop(L, newTop);
        }

        public void LuaRawGet(int idx)
        {
            LuaDLL.lua_rawget(L, idx);
        }

        public void LuaRawSet(int idx)
        {
            LuaDLL.lua_rawset(L, idx);
        }

        public void LuaRawGetI(int tableIndex, int index)
        {
            LuaDLL.lua_rawgeti(L, tableIndex, index);                  
        }

        public void LuaRawSetI(int tableIndex, int index)
        {
            LuaDLL.lua_rawseti(L, tableIndex, index);
        }

        public void LuaRemove(int index)
        {
            LuaDLL.lua_remove(L, index);
        }

        public void LuaRawGlobal(string name)
        {
            LuaDLL.lua_pushstring(L, name);
            LuaDLL.lua_rawget(L, LuaIndexes.LUA_GLOBALSINDEX);
        }

        public void LuaSetGlobal(string name)
        {
            LuaDLL.lua_setglobal(L, name);
        }

        public LuaTypes LuaType(int stackPos)
        {
            return LuaDLL.lua_type(L, stackPos);
        }

        public IntPtr LuaToThread(int stackPos)
        {
            return LuaDLL.lua_tothread(L, stackPos);
        }

        public bool LuaNext(int index)
        {
            return LuaDLL.lua_next(L, index) != 0;
        }

        public void LuaPop(int amount)
        {
            LuaDLL.lua_pop(L, amount);
        }

        public int LuaObjLen(int index)
        {            
            return LuaDLL.tolua_objlen(L, index);                        
        }

        public bool LuaCheckStack(int args)
        {
            return LuaDLL.lua_checkstack(L, args) == 0 ? false : true;
        }

        public void LuaGetTable(int index)
        {
            LuaDLL.lua_gettable(L, index);
        }

        public void LuaSetTable(int index)
        {
            LuaDLL.lua_settable(L, index);
        }

        public void LuaGetField(int index, string key)
        {
            LuaDLL.lua_getfield(L, index, key);
        }

        public void LuaSetField(int index, string name)
        {
            LuaDLL.lua_setfield(L, index, name);  
        }

        public int LuaRequire(string fileName)
        {
#if UNITY_EDITOR
            string str = Path.GetExtension(fileName);

            if (!string.IsNullOrEmpty(str))
            {
                throw new LuaException("Require not need file extension: " + str);
            }
#endif                        
            return LuaDLL.tolua_require(L, fileName);
        }

        public string LuaToString(int index)
        {
            return LuaDLL.lua_tostring(L, index);
        }

        public int LuaToInteger(int index)
        {
            return LuaDLL.lua_tointeger(L, index);
        }

        public void LuaGetRef(int reference)
        {
            LuaDLL.lua_getref(L, reference);
        }

        public int ToLuaRef()
        {
            return LuaDLL.toluaL_ref(L);
        }

        public void LuaCreateTable(int narr = 0, int nec = 0)
        {
            LuaDLL.lua_createtable(L, narr, nec);
        }

        public int LuaGetMetaTable(int idx)
        {
            return LuaDLL.lua_getmetatable(L, idx);
        }

        public IntPtr LuaToPointer(int idx)
        {
            return LuaDLL.lua_topointer(L, idx);
        }

        public void ToLuaException(Exception e)
        {
            if (LuaException.InstantiateCount > 0)
            {
                LuaDLL.toluaL_exception(L, e);
            }
            else
            {
                throw e;
            }
        }

        public void LuaGC(LuaGCOptions what, int data = 0)
        {            
            LuaDLL.lua_gc(L, what, data);
        }
        #endregion
        /*--------------------------------------------------------------------------------------------------*/

        void CloseBaseRef()
        {
            LuaDLL.lua_unref(L, PackBounds);
            LuaDLL.lua_unref(L, UnpackBounds);
            LuaDLL.lua_unref(L, PackRay);
            LuaDLL.lua_unref(L, UnpackRay);
            LuaDLL.lua_unref(L, PackRaycastHit);
            LuaDLL.lua_unref(L, PackTouch);   
        }
        
        public void Dispose()
        {
            if (IntPtr.Zero != L)
            {
                Debugger.Log("LuaState quit");
                LuaDLL.lua_gc(L, LuaGCOptions.LUA_GCCOLLECT, 0);
                Collect();

                foreach (KeyValuePair<Type, int> kv in metaMap)
                {
                    LuaDLL.lua_unref(L, kv.Value);
                }

                List<LuaBaseRef> list = new List<LuaBaseRef>();

                foreach (KeyValuePair<int, WeakReference> kv in funcRefMap)
                {
                    if (kv.Value.IsAlive)
                    {
                        list.Add((LuaBaseRef)kv.Value.Target);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Dispose(true);
                }

                CloseBaseRef();
                funcRefMap.Clear();
                funcMap.Clear();
                metaMap.Clear();
                typeMap.Clear();
                enumMap.Clear();
                stateMap.Remove(L);
                LuaDLL.lua_close(L);
                translator.Dispose();
                translator = null;
                L = IntPtr.Zero;
            }

            
#if !MULTI_STATE                  
            mainState = null;
#endif                              
            LuaFileUtils.Instance.Dispose();
            System.GC.SuppressFinalize(this);            
        }

        public virtual void Dispose(bool dispose)
        {
        }

        public override int GetHashCode()
        {
            return L.GetHashCode();
        }

        public override bool Equals(object o)
        {
            if ((object)o == null) return false;
            LuaState state = o as LuaState;
            return state != null && state.L == L;
        }

        public static bool operator ==(LuaState a, LuaState b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            object l = (object)a;
            object r = b;

            if (l == null && r != null)
            {
                return b.L == IntPtr.Zero;
            }

            if (l != null && r == null)
            {
                return a.L == IntPtr.Zero;
            }

            return a.L == b.L;
        }

        public static bool operator !=(LuaState a, LuaState b)
        {
            return !(a == b);
        }

        public void PrintTable(string name)
        {
            LuaTable table = GetTable(name);
            LuaDictTable dict = table.ToDictTable();
            table.Dispose();            
            var iter2 = dict.GetEnumerator();

            while (iter2.MoveNext())
            {
                Debugger.Log("map item, k,v is {0}:{1}", iter2.Current.Key, iter2.Current.Value);
            }

            iter2.Dispose();
            dict.Dispose();
        }

        protected void Collect(int reference, string name, bool beThread)
        {
            if (beThread)
            {
                WeakReference weak = null;

                if (name != null)
                {
                    funcMap.TryGetValue(name, out weak);

                    if (weak != null && !weak.IsAlive)
                    {
                        funcMap.Remove(name);
                        weak = null;
                    }
                }
                
                funcRefMap.TryGetValue(reference, out weak);

                if (weak != null && !weak.IsAlive)
                {                    
                    LuaDLL.toluaL_unref(L, reference);
                    funcRefMap.Remove(reference);

                    if (LogGC)
                    {
                        Debugger.Log("collect lua reference name {0}, id {1} in thread", name, reference);
                    }
                }
            }
            else
            {
                if (name != null)
                {
                    WeakReference weak = null;
                    funcMap.TryGetValue(name, out weak);
                    
                    if (weak != null && weak.IsAlive)
                    {
                        LuaBaseRef lbr = (LuaBaseRef)weak.Target;

                        if (reference == lbr.GetReference())
                        {
                            funcMap.Remove(name);
                        }
                    }
                }

                LuaDLL.toluaL_unref(L, reference);
                funcRefMap.Remove(reference);

                if (LogGC)
                {
                    Debugger.Log("collect lua reference name {0}, id {1} in main", name, reference);
                }
            }
        }

        protected object[] LuaLoadBuffer(byte[] buffer, string chunkName)
        {            
            LuaDLL.tolua_pushtraceback(L);
            int oldTop = LuaDLL.lua_gettop(L);

            if (LuaDLL.luaL_loadbuffer(L, buffer, buffer.Length, chunkName) == 0)
            {
                if (LuaDLL.lua_pcall(L, 0, LuaDLL.LUA_MULTRET, oldTop) == 0)
                {
                    object[] result = CheckObjects(oldTop);
                    LuaDLL.lua_settop(L, oldTop - 1);
                    return result;
                }
            }

            string err = LuaDLL.lua_tostring(L, -1);
            LuaDLL.lua_settop(L, oldTop - 1);
            throw new LuaException(err);                        
        }
    }
}