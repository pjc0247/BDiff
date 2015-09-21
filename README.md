# BDiff

* [Source](bdiff.cs)

Usage
----
```c#
var f1 = BHash.Load("c:\\sample_android_3.apk");
var f2 = BHash.Load("c:\\sample_android_3.apk");

// f1과 f2에 차이가 있는 오프셋 리스트를 반환합니다.
var diffPoints = f1.FindDiff(f2);

// f1과 f2중 첫 번째로 차이가 있는 오프셋을 반환합니다.
var firstDiffPoint = f1.FindFirstDiffPoint(f2);

// f1과 f2가 가리키는 파일이 서로 같은지 비교합니다.
var isSame = f1.IsSame(f2);
```

각각의 BHash 인스턴스는 재사용을 위해 저장될 수 있습니다.<br>
(프로그램을 껏다가 켰을 시 다시 파일을 열어서 재 해싱하는 코스트를 줄이기 위해)
```c#
예제 추가
```

Flags
----
* 가능하다면 BHASH_USE_MEMCMP 플래그를 활성화 하여, 바이트 배열 비교에 byte[]를 사용할 수 있습니다.
