
public class MainMenuPage : BasePage, IStartPage
{
    public override void Open()
    {
    }

    public override void Close()
    {
    }

    public void PlayButtonOnClick()
    {
        PageManager.Open<GamePage>();
    }
    
    public void LeaderBoardButtonOnClick()
    {
        PageManager.Open<LeaderBoardPage>();
    }
}
