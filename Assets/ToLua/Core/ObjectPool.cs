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
using System.Runtime.CompilerServices;

namespace LuaInterface
{
    public class LuaObjectPool
    {
        private class NodeBase
        {
            public int index;
            public bool bObjectType;
            public Type eleType;
            protected static List<NodeBase> headList = new List<NodeBase>();

            public NodeBase(int index)
            {
                this.index = index;
                bObjectType = true;
                eleType = null;
            }

            internal virtual object GetResult() { return null; }
            internal virtual int Remove(LuaObjectPool objPool, int pos, bool bLogGC) { return -1; }
            internal virtual void Destroy(LuaObjectPool objPool, int pos, bool bClean, bool bLogGC) { }

            public static void Clear()
            {
                for (int i = 0; i < headList.Count; ++i)
                {
                    headList[i].index = 0;
                }
            }
        }

        private class PoolNode<T> : NodeBase
        {
            public T obj;
            //同lua_ref策略，0作为一个回收链表头，不使用这个位置
            public static readonly NodeBase head;

            static PoolNode()
            {
                head = new NodeBase(0);
                headList.Add(head);
            }

            public PoolNode(int index, T obj)
            : base(index)
            {
                this.obj = obj;
                eleType = obj.GetType();
                bObjectType = !TypeChecker.IsValueType(eleType);
            }

            internal override object GetResult() { return (object)obj; }

            internal override int Remove(LuaObjectPool objPool, int pos, bool bLogGC)
            {
                RemoveMapping(objPool, pos);

                int result = head.index;
                head.index = pos;
                if (bLogGC) Debugger.Log("destroy object {0}, id {1}", obj, pos);
                obj = default(T);

                return result;
            }

            internal override void Destroy(LuaObjectPool objPool, int pos, bool bClean, bool bLogGC)
            {
                if (bClean)
                {
                    RemoveMapping(objPool, pos);
                    if (bLogGC) Debugger.Log("destroy object {0}, id {1}", obj, pos);
                }

                obj = default(T);
            }

            private void RemoveMapping(LuaObjectPool objPool, int pos)
            {
                if (bObjectType)
                {
                    if (obj != null) StackDataMapping<object>.InstanceGet(objPool).Remove(obj, pos);
                }
                else StackDataMapping<T>.InstanceGet(objPool).Remove(obj, pos);
            }
        }

        private NodeBase[] list;
        private int count = 0;
        private int collectStep = 2;
        private int collectedIndex = -1;
        private readonly Type objType = typeof(object);
        private Action disposeListener;

        public LuaObjectPool()
        {
            list = new NodeBase[1024];
            object temp = new object();
            list[0] = new PoolNode<object>(0, temp);
            list[1] = new PoolNode<object>(1, temp);
            count = 2;
        }

        public object this[int i]
        {
            get
            {
                if (i > 0 && i < count)
                {
                    return list[i].GetResult();
                }

                return null;
            }
        }

        public void Clear()
        {
            if (disposeListener != null)
            {
                disposeListener();
            }
            disposeListener = null;
            if (count > 0)
            {
                Array.Clear(list, 0, count);
                count = 0;
            }
            NodeBase.Clear();
        }

        public bool Add<T>(T obj, out int pos)
        {
            NodeBase node = null;
            pos = -1;

            var head = PoolNode<T>.head;
            if (head.index != 0)
            {
                pos = head.index;
                node = list[pos];
                var gNode = node as PoolNode<T>;
                gNode.obj = obj;
                gNode.eleType = obj.GetType();
                head.index = node.index;
            }
            else
            {
                if (count == list.Length) EnsureCapacity(count + 1);
                pos = count;
                node = new PoolNode<T>(pos, obj);
                list[pos] = node;
                count++;
            }

#if UNITY_EDITOR
            Type dataType = typeof(T);
            if (node.bObjectType && dataType != objType)
            {
                Debugger.LogError("strict object type required, got :" + node.eleType);
                return false;
            }
#endif
            StackDataMapping<T>.InstanceGet(this).Add(obj, pos);

            return true;
        }

        public T TryGetValue<T>(int index)
        {
            if (index > 1 && index < count)
            {
                var node = list[index];
                if (node.bObjectType)
                {
                    var objNode = node as PoolNode<object>;
                    return (T)objNode.obj;
                }

                var gNode = node as PoolNode<T>;
                if (gNode != null) return gNode.obj;
                else if (typeof(T) == objType) return (T)node.GetResult();
            }

            return default(T);
        }

