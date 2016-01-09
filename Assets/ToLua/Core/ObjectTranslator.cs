using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuaInterface
{
    public class ObjectTranslator
    {        
        private class DelayAction
        {
            public DelayAction(Action act, float time)
            {
                this.Call = act;
                this.time = time;
            }

            public Action Call = null;
            public float time;
        }

        private class CompareObject : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);                
            }

            public int GetHashCode(object obj)
            {
                if (obj != null)
                {
                    return obj.GetHashCode();
                }

                return 0;
            }
        }

        public bool LogGC { get; set; }
        public readonly Dictionary<object, int> objectsBackMap = new Dictionary<object, int>(new CompareObject());

        /*public readonly Dictionary<object, int> objectsBackMap = new Dictionary<object, int>();
        
        public readonly Dictionary<int, object> objects = new Dictionary<int, object>();
        int nextObj = 1;

        public int AddObject(object obj)
        {
            int index = nextObj++;
            objects[index] = obj;

            if (!CheckType.IsValueType(obj.GetType()))
            {
                objectsBackMap[obj] = index;
            }

            return index;
        }
         
        public void RemoveObject(int udata)
        {
            object o;
            bool found = objects.TryGetValue(udata, out o);

            if (found)
            {
                objects.Remove(udata);

                if (o != null && !CheckType.IsValueType(o.GetType()))
                {
                    objectsBackMap.Remove(o);
                }
            }
        }

        public bool Getudata(object o, out int index)
        {
            index = -1;
            return objectsBackMap.TryGetValue(o, out index);
        }

        public object GetObject(int udata)
        {
            object o = null;

            if (objects.TryGetValue(udata, out o))
            {
                return o;
            }

            return null;
        }

        public void Destroy(int udata)
        {
            object o = objects[udata];
            objects[udata] = null;

            if (o != null && !CheckType.IsValueType(o.GetType()))
            {
                objectsBackMap.Remove(o);                
            }
        }*/

        public readonly LuaObjectPool objects = new LuaObjectPool();
        private List<DelayAction> gcList = new List<DelayAction>();

        public ObjectTranslator()
        {
            LogGC = false;
        }

        public int AddObject(object obj)
        {
            int index = objects.Add(obj);

            if (!TypeChecker.IsValueType(obj.GetType()))
            {
                objectsBackMap[obj] = index;
            }

            return index;
        }

        public void RemoveObject(int udata)
        {
            object o = objects.Remove(udata);

            if (o != null && !TypeChecker.IsValueType(o.GetType()))
            {
                objectsBackMap.Remove(o);
            }            
        }

        //public void RemoveObject(object o)
        //{
        //    if (o != null && !TypeChecker.IsValueType(o.GetType()))
        //    {
        //        int udata = 0;

        //        if (objectsBackMap.TryGetValue(o, out udata))
        //        {
        //            objects.Remove(udata);
        //            objectsBackMap.Remove(o);
        //        }
        //    }
        //}

        public object GetObject(int udata)
        {
            return objects.TryGetValue(udata);         
        }

        public void Destroy(int udata)
        {
            object o = objects.Destroy(udata);

            if (o != null && !TypeChecker.IsValueType(o.GetType()))
            {
                objectsBackMap.Remove(o);

                if (LogGC)
                {
                    Debugger.Log("collect object {0}, id {1}", o, udata);
                }
            }
        }

        public void DelayDestroy(Action act, float time)
        {
            gcList.Add(new DelayAction(act, time));
        }

        public bool Getudata(object o, out int index)
        {
            index = -1;
            return objectsBackMap.TryGetValue(o, out index);
        }

        public void SetBack(int index, object o)
        {
            object obj = objects.Replace(index, o);
            objectsBackMap.Remove(obj);
            objectsBackMap[o] = index;
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
                    gcList[i].Call();
                    gcList.RemoveAt(i);
                }
                else
                {
                    gcList[i].time = time;
                }
            }
        }

        public void Dispose()
        {
            objectsBackMap.Clear();
            objects.Clear();                        
        }
    }
}