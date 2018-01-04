﻿using System.Collections;
using UnityEngine;

public class BaseTest
{
    public virtual int TestRef(ref int count)
    {
        Debug.Log("CS:Base TestRef");
        ++count;

        return 1;
    }
}

public class ToLuaInjectionTest : BaseTest
{
    private int propertyTest;

    public ToLuaInjectionTest()
    {
        Debug.Log("CS:Constructor Test");
    }

    public ToLuaInjectionTest(bool state)
    {
        Debug.Log("CS:Constructor Test " + state);
    }

    public int PropertyTest
    {
        get
        {
            Debug.Log("CS:PropertyTestGet");
            return propertyTest;
        }
        set
        {
            Debug.Log("CS:PropertyTestSet");
            propertyTest = value;
        }
    }

    public override int TestRef(ref int count)
    {
        Debug.Log("CS:Override TestRef");
        ++count;

        return 2;
    }

    public void TestOverload(int param1, bool param2)
    {
        Debug.Log("CS:TestOverload");
    }

    public void TestOverload(int param1, ref bool param2)
    {
        Debug.Log("CS:TestOverload");
        param2 = !param2;
    }

    public void TestOverload(bool param1, int param2)
    {
        Debug.Log("CS:TestOverload");
    }

    public IEnumerator TestCoroutine(float delay)
    {
        Debug.Log("CS:TestCoroutine Run");
        yield return new WaitForSeconds(delay);
        Debug.Log("CS:TestCoroutine End");
    }
}