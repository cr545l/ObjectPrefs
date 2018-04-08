# Object Preference

ObjectPrefs는 UnityPrefs를 개선한 클래스로써 더 다양한 타입의 데이터를 저장/불러올 수 있도록 인터페이스를 제공합니다.

`level`이란 키로 `int 타입`의 값 `10`을 저장한다면 아래와 같습니다.

``` csharp
ObjectPrefs.Set( "level", 10 );
ObjectPrefs.Save();
```

`level`을 불러오고 싶다면 타입만 정의해주면 정상적으로 불러올 수 있습니다.

``` csharp
int level = ObjectPrefs.Get<int>( "level" );
```