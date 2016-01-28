/*    
 * Copyright (c) 2015.10 , 蒙占志 (Zhanzhi Meng) topameng@gmail.com
 * Use, modification and distribution are subject to the "MIT License"
 * (bezip = false)在search path 中查找读取lua文件。
 * (bezip = true)可以从外部设置过来bundel文件中读取lua文件。
*/

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;


public class LuaFileUtils
{
    public static LuaFileUtils Instance
    {
        get
        {
            if (instance == null)
            {
                Instance = new LuaFileUtils();
            }

            return instance;
        }

        protected set
        {
            instance = value;
        }
    }

    public bool beZip = false;    
    protected List<string> searchPaths = new List<string>();      
    protected Dictionary<string, AssetBundle> zipMap = new Dictionary<string, AssetBundle>();

    protected static LuaFileUtils instance = null;

    public LuaFileUtils()
    {
        instance = this;    
    }

    public virtual void Dispose()
    {
        if (instance != null)
        {
            instance = null;
            searchPaths.Clear();

            foreach (KeyValuePair<string, AssetBundle> iter in zipMap)
            {
                iter.Value.Unload(true);
            }

            zipMap.Clear();
        }
    }

    public void AddSearchPath(string path, bool front = false)
    {
        if (path.Length > 0 && path[path.Length - 1] != '/')
        {
            path += "/";
        }        

        if (front)
        {
            searchPaths.Insert(0, path);
        }
        else
        {
            searchPaths.Add(path);
        }
    }

    public void RemoveSearchPath(string path)
    {
        if (path.Length > 0 && path[path.Length - 1] != '/')
        {
            path += "/";
        }

        int index = searchPaths.IndexOf(path);

        if (index >= 0)
        {
            searchPaths.RemoveAt(index);
        }
    }

    public void AddSearchBundle(string name, AssetBundle bundle)
    {
        zipMap[name] = bundle;
        Debugger.Log("Add Lua bundle: " + name);
    }    

    public string GetFullPathFileName(string fileName)
    {
        if (fileName == string.Empty)
        {
            return string.Empty;
        }

        if (Path.IsPathRooted(fileName))
        {
            return fileName;
        }

        string fullPath = null;

        for (int i = 0; i < searchPaths.Count; i++)
        {
            fullPath = Path.Combine(searchPaths[i], fileName);

            if (File.Exists(fullPath))
            {                    
                return fullPath;
            }            
        }
        
        return null;
    }

    public virtual byte[] ReadFile(string fileName)
    {
        if (!beZip)
        {
            string path = GetFullPathFileName(fileName);
            byte[] str = null;

            if (File.Exists(path))
            {
                str = File.ReadAllBytes(path);
            }

            return str;
        }
        else
        {
            return ReadZipFile(fileName);
        }  
    }

    byte[] ReadZipFile(string fileName)
    {
        AssetBundle zipFile = null;
        byte[] buffer = null;
        string zipName = "Lua";
        int pos = fileName.LastIndexOf('/');

        if (pos > 0)
        {
            zipName = fileName.Substring(0, pos);
            zipName.Replace('/', '_');
            zipName = string.Format("Lua_{0}", zipName);            
            fileName = fileName.Substring(pos + 1);
        }

        zipMap.TryGetValue(zipName, out zipFile);        

        if (zipFile != null)
        {
#if UNITY_5
            TextAsset luaCode = zipFile.LoadAsset<TextAsset>(fileName);
#else
            TextAsset luaCode = zipFile.Load(fileName, typeof(TextAsset)) as TextAsset;
#endif            

            if (luaCode != null)
            {
                buffer = luaCode.bytes;
                Resources.UnloadAsset(luaCode);
            }
        }

        return buffer;
    }

    public static string GetOSDir()
    {
#if UNITY_STANDALONE
        return "Win";
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_IPHONE
        return "IOS";
#else
        return "";
#endif
    }
}
