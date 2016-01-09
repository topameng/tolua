using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;

using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using System.Threading;

[InitializeOnLoad]
public static class ToLuaMenu
{
    //不需要导出或者无法导出的类型
    public static List<Type> dropType = new List<Type>
    {
        typeof(ValueType),                                  //不需要
        typeof(Motion),                                     //很多平台只是空类
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
        typeof(LuaInterface.LuaOutMetatable),               //手写支持
        typeof(LuaInterface.NullObject),     
        typeof(LuaInterface.LuaOutMetatable),    
        typeof(System.Array),                        
        typeof(System.Reflection.MemberInfo),               
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
    };

    private static bool beAutoGen = false;

    static ToLuaMenu()
    {
        string dir = CustomSettings.saveDir;
        string[] files = Directory.GetFiles(dir, "*.cs", SearchOption.TopDirectoryOnly);

        if (files.Length < 3)
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

        public BindType(Type t)
        {
            type = t;                        
            nameSpace = ToLuaExport.GetNameSpace(t, out libName);
            name = ToLuaExport.CombineTypeStr(nameSpace, libName);
            nameSpace = ToLuaExport.ConvertToLibSign(nameSpace);
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

            if (t.BaseType != null && t.BaseType != typeof(ValueType))
            {                
                baseType = t.BaseType;
            }
            
            int index = CustomSettings.staticClassTypes.IndexOf(t);

            if (index >= 0 || (t.GetConstructor(Type.EmptyTypes) == null && t.IsAbstract && t.IsSealed))
            {                         
                IsStatic = true;
                baseType = baseType == typeof(object) ?  null : baseType;
            }
        }

        public BindType SetBaseType(Type t)
        {
            baseType = t;
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
    }

    static BindType[] GenBindTypes(BindType[] list, bool beDropBaseType = true)
    {                
        allTypes = new List<BindType>(list);

        for (int i = 0; i < list.Length; i++)
        {            
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

            Type t = list[i].baseType;

            while (t != null)
            {
                if (t.IsInterface)
                {
                    list[i].baseType = t.BaseType;       
                }
                else if (dropType.IndexOf(t) >= 0)
                {
                    Debugger.LogWarning("{0} has a base type {1} is a drop type", list[i].name, t.FullName);
                    list[i].baseType = t.BaseType;                    
                }
                else if (!beDropBaseType || baseType.IndexOf(t) < 0)
                {
                    int index = allTypes.FindIndex((bt) => { return bt.type == t; });

                    if (index < 0)
                    {
                        Debugger.LogWarning("not defined bindtype for {0}, autogen it, child class is {1}", t.FullName, list[i].name);
                        BindType bt = new BindType(t);
                        allTypes.Add(bt);
                    }                    
                }

                t = t.BaseType;
            }
        }

        return allTypes.ToArray();
    }

    [MenuItem("Lua/Gen Lua Wrap Files", false, 1)]
    public static void GenerateClassWraps()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        if (!File.Exists(CustomSettings.saveDir))
        {
            Directory.CreateDirectory(CustomSettings.saveDir);
        }

        allTypes.Clear();
        BindType[] list = GenBindTypes(CustomSettings.customTypeList);

        for (int i = 0; i < list.Length; i++)
        {
            ToLuaExport.Clear();
            ToLuaExport.className = list[i].name;
            ToLuaExport.type = list[i].type;
            ToLuaExport.isStaticClass = list[i].IsStatic;            
            ToLuaExport.baseType = list[i].baseType;
            ToLuaExport.wrapClassName = list[i].wrapName;
            ToLuaExport.libClassName = list[i].libName;
            ToLuaExport.Generate(CustomSettings.saveDir);
        }

        EditorApplication.isPlaying = false;
        Debug.Log("Generate lua binding files over");
        allTypes.Clear();
        AssetDatabase.Refresh();
    }

    static HashSet<Type> GetCustomTypeDelegates()
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

