
public class LeaderBoardPage : BasePage
{
    public override void Open()
    {
    }

    public override void Close()
    {
    }

    public void BackButtonOnClick()
    {
        PageManager.Open<MainMenuPage>();
    }
}
