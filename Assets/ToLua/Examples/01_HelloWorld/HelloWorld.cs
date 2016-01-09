using UnityEngine;
using System.Collections;
using LuaInterface;


public class HelloWorld : MonoBehaviour 
{	
	void Start () 
    {        
        LuaState lua = new LuaState();
        string hello =
            @"                
                print('hello tolua, 广告招租')                
            ";

        lua.DoString(hello, "hello");
        lua.CheckTop();
        lua.Dispose();
	}
}
