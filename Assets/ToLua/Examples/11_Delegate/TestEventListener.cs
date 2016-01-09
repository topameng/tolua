using UnityEngine;
using System;
using System.Collections;

public class TestEventListener : MonoBehaviour
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void OnClick(GameObject go);

    public OnClick onClick = delegate { };    

    public void SetOnFinished(OnClick click)
    {
        Debugger.Log("SetOnFinished OnClick");
    }

    //如何应对重载
    public void SetOnFinished(VoidDelegate click)
    {
        Debugger.Log("SetOnFinished VoidDelegate");
    }
}
