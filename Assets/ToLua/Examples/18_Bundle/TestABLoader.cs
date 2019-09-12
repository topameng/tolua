using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

//click Lua/Build lua bundle
public class TestABLoader : MonoBehaviour
{
    int bundleCount = int.MaxValue;
    string tips = null;

    IEnumerator CoLoadBundle(string name, string path)
    {
#if UNITY_4_6 || UNITY_4_7
        using (WWW www = new WWW(path))
        {
            if (www == null)
            {
                Debugger.LogError(name + " bundle not exists");
                yield break;
            }

            yield return www;

            if (www.error != null)
            {
                Debugger.LogError(string.Format("Read {0} failed: {1}", path, www.error));
                yield break;
            }

            --bundleCount;
            LuaFileUtils.Instance.AddSearchBundle(name, www.assetBundle);
            www.Dispose();
        }  
#else
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;

        --bundleCount;
        LuaFileUtils.Instance.AddSearchBundle(name, request.assetBundle);
#endif        
    }

    IEnumerator LoadFinished()
    {
        while (bundleCount > 0)
        {
            yield return null;
        }

        OnBundleLoad();
    }

    public IEnumerator LoadBundles()
    {
        string streamingPath = Application.streamingAssetsPath.Replace('\\', '/');
        string dir = streamingPath + "/" + LuaConst.osDir;

#if UNITY_EDITOR
        if (!Directory.Exists(dir))
        {
            throw new Exception("must build bundle files first");
        }
#endif

#if UNITY_4_6 || UNITY_4_7
        //此处应该配表获取
        List<string> list = new List<string>() { "lua.unity3d", "lua_cjson.unity3d", "lua_system.unity3d", "lua_unityengine.unity3d", "lua_protobuf.unity3d", "lua_misc.unity3d", "lua_socket.unity3d", "lua_system_reflection.unity3d" };   
#else       
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(dir + "/" + LuaConst.osDir);
        yield return request;

        AssetBundleManifest manifest = (AssetBundleManifest)request.assetBundle.LoadAsset("AssetBundleManifest");
        List<string> list = new List<string>(manifest.GetAllAssetBundles());
#endif

        bundleCount = list.Count;

        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i];

#if (UNITY_4_6 || UNITY_4_7) && UNITY_EDITOR
            string path = "file:///" + streamingPath + "/" + LuaConst.osDir + "/" + str;
#else
            string path = streamingPath + "/" + LuaConst.osDir + "/" + str;
#endif
            string name = Path.GetFileNameWithoutExtension(str);
            StartCoroutine(CoLoadBundle(name, path));
        }

        yield return StartCoroutine(LoadFinished());
    }

    void Awake()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(ShowTips);
#else
        Application.logMessageReceived += ShowTips;
#endif
        LuaFileUtils file = new LuaFileUtils();
        file.beZip = true;
#if UNITY_ANDROID && UNITY_EDITOR
        if (IntPtr.Size == 8)
        {
            throw new Exception("can't run this on standalone 64 bits, switch to pc platform, or run it in android mobile");
        }
#endif

        StartCoroutine(LoadBundles());
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300), tips);
    }

    void OnApplicationQuit()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(null);

#else
        Application.logMessageReceived -= ShowTips;
#endif
    }

    void OnBundleLoad()
    {
        LuaState state = new LuaState();
        state.Start();
        state.DoString("print('hello tolua#:'..tostring(Vector3.zero))", "TestABLoader.cs");
        state.Require("Main");
        LuaFunction func = state.GetFunction("Main");
        func.Call();
        func.Dispose();
        state.Dispose();
        state = null;
    }
}
