using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;

public class TestABLoader : MonoBehaviour 
{
    int bundleCount = 5;

    IEnumerator CoLoadBundle(string name, string path)
    {
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

            LuaFileUtils.Instance.AddSearchBundle(name, www.assetBundle);
            www.Dispose();
        }

        --bundleCount;
    }

    IEnumerator LoadFinished()
    {
        if (bundleCount > 0)
        {
            yield return null;
        }

        OnBundleLoad();
    }

#if UNITY_IPHONE
	static string remoteFolder = "iOS/";
#elif UNITY_ANDROID
	static string remoteFolder = "Android/";
#else
    static string remoteFolder = "Win/";
#endif

    public void LoadBundles()
    {
        List<string> list = new List<string>() { "Lua.unity3d", "Lua_math.unity3d", "Lua_system.unity3d", "Lua_u3d.unity3d", "Lua_protobuf.unity3d" };        
        string streamingPath = Application.streamingAssetsPath.Replace('\\', '/');

        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i];            
            string path = "file:///" + streamingPath + "/" + remoteFolder + str;
            string name = Path.GetFileNameWithoutExtension(str);
            StartCoroutine(CoLoadBundle(name, path));            
        }

        StartCoroutine(LoadFinished());
    }

    void Awake()
    {
        LuaFileUtils file = new LuaFileUtils();
        file.beZip = true;
        LoadBundles();
    }

    void OnBundleLoad()
    {
        LuaState state = new LuaState();
        state.Start();

        state.DoString("print('hello world')");

        state.Dispose();
        state = null;
	}	
}
