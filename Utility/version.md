注意:
.net版本 对应参考： https://blog.csdn.net/qq_20936333/article/details/81219166
https://docs.microsoft.com/zh-cn/dotnet/standard/frameworks#how-to-specify-target-frameworks
1.不支持mysql 没有驱动：.enterpriselibrary、ef netframework 
2.cs 模式 core 不支持：wcf 、remote 
3.nihernate 同一个 session cs 模式 下 修改后 再次查询 该条数据存在,但值都没, null or "" or  0,即默认值 bug
