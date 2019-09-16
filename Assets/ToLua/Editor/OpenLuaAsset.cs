using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using System.IO;

public class OpenLuaAsset : Editor
{
    private static int toluaFileID = -1;    
    private static int luaStateFileID = -1;
    private static int luaDllFileID = -1;

    private static object consoleWindow;
    private static object logListView;
    private static FieldInfo logListViewCurrentRow;
    private static MethodInfo LogEntriesGetEntry;

#if UNITY_2017_1_OR_NEWER
    private static MethodInfo StartGettingEntries;
    private static MethodInfo EndGettingEntries;
#endif
    private static object logEntry;
    private static FieldInfo logEntryCondition;
    private static UnityEngine.Object lastObject = null;

    const string LuaExceptionHeader = "LuaException:";
    const string LuaStringChunk = "[string \"";


    private static bool GetConsoleWindowListView()
    {
        if (logListView == null)
        {
            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            consoleWindow = fieldInfo.GetValue(null);

            if (consoleWindow == null)
            {
                logListView = null;
                return false;
            }

            FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
            logListView = listViewFieldInfo.GetValue(consoleWindow);
            logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
#if UNITY_2017_1_OR_NEWER
            Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            Type logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");                
            StartGettingEntries = logEntriesType.GetMethod("StartGettingEntries", BindingFlags.Static | BindingFlags.Public);
            EndGettingEntries = logEntriesType.GetMethod("EndGettingEntries", BindingFlags.Static | BindingFlags.Public);
#else
            Type logEntriesType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            Type logEntryType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntry");
#endif
            logEntryCondition = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
            logEntry = Activator.CreateInstance(logEntryType);
        }

        return true;
    }

    public static string GetLogCondition()
    {
#if UNITY_2017_1_OR_NEWER
        object rows = StartGettingEntries.Invoke(null, null);
#endif
        int row = (int)logListViewCurrentRow.GetValue(logListView);
        LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
        string condition = logEntryCondition.GetValue(logEntry) as string;
#if UNITY_2017_1_OR_NEWER
        EndGettingEntries.Invoke(null, null);
#endif
        return condition.Substring(0, condition.IndexOf('\n'));        
    }


    private static string GetListViewRowCount(string condition, ref int line)
    {  
        int index = condition.IndexOf(".lua:");

        if (index >= 0)
        {
            int start = condition.IndexOf("[");
            if (start <= 0) return null;
            int end = condition.IndexOf("]:");
            if (end <= 0) return null;
            string _line = condition.Substring(index + 5, end - index - 5);
            if (!Int32.TryParse(_line, out line)) return null;
            return condition.Substring(start + 1, index + 3 - start);
        }

        index = condition.IndexOf(".cs:");

        if (index >= 0)
        {
            int start = condition.IndexOf("[");
            if (start <= 0) return null;
            int end = condition.IndexOf("]:");
            if (end <= 0) return null;
            string _line = condition.Substring(index + 4, end - index - 4);
            if (!Int32.TryParse(_line, out line)) return null;
            return condition.Substring(start + 1, index + 2 - start);
        }

        return null;
    }

    private static string GetErrorViewRowCount(string condition, ref int line)
    {
        int index = condition.LastIndexOf(".cs");

        if (index >= 0)
        {
            //LuaException: file.cs:7: message
            int fileNameEnd = condition.IndexOf(':', index);
            if (fileNameEnd <= 0 || fileNameEnd != index + 3) return null;
            int fileNameStart = condition.LastIndexOf(": ", index);
            if (fileNameStart <= 0) return null;

            int lineStart = fileNameEnd;
            int lineEnd = condition.IndexOf(": ", lineStart);
            if (lineEnd <= 0) return null;
            string _line = condition.Substring(lineStart + 1, lineEnd - lineStart - 1);
            if (!Int32.TryParse(_line, out line)) return null;
            return condition.Substring(fileNameStart + 2, fileNameEnd - (fileNameStart + 2));
        }

        index = condition.LastIndexOf(".lua:");

        if (index >= 0)
        {
            /*int fileNameStart = condition.LastIndexOf(": [");

            if (fileNameStart > 0)
            {
                int fileNameEnd = index + 4;

                int lineEnd = condition.IndexOf("]:", fileNameEnd + 1);
                string _line = condition.Substring(fileNameEnd + 1, lineEnd - fileNameEnd - 1);
                if (!Int32.TryParse(_line, out line)) return null;
                return condition.Substring(fileNameStart + 3, index + 1 - fileNameStart).Trim();
            }
            else
            {*/
                int fileNameStart = condition.LastIndexOf(": ", index);
                if (fileNameStart <= 0) return null;
                int fileNameEnd = index + 4;
                int lineEnd = condition.IndexOf(":", fileNameEnd + 1);
                if (lineEnd <= 0) return null;
                string _line = condition.Substring(fileNameEnd + 1, lineEnd - fileNameEnd - 1);
                if (!Int32.TryParse(_line, out line)) return null;
                return condition.Substring(fileNameStart + 1, index + 3 - fileNameStart).Trim();
            //}
        }
        else
        {
            int lineEnd = condition.LastIndexOf(": ");
            if (lineEnd <= 0) return null;
            int fileNameEnd = condition.LastIndexOf(":", lineEnd - 1);
            if (fileNameEnd <= 0) return null;
            int fileNameStart = condition.LastIndexOf(":", fileNameEnd - 1);
            if (fileNameStart <= 0) return null;                        
            string _line = condition.Substring(fileNameEnd + 1, lineEnd - fileNameEnd - 1);
            if (!Int32.TryParse(_line, out line)) return null;
            string fileName = condition.Substring(fileNameStart + 1, fileNameEnd - fileNameStart - 1);
            fileName += ".lua";                     
            return fileName.Trim();
        }        
    }

