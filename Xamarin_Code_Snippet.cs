// 1. 디펜던시 클래스 등록
[assembly: Dependency(typeof(customClass))] // Android, IOS 프로젝트 경우
DependencyService.Register<MockDataStore>(); // 공유 프로젝트 경우

// 2. 커스텀 렌더러 등록
[assembly: ExportRenderer(typeof(CustomView), typeof(CustomRenderer_AOS))] // CustomView의 렌더러로 CustomRenderer_UWP를 사용하겠다.. 라는 뜻

// 3. Extern module
// Acr.UserDialogs
UserDialogs.Instance
// Shiny
ShinyHost.Resolve

// 4. SearchHandler
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

// 5. MessagingCenter
MessagingCenter.Send(this, "AddItem", Item);
MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
{
    var newItem = item as Item;
    Items.Add(newItem);
    await DataStore.AddItemAsync(newItem);
});