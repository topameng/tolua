/*
Copyright (c) 2015-2021 topameng(topameng@qq.com)
https://github.com/topameng/tolua

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
using System.Collections.Generic;
using UnityEngine;

namespace LuaInterface
{
    public class ObjectTranslator
    {
        private class DelayGC
        {
            public DelayGC(int id, UnityEngine.Object obj, float time)
            {
                this.id = id;
                this.time = time;
                this.obj = obj;
            }

            public int id;
            public UnityEngine.Object obj;
            public float time;
        }

        public bool LogGC { get; set; }
        private readonly LuaObjectPool objects = new LuaObjectPool();
        private List<DelayGC> gcList = new List<DelayGC>();

#if !MULTI_STATE
        private static ObjectTranslator _translator = null;
#endif

        public ObjectTranslator()
        {
            LogGC = false;
#if !MULTI_STATE
            _translator = this;
#endif
        }

        public int AddObject<T>(T obj)
        {
            int pos = 0;
            objects.Add(obj, out pos);
            return pos;
        }

        public static ObjectTranslator Get(IntPtr L)
        {
#if !MULTI_STATE
            return _translator;
#else
            return LuaState.GetTranslator(L);
#endif
        }

        //lua gc一个对象(lua 库不再引用，但不代表c#没使用)
        public void RemoveObject(int udata)
        {
            //只有lua gc才能移除
            objects.Remove(udata, LogGC);
        }

        public T GetObject<T>(int udata)
        {
            return objects.TryGetValue<T>(udata);
        }

        public Type CheckOutNodeType(int udata)
        {
            return objects.GetNodeType(udata);
        }

        //预删除，但不移除一个lua对象(移除id只能由gc完成)
        public void Destroy(int udata)
        {
            objects.Destroy(udata, true, LogGC);
        }

        //Unity Object 延迟删除
        public void DelayDestroy(int id, float time)
        {
            UnityEngine.Object obj = (UnityEngine.Object)GetObject<object>(id);

            if (obj != null)
            {
                gcList.Add(new DelayGC(id, obj, time));
            }
        }

        public bool Getudata<T>(T o, out int index)
        {
            return objects.GetDataExistRef(o, out index); 
        }

        public void Destroyudata(int udata)
        {
            objects.Destroy(udata, false, LogGC);
        }

        public void SetBack<T>(int index, T o) where T : struct
        {
            objects.Replace(index, o);
        }

        bool RemoveFromGCList(int id)
        {
            int index = gcList.FindIndex((p) => { return p.id == id; });

            if (index >= 0)
            {
                gcList.RemoveAt(index);
                return true;
            }

            return false;
        }

        //延迟删除处理
        void DestroyUnityObject(int udata, UnityEngine.Object obj)
        {
            object o = objects.TryGetValue<object>(udata);

            if (object.ReferenceEquals(o, obj))
            {
                //一定不能Remove, 因为GC还可能再来一次
                objects.Destroy(udata, true, LogGC);
            }

            UnityEngine.Object.Destroy(obj);
        }

        public void Collect()
        {
            if (gcList.Count == 0)
            {
                return;
            }

            float delta = Time.deltaTime;

            for (int i = gcList.Count - 1; i >= 0; i--)
            {
                float time = gcList[i].time - delta;

                if (time <= 0)
                {
                    DestroyUnityObject(gcList[i].id, gcList[i].obj);
                    gcList.RemoveAt(i);
                }
                else
                {
                    gcList[i].time = time;
                }
            }
        }

        public void StepCollect()
        {
            objects.StepCollect();
        }

        public void Dispose()
        {
            objects.Clear();

#if !MULTI_STATE
            _translator = null;
#endif
        }
    }
}