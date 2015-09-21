# BDiff

* [Source](bdiff.cs)

Usage
----
```c#
var f1 = BHash.Load("c:\\sample_android_3.apk");
var f2 = BHash.Load("c:\\sample_android_3.apk");

Console.WriteLine(f1.IsSame(f2));
```

각각의 BHash 인스턴스는 재사용을 위해 저장될 수 있습니다.
(프로그램을 껏다가 켰을 시 다시 파일을 열어서 재 해싱하는 코스트를 줄이기 위해)
```c#
예제 추가
```
