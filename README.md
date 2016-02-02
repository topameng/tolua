#tolua*#*

tolua# is an unity lua static binder solution. 

tolua# is an unity plugin that greatly simplifies the integration of C# code with Lua. can automatically generates the binding code to access unity from Lua. it maps c# constants,  variables, functions, properties, classes, and methods to Lua.

tolua# grows up from cstolua.  tolua#'s goal is to be a powerful development environment for unity.

support unity4.6.x and unity5.x all(in unity5.x, delete Plugins/x86 or Plugins/x86_64)

 **if you want to test in mobile, first click menu Lua/Copy lua files to Resources. then build it**

 **如果你想在手机上测试，首先点击菜单Lua/Copy lua files to Resources， 之后再build.**

#Library

**Debugger** <br>
https://github.com/topameng/Debugger

**tolua_runtime** <br>
https://github.com/topameng/tolua_runtime

**protoc-gen-lua** <br>
https://github.com/topameng/protoc-gen-lua

**luapb**<br>
https://github.com/Neopallium/lua-pb<br>
支持luapb可自行整合.未放入插件内

#Packages
　**Basics**　　　　　　　　**Math**　　　　　　**Data Structures**<br>
　luabitop　　　　　　　Quaternion　　　　　　　list<br>
　 struct　　　　　　　 　Vector3　　　　　　　　event<br>
　 int64　　　　 　　　  　Vector4　　　　　　　　slot<br>
　 Time　　　　 　　　  　Vector2<br>
**Networking**　　　　 　　　Ray<br>
　luasocket　　　　 　　　 Color<br>
　**Parsing**　　　　 　　　Bounds<br>
　lpeg　　 　　 　　　 　  　Mathf<br>
　**Protol**　　　　　 　 　　 Touch<br>
　pblua　　　 　　 　 　RaycastHit<br>
　
#Performance

#Examples

#About Lua
win, android using luajit2.0.4. macos using luac(for u5.x). ios using luajit2.1beta
