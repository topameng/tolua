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
using UnityEngine;
using System.Collections.Generic;
using System;

public class ToLuaNode<T>
{
    public List<ToLuaNode<T>> childs = new List<ToLuaNode<T>>();
    public ToLuaNode<T> parent = null;
    public T value;
}

public class ToLuaTree<T> 
{       
    public ToLuaNode<T> _root = null;

    public ToLuaTree()
    {
        _root = new ToLuaNode<T>();
    }

    ToLuaNode<T> FindParent(List<ToLuaNode<T>> root, Predicate<T> match)
    {
        if (root == null)
        {
            return null;
        }

        for (int i = 0; i < root.Count; i++)
        {
            if (match(root[i].value))
            {
                return root[i];
            }

            ToLuaNode<T> node = FindParent(root[i].childs, match);

            if (node != null)
            {
                return node;
            }
        }

        return null;
    }

    /*public void BreadthFirstTraversal(Action<ToLuaNode<T>> action)
    {
        List<ToLuaNode<T>> root = _root.childs;        
        Queue<ToLuaNode<T>> queue = new Queue<ToLuaNode<T>>();

        for (int i = 0; i < root.Count; i++)
        {
            queue.Enqueue(root[i]);
        }

        while (queue.Count > 0)
        {
            ToLuaNode<T> node = queue.Dequeue();
            action(node);

            if (node.childs != null)
            {
                for (int i = 0; i < node.childs.Count; i++)
                {
                    queue.Enqueue(node.childs[i]);
                }
            }
        }
    }*/

    public void DepthFirstTraversal(Action<ToLuaNode<T>> begin, Action<ToLuaNode<T>> end, ToLuaNode<T> node)
    {
        begin(node);

        for (int i = 0; i < node.childs.Count; i++)
        {            
            DepthFirstTraversal(begin, end, node.childs[i]);
        }

        end(node);
    }

    public ToLuaNode<T> Find(Predicate<T> match)
    {
        return FindParent(_root.childs, match);
    }

    public ToLuaNode<T> GetRoot()
    {
        return _root;
    }
}
