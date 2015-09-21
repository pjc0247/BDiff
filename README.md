# BDiff

* [Source](bdiff.cs)

Usage
----
```c#
var f1 = BHash.Load("c:\\sample_android_3.apk");
var f2 = BHash.Load("c:\\sample_android_3.apk");

Console.WriteLine(f1.IsSame(f2));
```
