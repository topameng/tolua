using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using LuaInterface;
using System.Collections;

public class LuaHotLoader : MonoBehaviour
{
    public string ScriptPath;
    public string Pattern;

    private Regex regex;
    private Dictionary<string, DateTime> fileInfo;
    private LuaState luaState;
    public LuaState LuaState
    {
        private get { return luaState; }
        set
        {
            if (luaState == value)
                return;
            luaState = value;
            luaState.OpenLibs(LuaDLL.luaopen_hot);
            luaState.DoString(reload);
        }
    }
    public string FullScriptPath
    {
        get
        {
            if (string.IsNullOrEmpty(ScriptPath))
                return Application.dataPath;
            else if (Path.IsPathRooted(ScriptPath) == false)
                return Path.Combine(Application.dataPath, ScriptPath);
            return ScriptPath;
        }
    }

    void Start()
    {
        regex = new Regex(Pattern, RegexOptions.Compiled);
         var files = new DirectoryInfo(FullScriptPath).GetFiles("*.lua", SearchOption.AllDirectories);
        fileInfo = new Dictionary<string, DateTime>();
        foreach (var file in files)
        {
            if (regex.IsMatch(file.FullName))
            {
                fileInfo.Add(file.FullName, file.LastWriteTime);
            }
        }
    }

    public void CheckAndReload()
    {
        if (LuaState == null)
            return;

        var files = new DirectoryInfo(FullScriptPath).GetFiles("*.lua", SearchOption.AllDirectories);
        var reloadList = new List<string>();
        foreach (var file in files)
        {
            if (regex.IsMatch(file.FullName) == false)
                continue;

            if (fileInfo.ContainsKey(file.FullName) == false)
            {
                fileInfo.Add(file.FullName, file.LastWriteTime);
            }
            else if (fileInfo[file.FullName] != file.LastWriteTime)
            {
                reloadList.Add(file.FullName);
                fileInfo[file.FullName] = file.LastWriteTime;
            }
        }
        if (reloadList.Count > 0)
            Reload(reloadList);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
            return;
        CheckAndReload();
    }

    private void Reload(List<string> reloadList)
    {
        var moduleList = new List<string>();
        foreach (var fullname in reloadList)
        {
            var moduleFileName = fullname.Substring(FullScriptPath.Length).Replace("/", ".");
            moduleFileName = moduleFileName.Substring(0, moduleFileName.Length - 4);
            moduleList.Add(moduleFileName);
        }

        LuaState.DoString(string.Format("hotreload('{0}')", string.Join("', '", moduleList)));
    }

    private readonly string reload = @"
function hotreload(...)
    local cache = {}
    for i, moduleName in ipairs({...}) do
        cache[moduleName] = require(moduleName)
        if type(cache[moduleName]) == 'string' then
            cache[moduleName] = _G[cache[moduleName]]
        end
        package.loaded[moduleName] = nil
    end
    for i, moduleName in ipairs({...}) do
        local ret = require(moduleName)
        local new = ret
        if type(ret) == 'string' then
            new = _G[ret]
        end
        if type(new) == 'table' then
            local old = cache[moduleName]
            for k, v in pairs(new) do
                if type(v) == 'function' then
                    if old[k] == nil then
                        old[k] = v
                    else
                        hot.swaplfunc(old[k], v)
                    end
                end
            end
            package.loaded[moduleName] = old
            if type(ret) == 'string' then
                _G[ret] = old
            end
        end
    end
end
";
}
