using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

public class TestEventListener : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void OnClick(GameObject go);    
    public OnClick onClick = delegate { };

    public event OnClick onClickEvent = delegate { };

    public void SetOnFinished(OnClick click)
    {
        Debugger.Log("SetOnFinished OnClick");
    }
    
    public void SetOnFinished(VoidDelegate click)
    {
        Debugger.Log("SetOnFinished VoidDelegate");
    }

    [NoToLuaAttribute]
    public void OnClickEvent(GameObject go)
    {
        onClickEvent(go);
    }
}