                if (typeof(System.Delegate).IsAssignableFrom(t))
                {
                    set.Add(t);
                }
            }

            for (int j = 0; j < props.Length; j++)
            {
                Type t = props[j].PropertyType;

                if (typeof(System.Delegate).IsAssignableFrom(t))
                {
                    set.Add(t);
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

                    if (typeof(System.MulticastDelegate).IsAssignableFrom(t))
                    {
                        set.Add(t);
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
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        ToLuaExport.Clear();
        List<DelegateType> list = new List<DelegateType>();
        list.AddRange(CustomSettings.customDelegateList);
        HashSet<Type> set = GetCustomTypeDelegates();        

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

    static List<BindType> allTypes = new List<BindType>();

    static ToLuaTree<string> InitTree()
    {                        
        ToLuaTree<string> tree = new ToLuaTree<string>();
        ToLuaNode<string> root = tree.GetRoot();        
        BindType[] list = GenBindTypes(CustomSettings.customTypeList);

        for (int i = 0; i < list.Length; i++)
        {
            string space = list[i].nameSpace;

            if (space == null || space == string.Empty)
            {
                continue;
            }

            string[] ns = space.Split(new char[] { '.' });
            ToLuaNode<string> parent = root;

            for (int j = 0; j < ns.Length; j++)
            {
                ToLuaNode<string> node = tree.Find((_t) => { return _t == ns[j];});

                if (node == null)
                {
                    node = new ToLuaNode<string>();
                    node.value = ns[j];
                    parent.childs.Add(node);
                    node.parent = parent;
                    parent = node;
                }
                else
                {
                    parent = node;
                }
            }
        }

        return tree;
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

    static string GetNameSpace(Type t, out string libName)
    {
        if (t.IsGenericType)
        {
            string space = t.Namespace;

            if (t.FullName.Contains("+"))
            {
                space = ToLuaExport.GetTypeStr(t);
                int len = t.Namespace == null ? 0 : t.Namespace.Length;                
                space = space.Remove(0, len);
                space = RemoveTemplateSign(space);
                int index = space.LastIndexOf('.');
                libName = space.Substring(index + 1);
                space = space.Substring(0, index);                
                space = t.Namespace + space;                
            }
            else
            {
                libName = ToLuaExport.GetTypeStr(t);
                libName = space == null ? libName : libName.Substring(space.Length + 1);
                libName = RemoveTemplateSign(libName);
            }

            return space;
        }
        else
        {
            string space = t.FullName;

            if (space.Contains("+"))
            {
                space = space.Replace('+', '.');
                int index = space.LastIndexOf('.');
                libName = space.Substring(index + 1);
                return space.Substring(0, index);                                
            }
            else
            {
                libName = t.Namespace == null ? space : space.Substring(t.Namespace.Length + 1);
                return t.Namespace;
            }
        }        
    }

    /*[MenuItem("Lua/Gen DelegateFactoryWrap", false, 3)]
    static void GenLuaDelegateWrap()
    {
        if (!Application.isPlaying)
        {
            EditorApplication.isPlaying = true;
            AssetDatabase.Refresh();
        }

        BindType bt = new BindType(typeof(DelegateFactory));
        ToLuaExport.Clear();
        ToLuaExport.className = bt.name;
        ToLuaExport.type = bt.type;
        ToLuaExport.isStaticClass = true;
        ToLuaExport.baseType = null;
        ToLuaExport.wrapClassName = bt.wrapName;
        ToLuaExport.libClassName = bt.libName;
        ToLuaExport.Generate(CustomSettings.saveDir);

        EditorApplication.isPlaying = false;        
        allTypes.Clear();
        AssetDatabase.Refresh();
        GenLuaBinder();
    }*/
     
    [MenuItem("Lua/Gen LuaBinder File", false, 4)]
    static void GenLuaBinder()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        allTypes.Clear();
        ToLuaTree<string> tree = InitTree();        
        StringBuilder sb = new StringBuilder();

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

        if (File.Exists(CustomSettings.saveDir + "DelegateFactoryWrap.cs"))
        {
            sb.AppendLineEx("\t\tDelegateFactoryWrap.Register(L);");
        }

        for (int i = 0; i < allTypes.Count; i++)
        {
            if (allTypes[i].nameSpace == null)
            {
                string str = "\t\t" + allTypes[i].wrapName + "Wrap.Register(L);\r\n";
                sb.Append(str);
                allTypes.RemoveAt(i--);                
            }
        }        

        Action<ToLuaNode<string>> begin = (node) =>
        {
            if (node.value == null)
            {
                return;
            }

            sb.AppendFormat("\t\tL.BeginModule(\"{0}\");\r\n", node.value);
            string space = GetSpaceNameFromTree(node);

            for (int i =0; i < allTypes.Count; i++)
            {                                                
                if (allTypes[i].nameSpace == space)
                {
                    string str = "\t\t" + allTypes[i].wrapName + "Wrap.Register(L);\r\n";
                    sb.Append(str);
                    allTypes.RemoveAt(i--);
                }
            }            
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
        sb.AppendLineEx("\t\tDebugger.Log(\"Register lua type cost time: {0}\", Time.realtimeSinceStartup - t);");
        sb.AppendLineEx("\t}");
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
    }

    static string GetOS()
    {
#if UNITY_STANDALONE
        return "Win";
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_IPHONE
        return "IOS";
#endif
    }

    static void CreateStreamDir(string dir)
    {
        dir = Application.streamingAssetsPath + "/" + dir;

        if (!File.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void BuildLuaBundle(string dir)
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;

        string[] files = Directory.GetFiles("Assets/StreamingAssets/Lua/" + dir, "*.lua.bytes");
        List<Object> list = new List<Object>();
        string bundleName = dir == null ? "Lua.unity3d" : "Lua_" + dir + ".unity3d";

        for (int i = 0; i < files.Length; i++)
        {
            Object obj = AssetDatabase.LoadMainAssetAtPath(files[i]);
            list.Add(obj);
        }

        if (files.Length > 0)
        {            
            string output = string.Format("{0}/{1}/" + bundleName, Application.streamingAssetsPath, GetOS());
            File.Delete(output);
            BuildPipeline.BuildAssetBundle(null, list.ToArray(), output, options, EditorUserBuildSettings.activeBuildTarget);
            string output1 = string.Format("{0}/{1}/" + bundleName, Application.persistentDataPath, GetOS());
            File.Copy(output, output1, true);
            AssetDatabase.Refresh();
        }
    }

    static void CopyLuaFiles()
    {
        string sourceDir = Application.streamingAssetsPath + "/Lua";
        string destDir = Application.streamingAssetsPath + "/" + GetOS() + "/Lua";
        string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
        
        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, sourceDir.Length);
            string dest = destDir + str;
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], dest, true);            
        }

        Directory.Delete(sourceDir, true);
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

        path = Application.dataPath + "/Resources/Lua";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    [MenuItem("Lua/Build Lua files  (PC运行)", false, 5)]
    public static void BuildLua()
    {
        ClearAllLuaFiles();
        CreateStreamDir(GetOS());
        CreateStreamDir("Lua/Out/");

        Process proc = Process.Start(Application.dataPath + "/ToLua/Lua/Build.bat");
        proc.WaitForExit();
        UnityEngine.Debug.Log("build tolua fils over");

        if (File.Exists(CustomSettings.luaDir + "/Build.bat"))
        {
            proc = Process.Start(CustomSettings.luaDir + "/Build.bat");
            UnityEngine.Debug.Log("build lua files over");
            proc.WaitForExit();
        }

        CreateStreamDir(GetOS() + "/Lua");
        CopyLuaFiles();
        AssetDatabase.Refresh();
    }

    [MenuItem("Lua/Build Luajit bundle files   (PC运行)", false, 6)]
    public static void BuildLuaBundles()
    {
        ClearAllLuaFiles();
        CreateStreamDir(GetOS());
        CreateStreamDir("Lua/Out/");
        string dir = Application.persistentDataPath + "/" + GetOS();

        if (!File.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        Process proc = Process.Start(Application.dataPath + "/ToLua/Lua/Build.bat");
        proc.WaitForExit();
        UnityEngine.Debug.Log("build tolua fils over");

        if (File.Exists(CustomSettings.luaDir + "/Build.bat"))
        {
            proc = Process.Start(CustomSettings.luaDir + "/Build.bat");
            UnityEngine.Debug.Log("build lua files over");
            proc.WaitForExit();
        }
        
        AssetDatabase.Refresh();        
        string sourceDir = Application.streamingAssetsPath + "/Lua";
        string[] dirs = Directory.GetDirectories(sourceDir);

        for (int i = 0; i < dirs.Length; i++)
        {
            string str = dirs[i].Remove(0, sourceDir.Length + 1);
            BuildLuaBundle(str);
        }

        BuildLuaBundle(null);        
        Directory.Delete(sourceDir, true);        
        AssetDatabase.Refresh();
    }

    [MenuItem("Lua/Build bundle files not jit", false, 6)]
    public static void BuildNotJitBundles()
    {
        ClearAllLuaFiles();
        CreateStreamDir(GetOS());
        CreateStreamDir("Lua/");
        string dir = Application.persistentDataPath + "/" + GetOS();

        if (!File.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string streamDir = Application.streamingAssetsPath + "/Lua";
        CopyLuaBytesFiles(CustomSettings.luaDir, streamDir);
        CopyLuaBytesFiles(Application.dataPath + "/ToLua/Lua", streamDir);

        AssetDatabase.Refresh();
        string[] dirs = Directory.GetDirectories(streamDir);

        for (int i = 0; i < dirs.Length; i++)
        {
            string str = dirs[i].Remove(0, streamDir.Length + 1);
            BuildLuaBundle(str);
        }

        BuildLuaBundle(null);
        Directory.Delete(streamDir, true);
        AssetDatabase.Refresh();
    }

    static void CopyLuaBytesFiles(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            return;
        }

        string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
        int len = sourceDir.Length;

        if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
        {
            --len;
        }

        for (int i = 0; i < files.Length; i++)
        {            
            string str = files[i].Remove(0, len);            
            string dest = destDir + str + ".bytes";
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], dest, true);
        }        
    }

    [MenuItem("Lua/Copy Lua  files to Resources", false, 7)]
    public static void CopyLuaFilesToRes()
    {
        ClearAllLuaFiles();
        CreateStreamDir(GetOS());
        CreateStreamDir(GetOS() + "/Lua");
        string destDir = Application.dataPath + "/Resources" + "/Lua";
        CopyLuaBytesFiles(CustomSettings.luaDir, destDir);
        CopyLuaBytesFiles(Application.dataPath + "/ToLua/Lua", destDir);
        AssetDatabase.Refresh();
    }

    [MenuItem("Lua/Clear wrap files", false, 10)]
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

    [MenuItem("Lua/", false, 20)]
    static void Breaker() { }

    [MenuItem("Lua/Gen BaseType Wrap", false, 21)]
    static void GenBaseTypeLuaWrap()
    {
        if (!beAutoGen && EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译在执行此功能", "确定");
            return;
        }

        string dir = Application.dataPath + "/ToLua/BaseType/";

        if (!File.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        allTypes.Clear();
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
        sb.AppendLineEx("\t}");
        sb.AppendLineEx("}");

        using (StreamWriter textWriter = new StreamWriter(path, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }
    }
    
    [MenuItem("Lua/Clear BaseType Wrap", false, 22)]
    static void ClearBaseTypeLuaWrap()
    {
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_ObjectWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_DelegateWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_StringWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_EnumWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_TypeWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "System_Collections_IEnumeratorWrap");
        CreateDefaultWrapFile(CustomSettings.toluaBaseType, "UnityEngine_ObjectWrap");
    }
}
