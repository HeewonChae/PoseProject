// 디펜던시 클래스 등록
[assembly: Dependency(typeof(customClass))]

// 커스텀 렌더러 등록
[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_UWP))] // CustomView의 렌더러로 CustomRenderer_UWP를 사용하겠다.. 라는 뜻

MainViewPage는 로딩용으로 사용하면 좋겠다.
Page 와 UserControl를 분리해서 만들자.
