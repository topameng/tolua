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
//打开开关没有写入导出列表的纯虚类自动跳过
//#define JUMP_NODEFINED_ABSTRACT         

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;
using LuaInterface;

using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Debugger = LuaInterface.Debugger;
using System.Threading;
using System.Runtime.CompilerServices;

[InitializeOnLoad]
public static class ToLuaMenu
{
    //不需要导出或者无法导出的类型
    public static List<Type> dropType = new List<Type>
    {
        typeof(ValueType),                                  //不需要
#if UNITY_4_6 || UNITY_4_7
        typeof(Motion),                                     //很多平台只是空类
#endif

#if UNITY_5_3_OR_NEWER
        typeof(UnityEngine.CustomYieldInstruction),
#endif
        typeof(UnityEngine.YieldInstruction),               //无需导出的类      
        typeof(UnityEngine.WaitForEndOfFrame),              //内部支持
        typeof(UnityEngine.WaitForFixedUpdate),
        typeof(UnityEngine.WaitForSeconds),        
        typeof(UnityEngine.Mathf),                          //lua层支持                
        typeof(Plane),                                      
        typeof(LayerMask),                                  
        typeof(Vector3),
        typeof(Vector4),
        typeof(Vector2),
        typeof(Quaternion),
        typeof(Ray),
        typeof(Bounds),
        typeof(Color),                                    
        typeof(Touch),
        typeof(RaycastHit),                                 
        typeof(TouchPhase),     
        //typeof(LuaInterface.LuaOutMetatable),               //手写支持
        typeof(LuaInterface.NullObject),             
        typeof(System.Array),                        
        typeof(System.Reflection.MemberInfo),    
        typeof(System.Reflection.BindingFlags),
        typeof(LuaClient),
        typeof(LuaInterface.LuaFunction),
        typeof(LuaInterface.LuaTable),
        typeof(LuaInterface.LuaThread),
        typeof(LuaInterface.LuaByteBuffer),                 //只是类型标识符
        typeof(DelegateFactory),                            //无需导出，导出类支持lua函数转换为委托。如UIEventListener.OnClick(luafunc)
    };

    //可以导出的内部支持类型
    public static List<Type> baseType = new List<Type>
    {
        typeof(System.Object),
        typeof(System.Delegate),
        typeof(System.String),
        typeof(System.Enum),
        typeof(System.Type),
        typeof(System.Collections.IEnumerator),
        typeof(UnityEngine.Object),
        typeof(LuaInterface.EventObject),
        typeof(LuaInterface.LuaMethod),
        typeof(LuaInterface.LuaProperty),
        typeof(LuaInterface.LuaField),
        typeof(LuaInterface.LuaConstructor),        
    };

    private static bool beAutoGen = false;
    private static bool beCheck = true;        
    static List<BindType> allTypes = new List<BindType>();

    static ToLuaMenu()
    {
        string dir = CustomSettings.saveDir;
        string[] files = Directory.GetFiles(dir, "*.cs", SearchOption.TopDirectoryOnly);

        if (files.Length < 3 && beCheck)
        {
            if (EditorUtility.DisplayDialog("自动生成", "点击确定自动生成常用类型注册文件， 也可通过菜单逐步完成此功能", "确定", "取消"))
            {
                beAutoGen = true;
                GenLuaDelegates();
                AssetDatabase.Refresh();
                GenerateClassWraps();
                GenLuaBinder();
                beAutoGen = false;                
            }

            beCheck = false;
        }
    }

    static string RemoveNameSpace(string name, string space)
    {
        if (space != null)
        {
            name = name.Remove(0, space.Length + 1);
        }

        return name;
    }

    public class BindType
    {
        public string name;                 //类名称
        public Type type;
        public bool IsStatic;        
        public string wrapName = "";        //产生的wrap文件名字
        public string libName = "";         //注册到lua的名字
        public Type baseType = null;
        public string nameSpace = null;     //注册到lua的table层级

        public List<Type> extendList = new List<Type>();

        public BindType(Type t)
        {
            if (typeof(System.MulticastDelegate).IsAssignableFrom(t))
            {
                throw new NotSupportedException(string.Format("\nDon't export Delegate {0} as a class, register it in customDelegateList", LuaMisc.GetTypeName(t)));
            }            

            //if (IsObsolete(t))
            //{
            //    throw new Exception(string.Format("\n{0} is obsolete, don't export it!", LuaMisc.GetTypeName(t)));
            //}

            type = t;                        
            nameSpace = ToLuaExport.GetNameSpace(t, out libName);
            name = ToLuaExport.CombineTypeStr(nameSpace, libName);            
            libName = ToLuaExport.ConvertToLibSign(libName);

            if (name == "object")
            {
                wrapName = "System_Object";
                name = "System.Object";
            }
            else if (name == "string")
            {
                wrapName = "System_String";
                name = "System.String";
            }
            else
            {
                wrapName = name.Replace('.', '_');
                wrapName = ToLuaExport.ConvertToLibSign(wrapName);
            }

            int index = CustomSettings.staticClassTypes.IndexOf(type);

            if (index >= 0 || (type.IsAbstract && type.IsSealed))
            {
                IsStatic = true;                
            }

            baseType = LuaMisc.GetExportBaseType(type);
        }

        public BindType SetBaseType(Type t)
        {
            baseType = t;
            return this;
        }

        public BindType AddExtendType(Type t)
        {
            if (!extendList.Contains(t))
            {
                extendList.Add(t);
            }

            return this;
        }

        public BindType SetWrapName(string str)
        {
            wrapName = str;
            return this;
        }

        public BindType SetLibName(string str)
        {
            libName = str;
            return this;
        }

        public BindType SetNameSpace(string space)
        {
            nameSpace = space;            
            return this;
        }

        public static bool IsObsolete(Type type)
        {
            object[] attrs = type.GetCustomAttributes(true);

            for (int j = 0; j < attrs.Length; j++)
            {
                Type t = attrs[j].GetType();

                if (t == typeof(System.ObsoleteAttribute) || t == typeof(NoToLuaAttribute) || t == typeof(CompilerGeneratedAttribute) || t == typeof(UnsafeValueTypeAttribute)
                    || t.Name == "MonoNotSupportedAttribute" || t.Name == "MonoTODOAttribute")
                {
                    return true;
                }
            }

            return false;
        }
    }