        public bool GetDataExistRef<T>(T o, out int index)
        {
            index = -1;
#if UNITY_EDITOR
            Type dataType = typeof(T);
            Type dataOriginType = o.GetType();
            if (!TypeChecker.IsValueType(dataOriginType) && dataType != objType)
            {
                Debugger.LogError("strict object type required, got :" + dataOriginType);
                return false;
            }
#endif
            return StackDataMapping<T>.InstanceGet(this).TryGetValue(o, out index);
        }

        public Type GetNodeType(int i)
        {
            if (i > 1 && i < count)
            {
                return list[i].eleType;
            }

            return null;
        }

        public bool Remove(int pos, bool bLogGC)
        {
            if (pos > 1 && pos < count)
            {
                var node = list[pos];
                node.index = node.Remove(this, pos, bLogGC);
                return true;
            }

            return false;
        }

        public bool Destroy(int pos, bool bClean, bool bLogGC)
        {
            if (pos > 1 && pos < count)
            {
                var node = list[pos];
                node.Destroy(this, pos, bClean, bLogGC);
                return true;
            }

            return false;
        }

        // update valuetype etc
        public T Replace<T>(int pos, T o)
        {
            if (pos > 1 && pos < count)
            {
                var gNode = list[pos] as PoolNode<T>;
                var result = gNode.obj;
                gNode.obj = o;

                return result;
            }

            return default(T);
        }

        public void StepCollect()
        {
            ++collectedIndex;
            for (int i = 0; i < collectStep; ++i)
            {
                collectedIndex += i;
                if (collectedIndex >= count)
                {
                    collectedIndex = -1;
                    return;
                }

                var node = list[collectedIndex];
                if (!node.bObjectType) continue;

                var objNode = node as PoolNode<object>;
                object o = objNode.obj;
                if (o != null && o.Equals(null))
                {
                    objNode.obj = null;
                    StackDataMapping<object>.InstanceGet(this).Remove(o, collectedIndex);
                }
            }
        }

        public void AttachDisposeListenter(Action listener)
        {
            disposeListener += listener;
        }

        void EnsureCapacity(int min)
        {
            int newCapacity = list.Length << 1;
            // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
            // Note that this check works even when list.Length overflowed thanks to the (uint) cast
            if ((uint)newCapacity > 0X7FEFFFFF) newCapacity = 0X7FEFFFFF;
            if (newCapacity < min) newCapacity = min;

            NodeBase[] newList = new NodeBase[newCapacity];
            Array.ConstrainedCopy(list, 0, newList, 0, count);
            list = newList;
        }

        private class StackDataMapping<T>
        {
            private class CompareObject : IEqualityComparer<T>
            {
                public bool Equals(T x, T y)
                {
                    return System.Object.ReferenceEquals(x, y);
                }

                public int GetHashCode(T obj)
                {
                    return RuntimeHelpers.GetHashCode(obj);
                }
            }

            private Dictionary<T, int> container;
            private static readonly Type objType = typeof(object);
#if !MULTI_STATE
            private static StackDataMapping<T> _instance;
#else
            private static Dictionary<LuaObjectPool, StackDataMapping<T>> _instanceGroup = new Dictionary<LuaObjectPool, StackDataMapping<T>>();
#endif

            public static StackDataMapping<T> InstanceGet(LuaObjectPool luaObjectPool)
            {
#if !MULTI_STATE
                _instance = _instance ?? new StackDataMapping<T>(luaObjectPool);
                return _instance;
#else
                StackDataMapping<T> _instance = null;
                if (!_instanceGroup.TryGetValue(luaObjectPool, out _instance) || _instance == null)
                {
                    _instance = new StackDataMapping<T>(luaObjectPool);
                    _instanceGroup[luaObjectPool] = _instance;
                }

                return _instance;
#endif
            }

            private StackDataMapping(LuaObjectPool luaObjectPool)
            {
                if (typeof(T) == objType) container = new Dictionary<T, int>(2017, new CompareObject());
                else container = new Dictionary<T, int>(17);
                luaObjectPool.AttachDisposeListenter(Clear);
            }

            public void Add(T obj, int pos)
            {
                container[obj] = pos;
            }

            public bool TryGetValue(T obj, out int pos)
            {
                return container.TryGetValue(obj, out pos);
            }

            public void Remove(T o, int udata)
            {
                int pos = -1;
                if (container.TryGetValue(o, out pos) && pos == udata)
                {
                    container.Remove(o);
                }
            }

            private void Clear()
            {
                container.Clear();
            }
        }
    }
}