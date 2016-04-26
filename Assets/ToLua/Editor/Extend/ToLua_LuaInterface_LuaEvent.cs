using System;
using LuaInterface;

public class ToLua_LuaInterface_LuaEvent
{
    public static string AddEventHandlerDefined =
@"		try
		{			
			LuaInterface.LuaEvent obj = (LuaInterface.LuaEvent)ToLua.CheckObject(L, 1, typeof(LuaInterface.LuaEvent));            
            obj.AddEventHandler(L);						
            return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    public static string RemoveEventHandlerDefined =
@"		try
		{			
            LuaInterface.LuaEvent obj = (LuaInterface.LuaEvent)ToLua.CheckObject(L, 1, typeof(LuaInterface.LuaEvent));            
            obj.RemoveEventHandler(L);
            return 0;
        }
        catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    [UseDefinedAttribute]
    public int RemoveEventHandler(IntPtr L)
    {
        return 0;
    }

    [UseDefinedAttribute]
    public int AddEventHandler(IntPtr L)
    {
        return 0;
    }
}