    static void AutoAddBaseType(BindType bt, bool beDropBaseType)
    {
        Type t = bt.baseType;

        if (t == null)
        {
            return;
        }

        if (CustomSettings.sealedList.Contains(t))
        {
            CustomSettings.sealedList.Remove(t);
            Debugger.LogError("{0} not a sealed class, it is parent of {1}", LuaMisc.GetTypeName(t), bt.name);
        }

        if (t.IsInterface)
        {
            Debugger.LogWarning("{0} has a base type {1} is Interface, use SetBaseType to jump it", bt.name, t.FullName);
            bt.baseType = t.BaseType;
        }
        else if (dropType.IndexOf(t) >= 0)
        {
            Debugger.LogWarning("{0} has a base type {1} is a drop type", bt.name, t.FullName);
            bt.baseType = t.BaseType;
        }
        else if (!beDropBaseType || baseType.IndexOf(t) < 0)
        {
            int index = allTypes.FindIndex((iter) => { return iter.type == t; });

            if (index < 0)
            {
#if JUMP_NODEFINED_ABSTRACT
                if (t.IsAbstract && !t.IsSealed)
                {
                    Debugger.LogWarning("not defined bindtype for {0}, it is abstract class, jump it, child class is {1}", LuaMisc.GetTypeName(t), bt.name);
                    bt.baseType = t.BaseType;
                }
                else
                {
                    Debugger.LogWarning("not defined bindtype for {0}, autogen it, child class is {1}", LuaMisc.GetTypeName(t), bt.name);
                    bt = new BindType(t);
                    allTypes.Add(bt);
                }
#else
                Debugger.LogWarning("not defined bindtype for {0}, autogen it, child class is {1}", LuaMisc.GetTypeName(t), bt.name);                        
                bt = new BindType(t);
                allTypes.Add(bt);
#endif
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

        AutoAddBaseType(bt, beDropBaseType);
    }

    static BindType[] GenBindTypes(BindType[] list, bool beDropBaseType = true)
    {
        allTypes = new List<BindType>(list);

        for (int i = 0; i < list.Length; i++)
        {
            for (int j = i + 1; j < list.Length; j++)
            {
                if (list[i].type == list[j].type)
                    throw new NotSupportedException("Repeat BindType:" + list[i].type);
            }

            if (dropType.IndexOf(list[i].type) >= 0)
            {
                Debug.LogWarning(list[i].type.FullName + " in dropType table, not need to export");
                allTypes.Remove(list[i]);
                continue;
            }
            else if (beDropBaseType && baseType.IndexOf(list[i].type) >= 0)
            {
                Debug.LogWarning(list[i].type.FullName + " is Base Type, not need to export");
                allTypes.Remove(list[i]);
                continue;
            }
            else if (list[i].type.IsEnum)
            {
                continue;
            }

            AutoAddBaseType(list[i], beDropBaseType);
        }

        return allTypes.ToArray();
    }

    [MenuItem("Lua/Gen Lua Wrap Files", false, 1)]
    public static void GenerateClassWraps()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        if (!Directory.Exists(CustomSettings.saveDir))
        {
            Directory.CreateDirectory(CustomSettings.saveDir);
        }

        allTypes.Clear();
        BindType[] typeList = CustomSettings.customTypeList;

        BindType[] list = GenBindTypes(typeList);
        ToLuaExport.allTypes.AddRange(baseType);

        for (int i = 0; i < list.Length; i++)
        {            
            ToLuaExport.allTypes.Add(list[i].type);
        }

        for (int i = 0; i < list.Length; i++)
        {
            ToLuaExport.Clear();
            ToLuaExport.className = list[i].name;
            ToLuaExport.type = list[i].type;
            ToLuaExport.isStaticClass = list[i].IsStatic;            
            ToLuaExport.baseType = list[i].baseType;
            ToLuaExport.wrapClassName = list[i].wrapName;
            ToLuaExport.libClassName = list[i].libName;
            ToLuaExport.extendList = list[i].extendList;
            ToLuaExport.Generate(CustomSettings.saveDir);
        }

        Debug.Log("Generate lua binding files over");
        ToLuaExport.allTypes.Clear();
        allTypes.Clear();        
        AssetDatabase.Refresh();
    }

    static bool IsDelegateInClass(Type delegateType, Type classType)
    {
        string funcName;
        string space = ToLuaExport.GetNameSpace(delegateType, out funcName);

        if (space == classType.FullName)
        {
            return true;
        }

        BindType bt = Array.Find(CustomSettings.customTypeList, (iter) => { return iter.type.FullName == space; });

        if (bt != null)
        {
            return true;
        }

        return false;
    }

    static HashSet<Type> GetCustomTypeDelegates(bool force = false)
    {
        BindType[] list = CustomSettings.customTypeList;
        HashSet<Type> set = new HashSet<Type>();
        BindingFlags binding = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.Instance;

        for (int i = 0; i < list.Length; i++)
        {
            Type type = list[i].type;
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.SetField | binding);
            PropertyInfo[] props = type.GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | binding);
            MethodInfo[] methods = null;

            if (type.IsInterface)
            {
                methods = type.GetMethods();
            }
            else
            {
                methods = type.GetMethods(BindingFlags.Instance | binding);
            }

            for (int j = 0; j < fields.Length; j++)
            {
                Type t = fields[j].FieldType;

                if (ToLuaExport.IsDelegateType(t))
                {
                    if (!IsDelegateInClass(t, type) || force)
                    {
                        set.Add(t);
                    }
                }
            }

            for (int j = 0; j < props.Length; j++)
            {
                Type t = props[j].PropertyType;

                if (ToLuaExport.IsDelegateType(t))
                {                    
                    if (!IsDelegateInClass(t, type) || force)
                    {
                        set.Add(t);
                    }
                }
            }

            for (int j = 0; j < methods.Length; j++)
            {
                MethodInfo m = methods[j];

                if (m.IsGenericMethod)
                {
                    continue;
                }

                ParameterInfo[] pifs = m.GetParameters();

                for (int k = 0; k < pifs.Length; k++)
                {
                    Type t = pifs[k].ParameterType;
                    if (t.IsByRef) t = t.GetElementType();

                    if (ToLuaExport.IsDelegateType(t))
                    {                                                
                        if (!IsDelegateInClass(t, type) || force)
                        {
                            set.Add(t);
                        }
                    }
                }
            }

        }

