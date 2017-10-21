## tolua#
tolua#的优点在此无需过度阐述，本修改版本只集中描述差异化的修改。<br>
源版本传送门：https://github.com/topameng/tolua<br>
本修改版本Plugins库传送门：https://github.com/NewbieGameCoder/tolua_runtime

# 关于Wrap速度的修改
   用过tolua#的可能经历过，Lua环境初始化的时候，LuaBinder类的Bind花费了大量时间（比如4s），自己需要添加额外的逻辑，分散C#相关模块wrap进lua环境的时机（当然tolua#已经集成了一个PreLoad的概念能解决大部分的问题，某个模块只有在被用到的时候，才会动态整个wrap进lua环境，注意是整个）。而本工程的修改则更进一步，相对于原版用一点点且是一次性的性能消耗，离散C#任意一个模块的函数的wrap时机、变量的wrap时机，使得C#的函数、变量只有在lua端访问的时候才会wrap进lua环境，从而较大的改善某一个时间段整体的wrap速度。目前我自己的工程在编辑器上能获得3.5倍提升，其他工程视项目而定，如果lua环境初始化时候，访问C#各个模块的频率没我自己项目的高，那么会获得更大的提升。<br>
   本条修改可以查看ToLuaExport类里面的enableLazyFeature开启流程，自定义自己的修改。目前是ToLuaMenu里面自动生成代码的时候，自动帮你开了。如果自己修改成完全不用这个特性，那么本修改版本就是原汁原味的ToLua#，我尽量做到不改经过时间验证的整体逻辑，包括runtime库也是新添函数，不改tolua#库已有的逻辑代码。<br>
