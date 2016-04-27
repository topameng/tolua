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

    public void LoadBundles()
    {
        //此处应该配表获取
        List<string> list = new List<string>() { "Lua.unity3d", "Lua_cjson.unity3d", "Lua_System.unity3d", "Lua_UnityEngine.unity3d", "Lua_protobuf.unity3d", "Lua_misc.unity3d", "Lua_socket.unity3d", "Lua_System_Reflection.unity3d" };        
        string streamingPath = Application.streamingAssetsPath.Replace('\\', '/');

        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i];            
            string path = "file:///" + streamingPath + "/" + LuaConst.osDir + "/" + str;
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

        state.DoString("print('hello tolua#:'..tostring(Vector3.zero))");

        state.Dispose();
        state = null;
	}	
}