        return set;
    }

    [MenuItem("Lua/Gen Lua Delegates", false, 2)]
    static void GenLuaDelegates()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        ToLuaExport.Clear();
        List<DelegateType> list = new List<DelegateType>();
        list.AddRange(CustomSettings.customDelegateList);
        HashSet<Type> set = GetCustomTypeDelegates(true);        

        foreach (Type t in set)
        {
            if (null == list.Find((p) => { return p.type == t; }))
            {
                list.Add(new DelegateType(t));
            }
        }

        ToLuaExport.GenDelegates(list.ToArray());
        set.Clear();
        ToLuaExport.Clear();
        AssetDatabase.Refresh();
        Debug.Log("Create lua delegate over");
    }    

    static ToLuaTree<string> InitTree()
    {                        
        ToLuaTree<string> tree = new ToLuaTree<string>();
        ToLuaNode<string> root = tree.GetRoot();        
        BindType[] list = GenBindTypes(CustomSettings.customTypeList);

        for (int i = 0; i < list.Length; i++)
        {
            string space = list[i].nameSpace;
            AddSpaceNameToTree(tree, root, space);
        }

        DelegateType[] dts = CustomSettings.customDelegateList;
        string str = null;      

        for (int i = 0; i < dts.Length; i++)
        {            
            string space = ToLuaExport.GetNameSpace(dts[i].type, out str);
            AddSpaceNameToTree(tree, root, space);            
        }

        return tree;
    }

    static void AddSpaceNameToTree(ToLuaTree<string> tree, ToLuaNode<string> parent, string space)
    {
        if (space == null || space == string.Empty)
        {
            return;
        }

        string[] ns = space.Split(new char[] { '.' });

        for (int j = 0; j < ns.Length; j++)
        {
            List<ToLuaNode<string>> nodes = tree.Find((_t) => { return _t == ns[j]; }, j);

            if (nodes.Count == 0)
            {
                ToLuaNode<string> node = new ToLuaNode<string>();
                node.value = ns[j];
                parent.childs.Add(node);
                node.parent = parent;
                node.layer = j;
                parent = node;
            }
            else
            {
                bool flag = false;
                int index = 0;

                for (int i = 0; i < nodes.Count; i++)
                {
                    int count = j;
                    int size = j;
                    ToLuaNode<string> nodecopy = nodes[i];

                    while (nodecopy.parent != null)
                    {
                        nodecopy = nodecopy.parent;
                        if (nodecopy.value != null && nodecopy.value == ns[--count])
                        {
                            size--;
                        }
                    }

                    if (size == 0)
                    {
                        index = i;
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    ToLuaNode<string> nnode = new ToLuaNode<string>();
                    nnode.value = ns[j];
                    nnode.layer = j;
                    nnode.parent = parent;
                    parent.childs.Add(nnode);
                    parent = nnode;
                }
                else
                {
                    parent = nodes[index];
                }
            }
        }
    }

    static string GetSpaceNameFromTree(ToLuaNode<string> node)
    {
        string name = node.value;

        while (node.parent != null && node.parent.value != null)
        {
            node = node.parent;
            name = node.value + "." + name;
        }

        return name;
    }

    static string RemoveTemplateSign(string str)
    {
        str = str.Replace('<', '_');

        int index = str.IndexOf('>');

        while (index > 0)
        {
            str = str.Remove(index, 1);
            index = str.IndexOf('>');
        }

        return str;
    }
     
    [MenuItem("Lua/Gen LuaBinder File", false, 4)]
    static void GenLuaBinder()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        allTypes.Clear();
        ToLuaTree<string> tree = InitTree();        
        StringBuilder sb = new StringBuilder();
        List<DelegateType> dtList = new List<DelegateType>();

        List<DelegateType> list = new List<DelegateType>();
        list.AddRange(CustomSettings.customDelegateList);
        HashSet<Type> set = GetCustomTypeDelegates();

        List<BindType> backupList = new List<BindType>();
        backupList.AddRange(allTypes);
        ToLuaNode<string> root = tree.GetRoot();
        string libname = null;

        foreach (Type t in set)
        {
            if (null == list.Find((p) => { return p.type == t; }))
            {
                DelegateType dt = new DelegateType(t);                                
                AddSpaceNameToTree(tree, root, ToLuaExport.GetNameSpace(t, out libname));
                list.Add(dt);
            }
        }

        sb.AppendLineEx("//this source code was auto-generated by tolua#, do not modify it");
        sb.AppendLineEx("using System;");
        sb.AppendLineEx("using UnityEngine;");
        sb.AppendLineEx("using LuaInterface;");
        sb.AppendLineEx();
        sb.AppendLineEx("public static class LuaBinder");
        sb.AppendLineEx("{");
        sb.AppendLineEx("\tpublic static void Bind(LuaState L)");
        sb.AppendLineEx("\t{");
        sb.AppendLineEx("\t\tfloat t = Time.realtimeSinceStartup;");
        sb.AppendLineEx("\t\tL.BeginModule(null);");

        GenRegisterInfo(null, sb, list, dtList);

        Action<ToLuaNode<string>> begin = (node) =>
        {
            if (node.value == null)
            {
                return;
            }

            sb.AppendFormat("\t\tL.BeginModule(\"{0}\");\r\n", node.value);
            string space = GetSpaceNameFromTree(node);

            GenRegisterInfo(space, sb, list, dtList);
        };

        Action<ToLuaNode<string>> end = (node) =>
        {
            if (node.value != null)
            {
                sb.AppendLineEx("\t\tL.EndModule();");
            }
        };

        tree.DepthFirstTraversal(begin, end, tree.GetRoot());        
        sb.AppendLineEx("\t\tL.EndModule();");
        
        if (CustomSettings.dynamicList.Count > 0)
        {
            sb.AppendLineEx("\t\tL.BeginPreLoad();");            

            for (int i = 0; i < CustomSettings.dynamicList.Count; i++)
            {
                Type t1 = CustomSettings.dynamicList[i];
                BindType bt = backupList.Find((p) => { return p.type == t1; });                
                if (bt != null) sb.AppendFormat("\t\tL.AddPreLoad(\"{0}\", new LuaCSFunction(LuaOpen_{1}), typeof({0}));\r\n", bt.name, bt.wrapName);
            }

            sb.AppendLineEx("\t\tL.EndPreLoad();");
        }

        sb.AppendLineEx("\t\tDebugger.Log(\"Register lua type cost time: {0}\", Time.realtimeSinceStartup - t);");
        sb.AppendLineEx("\t}");

        for (int i = 0; i < dtList.Count; i++)
        {
            ToLuaExport.GenEventFunction(dtList[i].type, sb);
        }

        if (CustomSettings.dynamicList.Count > 0)
        {
            
            for (int i = 0; i < CustomSettings.dynamicList.Count; i++)
            {
                Type t = CustomSettings.dynamicList[i];
                BindType bt = backupList.Find((p) => { return p.type == t; });
                if (bt != null) GenPreLoadFunction(bt, sb);
            }            
        }

        sb.AppendLineEx("}\r\n");
        allTypes.Clear();
        string file = CustomSettings.saveDir + "LuaBinder.cs";

        using (StreamWriter textWriter = new StreamWriter(file, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }

        AssetDatabase.Refresh();
        Debugger.Log("Generate LuaBinder over !");
    }

    static void GenRegisterInfo(string nameSpace, StringBuilder sb, List<DelegateType> delegateList, List<DelegateType> wrappedDelegatesCache)
    {
        for (int i = 0; i < allTypes.Count; i++)
        {
            Type dt = CustomSettings.dynamicList.Find((p) => { return allTypes[i].type == p; });

            if (dt == null && allTypes[i].nameSpace == nameSpace)
            {
                string str = "\t\t" + allTypes[i].wrapName + "Wrap.Register(L);\r\n";
                sb.Append(str);
                allTypes.RemoveAt(i--);
            }
        }

        string funcName = null;

        for (int i = 0; i < delegateList.Count; i++)
        {
            DelegateType dt = delegateList[i];
            Type type = dt.type;
            string typeSpace = ToLuaExport.GetNameSpace(type, out funcName);

            if (typeSpace == nameSpace)
            {
                funcName = ToLuaExport.ConvertToLibSign(funcName);
                string abr = dt.abr;
                abr = abr == null ? funcName : abr;                
                sb.AppendFormat("\t\tL.RegFunction(\"{0}\", new LuaCSFunction({1}));\r\n", abr, dt.name);
                wrappedDelegatesCache.Add(dt);
            }
        }
    }

    static void GenPreLoadFunction(BindType bt, StringBuilder sb)
    {
        string funcName = "LuaOpen_" + bt.wrapName;

        sb.AppendLineEx("\r\n\t[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]");
        sb.AppendFormat("\tstatic int {0}(IntPtr L)\r\n", funcName);
        sb.AppendLineEx("\t{");
        sb.AppendLineEx("\t\ttry");
        sb.AppendLineEx("\t\t{");        
        sb.AppendLineEx("\t\t\tLuaState state = LuaState.Get(L);");
        sb.AppendFormat("\t\t\tstate.BeginPreModule(\"{0}\");\r\n", bt.nameSpace);
        sb.AppendFormat("\t\t\t{0}Wrap.Register(state);\r\n", bt.wrapName);
        sb.AppendFormat("\t\t\tint reference = state.GetMetaReference(typeof({0}));\r\n", bt.name);
        sb.AppendLineEx("\t\t\tstate.EndPreModule(L, reference);");                
        sb.AppendLineEx("\t\t\treturn 1;");
        sb.AppendLineEx("\t\t}");
        sb.AppendLineEx("\t\tcatch(Exception e)");
        sb.AppendLineEx("\t\t{");
        sb.AppendLineEx("\t\t\treturn LuaDLL.toluaL_exception(L, e);");
        sb.AppendLineEx("\t\t}");
        sb.AppendLineEx("\t}");
    }

    static string GetOS()
    {
        return LuaConst.osDir;
    }

    static string CreateStreamDir(string dir)
    {
        dir = Application.streamingAssetsPath + "/" + dir;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return dir;
    }

    static void BuildLuaBundle(string subDir, string sourceDir)
    {
        string[] files = Directory.GetFiles(sourceDir + subDir, "*.bytes");
        string bundleName = subDir == null ? "lua.unity3d" : "lua" + subDir.Replace('/', '_') + ".unity3d";
        bundleName = bundleName.ToLower();

#if UNITY_4_6 || UNITY_4_7
        List<Object> list = new List<Object>();

        for (int i = 0; i < files.Length; i++)
        {
            Object obj = AssetDatabase.LoadMainAssetAtPath(files[i]);
            list.Add(obj);
        }

        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;

        if (files.Length > 0)
        {
            string output = string.Format("{0}/{1}/" + bundleName, Application.streamingAssetsPath, GetOS());
            File.Delete(output);
            BuildPipeline.BuildAssetBundle(null, list.ToArray(), output, options, EditorUserBuildSettings.activeBuildTarget);            
        }
#else
        for (int i = 0; i < files.Length; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(files[i]);

            if (importer)
            {
                importer.assetBundleName = bundleName;
                importer.assetBundleVariant = null;
            }
        }
#endif
    }

    static void ClearAllLuaFiles()
    {
        string osPath = Application.streamingAssetsPath + "/" + GetOS();

        if (Directory.Exists(osPath))
        {
            string[] files = Directory.GetFiles(osPath, "Lua*.unity3d");

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
        }

        string path = osPath + "/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        path = Application.streamingAssetsPath + "/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        if (Directory.Exists(osPath))
        {
            Directory.Delete(osPath, true);
        }

        path = Application.dataPath + "/temp";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        path = Application.dataPath + "/Resources/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        path = Application.persistentDataPath + "/" + GetOS() + "/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    [MenuItem("Lua/Gen LuaWrap + Binder", false, 4)]
    static void GenLuaWrapBinder()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        beAutoGen = true;        
        AssetDatabase.Refresh();
        GenerateClassWraps();
        GenLuaBinder();
        beAutoGen = false;   
    }

    [MenuItem("Lua/Generate All", false, 5)]
    static void GenLuaAll()
    {
        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        beAutoGen = true;
        GenLuaDelegates();
        AssetDatabase.Refresh();
        GenerateClassWraps();
        GenLuaBinder();
        beAutoGen = false;
    }

    [MenuItem("Lua/Clear wrap files", false, 6)]
    static void ClearLuaWraps()
    {
        string[] files = Directory.GetFiles(CustomSettings.saveDir, "*.cs", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }

        ToLuaExport.Clear();
        List<DelegateType> list = new List<DelegateType>();
        ToLuaExport.GenDelegates(list.ToArray());
        ToLuaExport.Clear();

        StringBuilder sb = new StringBuilder();
        sb.AppendLineEx("using System;");
        sb.AppendLineEx("using LuaInterface;");
        sb.AppendLineEx();
        sb.AppendLineEx("public static class LuaBinder");
        sb.AppendLineEx("{");
        sb.AppendLineEx("\tpublic static void Bind(LuaState L)");
        sb.AppendLineEx("\t{");
        sb.AppendLineEx("\t\tthrow new LuaException(\"Please generate LuaBinder files first!\");");
        sb.AppendLineEx("\t}");
        sb.AppendLineEx("}");

        string file = CustomSettings.saveDir + "LuaBinder.cs";

        using (StreamWriter textWriter = new StreamWriter(file, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }

        AssetDatabase.Refresh();
    }

    static void CopyLuaBytesFiles(string sourceDir, string destDir, bool appendext = true, string searchPattern = "*.lua", SearchOption option = SearchOption.AllDirectories)
    {
        if (!Directory.Exists(sourceDir))
        {
            return;
        }

        string[] files = Directory.GetFiles(sourceDir, searchPattern, option);
        int len = sourceDir.Length;

        if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
        {
            --len;
        }         

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, len + 1);
            string dest = destDir + "/" + str;
            if (appendext) dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);            
            File.Copy(files[i], dest, true);            
        }

        Debug.Log("copy " + sourceDir + " " + destDir);
    }


    [MenuItem("Lua/Copy Lua  files to Resources", false, 51)]
    public static void CopyLuaFilesToRes()
    {
        ClearAllLuaFiles();
        string destDir = Application.dataPath + "/Resources" + "/Lua";
        CopyLuaBytesFiles(LuaConst.luaDir, destDir);
        CopyLuaBytesFiles(LuaConst.toluaDir, destDir);
        AssetDatabase.Refresh();
        Debug.Log("Copy lua files over");
    }

    [MenuItem("Lua/Copy Lua  files to Persistent", false, 52)]
    public static void CopyLuaFilesToPersistent()
    {
        ClearAllLuaFiles();
        string destDir = Application.persistentDataPath + "/" + GetOS() + "/Lua";
        CopyLuaBytesFiles(LuaConst.luaDir, destDir, false);
        CopyLuaBytesFiles(LuaConst.toluaDir, destDir, false);
        AssetDatabase.Refresh();
        Debug.Log("Copy lua files over");
    }

    static void GetAllDirs(string dir, List<string> list)
    {
        string[] dirs = Directory.GetDirectories(dir);
        list.AddRange(dirs);

        for (int i = 0; i < dirs.Length; i++)
        {
            GetAllDirs(dirs[i], list);
        }
    }

    static void CopyDirectory(string source, string dest, string searchPattern = "*.lua", SearchOption option = SearchOption.AllDirectories)
    {                
        string[] files = Directory.GetFiles(source, searchPattern, option);

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, source.Length);
            string path = dest + "/" + str;
            string dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], path, true);
        }        
    }

    [MenuItem("Lua/Build Lua files to Resources (PC)", false, 53)]
    public static void BuildLuaToResources()
    {
#if LUAC_5_3
        Debug.Log("Building luac5.3 bytecode for all platform");
#else
        Debug.Log("Building Luajit bytecode for " + LuaConst.osDir);
#endif

        ClearAllLuaFiles();
        string tempDir = CreateStreamDir("Lua");
        string destDir = Application.dataPath + "/Resources" + "/Lua";        
        string tempBytesDir = tempDir + "/Out";

        CopyLuaBytesFiles(LuaConst.luaDir, tempDir, false);
        ExportAllLuaToBytecode(tempDir, tempBytesDir);
        CopyLuaBytesFiles(tempBytesDir, destDir, false, "*.lua.bytes");
        CopyLuaBytesFiles(LuaConst.toluaDir, destDir);
        
        Directory.Delete(tempDir, true);                
        AssetDatabase.Refresh();
#if !LUAC_5_3
        Debug.Log("Building Luajit bytecode for " + LuaConst.osDir + " over");
#else
        Debug.Log("Building Luac5.3 bytecode for " + LuaConst.osDir + " over");
#endif
    }

    [MenuItem("Lua/Build Lua files to Persistent (PC)", false, 54)]
    public static void BuildLuaToPersistent()
    {
#if LUAC_5_3
        Debug.Log("Building luac5.3 bytecode for all platform");
#else
        Debug.Log("Building Luajit bytecode for " + LuaConst.osDir);
#endif
        ClearAllLuaFiles();
        string tempDir = CreateStreamDir("Lua");        
        string destDir = Application.persistentDataPath + "/" + GetOS() + "/Lua/";
        string tempBytesDir = tempDir + "/Out/";

        CopyLuaBytesFiles(LuaConst.luaDir, tempDir, false);
        ExportAllLuaToBytecode(tempDir, tempBytesDir);
        CopyLuaBytesFiles(tempBytesDir, destDir, false, "*.lua.bytes");
        CopyLuaBytesFiles(LuaConst.toluaDir, destDir, false);

        //string[] files = Directory.GetFiles(tempBytesDir, "*.lua.bytes");
        //int len = tempBytesDir.Length;

        //for (int i = 0; i < files.Length; i++)
        //{
        //    tempBytesDir = files[i].Remove(0, len);
        //    tempBytesDir = tempBytesDir.Substring(0, tempBytesDir.Length - 6);
        //    tempBytesDir = destDir + tempBytesDir;

        //    File.Copy(files[i], tempBytesDir, true);
        //}

        Directory.Delete(tempDir, true);
        AssetDatabase.Refresh();
#if !LUAC_5_3
        Debug.Log("Building Luajit bytecode for " + LuaConst.osDir + " over");
#else
        Debug.Log("Building Luac5.3 bytecode for " + LuaConst.osDir + " over");
#endif        
    }

    [MenuItem("Lua/Build lua string bundle files", false, 55)]
    //直接将lua字符串打包，不导出成bytecode
    public static void BuildNotJitBundles()
    {
#if LUAC_5_3
        Debug.Log("Building luac5.3 bundle for all platform");
#else
        Debug.Log("Building Luajit bundle for " + LuaConst.osDir);
#endif
        ClearAllLuaFiles();
        CreateStreamDir(GetOS());

#if UNITY_4_6 || UNITY_4_7
        string tempDir = CreateStreamDir("Lua");
#else
        string tempDir = Application.dataPath + "/temp/Lua";

        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }        
