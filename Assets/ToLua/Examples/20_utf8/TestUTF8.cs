using UnityEngine;
using LuaInterface;

public class TestUTF8 : LuaClient
{
    string script =
@"
    local utf8 = utf8

    function Test()
	    local l1 = utf8.len('你好')
        local l2 = utf8.len('こんにちは')
        print('chinese string len is: '..l1..' japanese len: '..l2)     

        local s = '遍历字符串'                                        

        for i in utf8.byte_indices(s) do            
            local next = utf8.next(s, i)                   
            print(s:sub(i, next and next -1))
        end   

        local len = utf8.len(s)                               

        for i = 2, len + 1 do
            print(utf8.sub(s, 1, i)..'...')        
        end

        local s1 = '天下风云出我辈'        
        print('风云 count is: '..utf8.count(s1, '风云'))
        s1 = s1:gsub('风云', '風雲')

        local function replace(s, i, j, repl_char)            
	        if s:sub(i, j) == '辈' then
		        return repl_char            
	        end
        end

        print(utf8.replace(s1, replace, '輩'))
    end
";

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    //屏蔽，例子不需要运行
    protected override void CallMain() { }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("Test");
        func.Call();
        func.Dispose();
        func = null;
    }
}
