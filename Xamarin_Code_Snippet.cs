// 디펜던시 클래스 등록
[assembly: Dependency(typeof(customClass))]

// 커스텀 렌더러 등록
[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_AOS))] // CustomView의 렌더러로 CustomRenderer_UWP를 사용하겠다.. 라는 뜻

// Acr.UserDialogs
UserDialogs.Instance

// redirect uri 
http://www.facebook.com/connect/login_success.html

public class CustomSearchHandler : SearchHandler
{
    public MyPageSearchHandler()
    {
        SearchBoxVisibility = SearchBoxVisibility.Collapsible;
        IsSearchEnabled = true;
    }

    protected override void OnQueryConfirmed()
    {
    }

    protected override void OnQueryChanged(string oldValue, string newValue)
    {
        // Do nothing, we will wait for confirmation
    }
}
// page constructors
Shell.SetSearchHandler(this, new CustomSearchHandler());