#endif
        CopyLuaBytesFiles(LuaConst.luaDir, tempDir);
        CopyLuaBytesFiles(LuaConst.toluaDir, tempDir);

        AssetDatabase.Refresh();
        List<string> dirs = new List<string>();
        GetAllDirs(tempDir, dirs);

#if UNITY_5 || UNITY_5_3_OR_NEWER
		for (int i = 0; i < dirs.Count; i++)
        {
            string str = dirs[i].Remove(0, tempDir.Length);
            BuildLuaBundle(str.Replace('\\', '/'), "Assets/temp/Lua");
        }

        BuildLuaBundle(null, "Assets/temp/Lua");

        AssetDatabase.SaveAssets();        
        string output = string.Format("{0}/{1}", Application.streamingAssetsPath, GetOS());        
        BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        Directory.Delete(Application.dataPath + "/temp/", true);
#else
        for (int i = 0; i < dirs.Count; i++)
        {
            string str = dirs[i].Remove(0, tempDir.Length);
            BuildLuaBundle(str.Replace('\\', '/'), "Assets/StreamingAssets/Lua");
        }

        BuildLuaBundle(null, "Assets/StreamingAssets/Lua");
        Directory.Delete(Application.streamingAssetsPath + "/Lua/", true);
#endif
        AssetDatabase.Refresh();