    static void GetToLuaInstanceID()
    {
        if (toluaFileID == -1)
        {
            int start = LuaConst.toluaDir.IndexOf("Assets");
            int end = LuaConst.toluaDir.LastIndexOf("/Lua");
            string dir = LuaConst.toluaDir.Substring(start, end - start);
            dir += "/Core/ToLua.cs";
            toluaFileID = AssetDatabase.LoadAssetAtPath(dir, typeof(MonoScript)).GetInstanceID();//"Assets/ToLua/Core/ToLua.cs"
        }

        if (luaStateFileID == -1)
        {
            int start = LuaConst.toluaDir.IndexOf("Assets");
            int end = LuaConst.toluaDir.LastIndexOf("/Lua");
            string dir = LuaConst.toluaDir.Substring(start, end - start);
            dir += "/Core/LuaState.cs";
            luaStateFileID = AssetDatabase.LoadAssetAtPath(dir, typeof(MonoScript)).GetInstanceID();//"Assets/ToLua/Core/LuaState.cs"
        }

        if (luaDllFileID == -1)
        {
            int start = LuaConst.toluaDir.IndexOf("Assets");
            int end = LuaConst.toluaDir.LastIndexOf("/Lua");
            string dir = LuaConst.toluaDir.Substring(start, end - start);
            dir += "/Core/LuaDLL.cs";
            luaDllFileID = AssetDatabase.LoadAssetAtPath(dir, typeof(MonoScript)).GetInstanceID();//"Assets/ToLua/Core/LuaState.cs"            
        }
    }

    static bool OpenLuaFile(string fileName, int line)
    {
        if (fileName.EndsWith(".cs"))
        {
            string filter = fileName.Substring(0, fileName.Length - 3);
            filter += " t:MonoScript";
            string[] searchPaths = AssetDatabase.FindAssets(filter);

            for (int i = 0; i < searchPaths.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);

                if (path.EndsWith(fileName))
                {
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    lastObject = obj;
                    AssetDatabase.OpenAsset(obj, line);                    
                    return true;
                }
            }
        }
        else
        {
            string filter = fileName.Substring(0, fileName.Length - 4);
            int index = filter.IndexOf("/");
            if (index > 0)
            {
                filter = filter.Substring(index + 1);
            }
            string[] searchPaths = AssetDatabase.FindAssets(filter);

            for (int i = 0; i < searchPaths.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);
                string ext = fileName + ".bytes";

                if (path.EndsWith(fileName) || path.EndsWith(ext))
                {
                    UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
                    lastObject = obj;
#if !UNITY_2017_1_OR_NEWER                    
                    EditorApplication.delayCall += () => { AssetDatabase.OpenAsset(obj, line); };
#else
                    AssetDatabase.OpenAsset(obj, line);
#endif
                    return true;
                }
            }
        }

        lastObject = null;
        return false;
    }        

    [OnOpenAssetAttribute(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        GetToLuaInstanceID();        

        if (!GetConsoleWindowListView() || (object)EditorWindow.focusedWindow != consoleWindow)
        {
            lastObject = null;
            return false;
        }

        if (instanceID == toluaFileID && line == CustomSettings.PRINTLOGLINE)
        {
            string condition = GetLogCondition();
            string fileName = GetListViewRowCount(condition, ref line);

            if (fileName == null)
            {
                lastObject = null;
                return false;
            }

            return OpenLuaFile(fileName, line);
        }
        else if ((instanceID == luaStateFileID && line == CustomSettings.PCALLERRORLINE) ||
                (instanceID == luaDllFileID && line == CustomSettings.LUADLLERRORLINE))
        {
            string condition = GetLogCondition();
            string fileName = GetErrorViewRowCount(condition, ref line);

            if (fileName == null)
            {
                lastObject = null;
                return false;
            }

            return OpenLuaFile(fileName, line);
        }
        else
        {
            UnityEngine.Object script = EditorUtility.InstanceIDToObject(instanceID);            

            if (object.ReferenceEquals(script, lastObject))
            {
                return false;
            }

            string condition = GetLogCondition();

            if (string.Compare(condition, 0, LuaExceptionHeader, 0, LuaExceptionHeader.Length) == 0)
            {
                string fileName = GetErrorViewRowCount(condition, ref line);

                if (fileName == null)
                {
                    lastObject = null;
                    return false;
                }

                return OpenLuaFile(fileName, line);
            }
        }

        lastObject = null;
        return false;
    }
}
