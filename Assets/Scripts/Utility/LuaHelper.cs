using UnityEngine;
using System.Collections;

public class LuaHelper
{
    public static void AddScrollRectHandler(GameObject go, Aciton<GameObject, Vector2> callback)
    {
        if (null == go || null == callback)
            return;
        var rect = go.GetComponent<UnityEngine.UI.ScrollRect>();
        if(null == rect)
        {
            Debug.LogError("未找到ScrollRect" + go.name)
            return;
        }
        rect.onValueChanged.AddListener((normal)=>{callback(go, normal);});
    }

    public static void RemoveScrollRectHandler(GameObject go)
    {
        if(null == go)
        {
            return ;
        }
        var rect = go.GetComponent<UnityEngine.UI.ScrollRect>();
        rect.onValueChanged.RemoveAllListeners();
    }
}