#if !LUAC_5_3
        Debug.Log("Building Luajit bundle for " + LuaConst.osDir + " over");
#else
        Debug.Log("Building Luac5.3 bundle for " + LuaConst.osDir + " over");
#endif        
    }

    [MenuItem("Lua/Build Lua bytecode bundle files   (PC)", false, 56)]
    // 使用luajit就是编译成当前平台架构（32位、64位）对应的luajit的bytecode。（自己可参考出手机包luajit的32位、64位2套字节码bundle包）
    // 使用lua5.3就是编译成lua5.3的兼容32/64位tolua运行环境的一份bytecode
    public static void BuildLuaBundles()
    {
#if LUAC_5_3
        Debug.Log("Building luac5.3 bytecode bundle for all platform");
#else
        Debug.Log("Building Luajit bytecode bundle for " + LuaConst.osDir);
#endif
        ClearAllLuaFiles();                
        CreateStreamDir(GetOS());

#if UNITY_4_6 || UNITY_4_7
        string tempDir = CreateStreamDir("Lua");
#else
        string tempDir = Application.dataPath + "/temp/Lua";

        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }
#endif
        string tempBytesDir = tempDir + "/Out";

        CopyLuaBytesFiles(LuaConst.luaDir, tempDir, false);
        ExportAllLuaToBytecode(tempDir, tempBytesDir);
        CopyLuaBytesFiles(LuaConst.toluaDir, tempBytesDir);

        AssetDatabase.Refresh();

        List<string> dirs = new List<string>();        
        GetAllDirs(tempBytesDir, dirs);

