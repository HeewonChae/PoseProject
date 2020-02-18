// 디펜던시 클래스 등록
[assembly: Dependency(typeof(customClass))]

// 커스텀 렌더러 등록
[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_UWP))] // CustomView의 렌더러로 CustomRenderer_UWP를 사용하겠다.. 라는 뜻