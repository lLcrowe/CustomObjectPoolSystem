# CustomObjectPoolSystem

Unity 컴포넌트 오브젝트 풀 + 순수 C# 클래스 풀을 함께 제공하는 메모리 최적화 모듈.

## 핵심 기능

- **Component 풀**: GameObject/Component 재사용 (프로젝타일, 이펙트 등)
- **Class 풀**: 순수 C# 클래스 재사용 (데이터 컨테이너, 임시 객체)
- **자동 회수**: `SetActive(false)` 호출 시 풀로 자동 반환

## 주요 클래스

| 클래스 | 역할 |
|--------|------|
| `ObjectPoolManager` | 싱글톤 중앙 관리자. 타입별 자동 풀 생성 |
| `CustomObjectPool<T>` | 컴포넌트용 Queue 기반 풀 |
| `CustomClassPool<T>` | 클래스용 List 기반 풀 |
| `CustomPoolTarget` | GameObject 비활성화 시 자동 풀 반환 마커 |
| `CustomClassPoolTarget` | 클래스 풀 회수 마커 (`isUse` 플래그) |

## 사용법

```csharp
// Component 풀 - 요청
var bullet = ObjectPoolManager.Instance.RequestDynamicComponentObject(bulletPrefab);

// Component 풀 - 반환 (자동)
bullet.gameObject.SetActive(false);

// Class 풀
var pool = new CustomClassPool<MyData>();
var data = pool.RequestClass();
data.SetIsUse(true);
// 사용 후
data.SetIsUse(false);
```

## 의존성

- `lLCroweTool.Singleton`
- `lLCroweTool.Dictionary`