#if UNITY_5 || UNITY_5_3_OR_NEWER
		for (int i = 0; i < dirs.Count; i++)
        {
            string str = dirs[i].Remove(0, tempBytesDir.Length);
            BuildLuaBundle(str.Replace('\\', '/'), "Assets/temp/Lua/Out");
        }

        BuildLuaBundle(null, "Assets/temp/Lua/Out");

        AssetDatabase.Refresh();
        string output = string.Format("{0}/{1}", Application.streamingAssetsPath, GetOS());
        BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        Directory.Delete(Application.dataPath + "/temp/", true);
#else
        for (int i = 0; i < dirs.Count; i++)
        {
            string str = dirs[i].Remove(0, tempBytesDir.Length);
            BuildLuaBundle(str.Replace('\\', '/'), "Assets/StreamingAssets/Lua/Out");
        }

        BuildLuaBundle(null, "Assets/StreamingAssets/Lua/Out/");
        Directory.Delete(tempDir, true);
#endif
        AssetDatabase.Refresh();
#if !LUAC_5_3
        Debug.Log("Building Luajit bytecode bundle for " + LuaConst.osDir + " over");
#else
        Debug.Log("Building Luac5.3 bytecode bundle for " + LuaConst.osDir + " over");
#endif           
    }

    [MenuItem("Lua/Clear all Lua files", false, 57)]
    public static void ClearLuaFiles()
    {
        ClearAllLuaFiles();
    }

    [MenuItem("Lua/Gen BaseType Wrap", false, 101)]
    static void GenBaseTypeLuaWrap()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
            return;
        }

        string dir = CustomSettings.toluaBaseType;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        allTypes.Clear();
        ToLuaExport.allTypes.AddRange(baseType);
        List<BindType> btList = new List<BindType>();
        
        for (int i = 0; i < baseType.Count; i++)
        {
            btList.Add(new BindType(baseType[i]));
        }

        GenBindTypes(btList.ToArray(), false);
        BindType[] list = allTypes.ToArray();

        for (int i = 0; i < list.Length; i++)
        {
            ToLuaExport.Clear();
            ToLuaExport.className = list[i].name;
            ToLuaExport.type = list[i].type;
            ToLuaExport.isStaticClass = list[i].IsStatic;
            ToLuaExport.baseType = list[i].baseType;
            ToLuaExport.wrapClassName = list[i].wrapName;
            ToLuaExport.libClassName = list[i].libName;
            ToLuaExport.Generate(dir);
        }
        
        Debug.Log("Generate base type files over");
        allTypes.Clear();
        AssetDatabase.Refresh();
    }

    static void CreateDefaultWrapFile(string path, string name)
    {
        StringBuilder sb = new StringBuilder();
        path = path + name + ".cs";
        sb.AppendLineEx("using System;");
        sb.AppendLineEx("using LuaInterface;");
        sb.AppendLineEx();
        sb.AppendLineEx("public static class " + name);
        sb.AppendLineEx("{");
        sb.AppendLineEx("\tpublic static void Register(LuaState L)");
        sb.AppendLineEx("\t{");        
        sb.AppendLineEx("\t\tthrow new LuaException(\"Please click menu Lua/Gen BaseType Wrap first!\");");
        sb.AppendLineEx("\t}");
        sb.AppendLineEx("}");

        using (StreamWriter textWriter = new StreamWriter(path, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }
    }
    
    [MenuItem("Lua/Clear BaseType Wrap", false, 102)]
    static void ClearBaseTypeLuaWrap()
    {
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_ObjectWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_DelegateWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_StringWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_EnumWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_TypeWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_Collections_IEnumeratorWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "UnityEngine_ObjectWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "LuaInterface_EventObjectWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "LuaInterface_LuaMethodWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "LuaInterface_LuaPropertyWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "LuaInterface_LuaFieldWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "LuaInterface_LuaConstructorWrap");        

        Debug.Log("Clear base type wrap files over");
        AssetDatabase.Refresh();
    }

    [MenuItem("Lua/Enable Lua Injection &e", false, 102)]
    static void EnableLuaInjection()
    {
        bool EnableSymbols = false;
        if (UpdateMonoCecil(ref EnableSymbols) != -1)
        {
            BuildTargetGroup curBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string existSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(curBuildTargetGroup);
            if (!existSymbols.Contains("ENABLE_LUA_INJECTION"))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(curBuildTargetGroup, existSymbols + ";ENABLE_LUA_INJECTION");
            }

            AssetDatabase.Refresh();
        }
    }

#if ENABLE_LUA_INJECTION
    [MenuItem("Lua/Injection Remove &r", false, 5)]
#endif
    static void RemoveInjection()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "游戏运行过程中无法操作", "确定");
            return;
        }

        BuildTargetGroup curBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string existSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(curBuildTargetGroup);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(curBuildTargetGroup, existSymbols.Replace("ENABLE_LUA_INJECTION", ""));
        Debug.Log("Lua Injection Removed!");
    }

    public static int UpdateMonoCecil(ref bool EnableSymbols)
    {
        string appFileName = Environment.GetCommandLineArgs()[0];
        string appPath = Path.GetDirectoryName(appFileName);
        string directory = appPath + "/Data/Managed/";
        if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.OSXEditor)
        {
            directory = appPath.Substring(0, appPath.IndexOf("MacOS")) + "Managed/";
        }

        string suitedMonoCecilPath = directory +
#if UNITY_2017_1_OR_NEWER
            "Unity.Cecil.dll";
#else
            "Mono.Cecil.dll";
#endif

#if UNITY_5_3_OR_NEWER
        string suitedMonoCecilMdbPath = directory +
#if UNITY_2017_1_OR_NEWER
            "Unity.Cecil.Mdb.dll";
#else
            "Mono.Cecil.Mdb.dll";
#endif

        string suitedMonoCecilPdbPath = directory +
#if UNITY_2017_1_OR_NEWER
            "Unity.Cecil.Pdb.dll";
#else
            "Mono.Cecil.Pdb.dll";
#endif
#endif
        string suitedMonoCecilToolPath = directory + "Unity.CecilTools.dll";

        if (!File.Exists(suitedMonoCecilPath)
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            && !File.Exists(suitedMonoCecilMdbPath)
            && !File.Exists(suitedMonoCecilPdbPath)
#endif
        )
        {
            EnableSymbols = false;
            Debug.Log("Haven't found Mono.Cecil.dll!Symbols Will Be Disabled");
            return -1;
        }

        bool bInjectionToolUpdated = false;
        string injectionToolPath = CustomSettings.injectionFilesPath + "Editor/";
        string existMonoCecilPath = injectionToolPath + Path.GetFileName(suitedMonoCecilPath);
#if UNITY_5_3_OR_NEWER
        string existMonoCecilPdbPath = injectionToolPath + Path.GetFileName(suitedMonoCecilPdbPath);
        string existMonoCecilMdbPath = injectionToolPath + Path.GetFileName(suitedMonoCecilMdbPath);
#endif
        string existMonoCecilToolPath = injectionToolPath + Path.GetFileName(suitedMonoCecilToolPath);

        try
        {
            bInjectionToolUpdated = TryUpdate(suitedMonoCecilPath, existMonoCecilPath) ? true : bInjectionToolUpdated;
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            bInjectionToolUpdated = TryUpdate(suitedMonoCecilPdbPath, existMonoCecilPdbPath) ? true : bInjectionToolUpdated;
            bInjectionToolUpdated = TryUpdate(suitedMonoCecilMdbPath, existMonoCecilMdbPath) ? true : bInjectionToolUpdated;
#endif
            TryUpdate(suitedMonoCecilToolPath, existMonoCecilToolPath);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return -1;
        }
        EnableSymbols = true;

        return bInjectionToolUpdated ? 1 : 0;
    }

    static bool TryUpdate(string srcPath, string destPath)
    {
        if (GetFileContentMD5(srcPath) != GetFileContentMD5(destPath))
        {
            File.Copy(srcPath, destPath, true);
            return true;
        }

        return false;
    }

    static string GetFileContentMD5(string file)
    {
        if (!File.Exists(file))
        {
            return string.Empty;
        }

        FileStream fs = new FileStream(file, FileMode.Open);
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(fs);
        fs.Close();

        StringBuilder sb = StringBuilderCache.Acquire();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return StringBuilderCache.GetStringAndRelease(sb);
    }

    static void ExportAllLuaToBytecode(string srcDir, string outDir, string searchPattern = "*.lua")
    {
        if (!Directory.Exists(srcDir))
        {
            return;
        }

        string[] files = Directory.GetFiles(srcDir, searchPattern, SearchOption.AllDirectories);
        int len = srcDir.Length;

        if (srcDir[len - 1] == '/' || srcDir[len - 1] == '\\')
        {
            --len;
        }

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, len + 1);
            string dest = outDir + "/" + str;
            dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            EditorUtility.DisplayProgressBar("Encoding", files[i], (float)i / files.Length);
            string src = files[i].Replace('\\', '/');
            ExportLuaBytecode(src, dest);
        }
        EditorUtility.ClearProgressBar();
    }

    /// <summary>
    /// 加密lua代码成字节码
    /// </summary>
    /// <param name="srcFile"></param>
    /// <param name="outFile"></param>
    static void ExportLuaBytecode(string srcFile, string outFile)
    {
        if (!srcFile.ToLower().EndsWith(".lua"))
        {
            File.Copy(srcFile, outFile, true);
            return;
        }

        bool isWin = true;        
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string exeRoot = string.Empty;        
        string libRoot = Application.dataPath.Replace('\\', '/');
        libRoot = libRoot.Substring(0, libRoot.LastIndexOf('/'));
        isWin = Application.platform == RuntimePlatform.WindowsEditor;

#if LUAC_5_3
        exeRoot = "Luac53";
        luaexe = isWin ? "luac.exe" : "./luac";
        args = " -o " + outFile + " " + srcFile;
#else

#if UNITY_IPHONE
        exeRoot = "Luajit64";
#elif UNITY_ANDROID
        exeRoot = "Luajit";
#else
        exeRoot = IntPtr.Size == 4 ? "Luajit" : "Luajit64";
#endif

        luaexe = isWin ? "luajit.exe" : "./luajit";
        args = CustomSettings.LUAJIT_CMD_OPTION + " " + srcFile + " " + outFile;
#endif
        exedir = string.Format("{0}/{1}/{2}/", libRoot, exeRoot, isWin ? "Win" : "Mac");
        
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo()
        {
            FileName = exedir + luaexe,
            Arguments = args,            
            UseShellExecute = false,
            ErrorDialog = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
            WorkingDirectory = exedir,            
        };

        if (!isWin)
        {
            info.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            info.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
        }        

        using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(info))
        {                                                
            string msg = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(msg))
            {
                Debug.LogError(msg);
            }
            else
            {
                Debug.Log(luaexe + args);
            }

            process.WaitForExit();
        }
    }

    [MenuItem("Lua/Gen u3d Wrap Files", false, 103)]
    public static void U3dBinding()
    {
        List<string> dropList = new List<string>
        {                             
            //一般情况不需要的类, 编辑器相关的
            "HideInInspector" ,
            "ExecuteInEditMode" ,
            "AddComponentMenu" ,
            "ContextMenu" ,
            "RequireComponent" ,
            "DisallowMultipleComponent" ,
            "SerializeField" ,
            "AssemblyIsEditorAssembly" ,
            "Attribute" ,  //一些列文件，都是编辑器相关的    
            "FFTWindow" ,

            "Types" ,
            "UnitySurrogateSelector" ,
            "TypeInferenceRules" ,
            "ThreadPriority" ,
            "Debug" ,        //自定义debugger取代
            "GenericStack" ,

            //异常，lua无法catch
            "PlayerPrefsException" ,
            "UnassignedReferenceException" ,
            "UnityException" ,
            "MissingComponentException" ,
            "MissingReferenceException" ,

            //RPC网络
            "RPC" ,
            "Network" ,
            "MasterServer" ,
            "BitStream" ,
            "HostData" ,
            "ConnectionTesterStatus" ,

            //unity 自带编辑器GUI
            "GUI" ,
            "EventType" ,
            "EventModifiers" ,
            //"Event",
            "FontStyle" ,
            "TextAlignment" ,
            "TextEditor" ,
            "TextEditorDblClickSnapping" ,
            "TextGenerator" ,
            "TextClipping" ,
            "TextGenerationSettings" ,
            "TextAnchor" ,
            "TextAsset" ,
            "TextWrapMode" ,
            "Gizmos" ,
            "ImagePosition" ,
            "FocusType" ,
           

            //地形相关
            "Terrain" ,
            "Tree" ,
            "SplatPrototype" ,
            "DetailPrototype" ,
            "DetailRenderMode" ,

            //其他
            "MeshSubsetCombineUtility" ,
            "AOT" ,
            "Random" ,
            "Mathf" ,
            "Social" ,
            "Enumerator" ,
            "SendMouseEvents" ,
            "Cursor" ,
            "Flash" ,
            "ActionScript" ,
            "CacheIndex",
            "QualityLevel",
            "RaycastCollider",
            "SerializePrivateVariables",
            "RaycastCollider",
           
   
            //非通用的类
            "ADBannerView" ,
            "ADInterstitialAd" ,
            "Android" ,
            "jvalue" ,
            "iPhone" ,
            "iOS" ,
            "CalendarIdentifier" ,
            "CalendarUnit" ,
            "CalendarUnit" ,
            "FullScreenMovieControlMode" ,
            "FullScreenMovieScalingMode" ,
            "Handheld" ,
            "LocalNotification" ,
            "Motion" ,   //空类
            "NotificationServices" ,
            "RemoteNotificationType" ,
            "RemoteNotification" ,
            "SamsungTV" ,
            "TextureCompressionQuality" ,
            "TouchScreenKeyboardType" ,
            "TouchScreenKeyboard" ,
            "MovieTexture" ,
            
            //2d 类
            "AccelerationEventWrap" , //加速
            "AnimatorUtility" ,
            "AudioChorusFilter" ,
            "AudioDistortionFilter" ,
            "AudioEchoFilter" ,
            "AudioHighPassFilter" ,
            "AudioLowPassFilter" ,
            "AudioReverbFilter" ,
            "AudioReverbPreset" ,
            "AudioReverbZone" ,
            "AudioRolloffMode" ,
            "AudioSettings" ,
            "AudioSpeakerMode" ,
            "AudioType" ,
            "AudioVelocityUpdateMode" ,

            "Profiler" ,
            "StaticBatchingUtility" ,
            "Font" ,
            "Gyroscope" ,                        //不需要重力感应
            "ISerializationCallbackReceiver" ,   //u3d 继承的序列化接口，lua不需要
            "ImageEffectOpaque" ,                //后处理
            "ImageEffectTransformsToLDR" ,
            "PrimitiveType" ,                // 暂时不需要 GameObject.CreatePrimitive          
            "Skybox" ,                       //不会u3d自带的Skybox
            "SparseTexture" ,                // mega texture 不需要
            "Plane" ,
            "PlayerPrefs" ,
            
            //不需要轮子碰撞体
            "WheelCollider" ,
            "WheelFrictionCurve" ,
            "WheelHit" ,

            //手机不适用雾
            "FogMode" ,

            "LightProbeGroup" ,
            "LightProbes" ,            

            //没用到substance纹理
            "ProceduralCacheSize" ,
            "ProceduralLoadingBehavior" ,
            "ProceduralMaterial" ,
            "ProceduralOutputType" ,
            "ProceduralProcessorUsage" ,
            "ProceduralPropertyDescription" ,
            "ProceduralPropertyType" ,
            "ProceduralTexture" ,

            //物理关节系统
            "JointDriveMode" ,
            "JointDrive" ,
            "JointLimits" ,
            "JointMotor" ,
            "JointProjectionMode" ,
            "JointSpring" ,
            "SoftJointLimit" ,
            "SpringJoint" ,
            "HingeJoint" ,
            "FixedJoint" ,
            "ConfigurableJoint" ,
            "CharacterJoint" ,
            "Joint" ,

            "LODGroup" ,
            "LOD" ,

            "CrashReport" ,
            "CombineInstance" ,
#if UNITY_5_4_OR_NEWER
            "ParticleEmitter",
            "ParticleRenderer",
            "ParticleAnimator",
            "AnimatorClipInfo",
            "VRDeviceType",
            "Particle",
            "ParticleRenderMode"
#endif
        };

        List<BindType> list = new List<BindType>();
        Assembly assembly = Assembly.Load("UnityEngine");
        Type[] types = assembly.GetExportedTypes();

        for (int i = 0; i < types.Length; i++)
        {
            //不导出： 模版类，event委托, c#协同相关, obsolete 类
            if (!types[i].IsGenericType && types[i].BaseType != typeof(System.MulticastDelegate) && !typeof(YieldInstruction).IsAssignableFrom(types[i]) && !types[i].IsInterface
                && !BindType.IsObsolete(types[i]))
            {
                list.Add(CustomSettings._GT(types[i]));
            }
            else
            {
                Debug.Log("drop generic type " + types[i].ToString());
            }
        }

        for (int i = 0; i < dropList.Count; i++)
        {
            list.RemoveAll((p) => { return p.type.ToString().Contains(dropList[i]); });
        }

        for (int i = 0; i < list.Count; i++)
        {
            CustomSettings.customTypeList = list.ToArray();
        }

        beAutoGen = true;
        GenLuaDelegates();
        AssetDatabase.Refresh();
        GenerateClassWraps();
        GenLuaBinder();
        beAutoGen = false;
    }
}
