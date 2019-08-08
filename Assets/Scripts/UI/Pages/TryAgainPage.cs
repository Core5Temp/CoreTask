
public class TryAgainPage : BasePage
{
    public override void Open()
    {
    }

    public override void Close()
    {
    }

    public void TryAgainButtonOnClick()
    {
        PageManager.Open<GamePage>();
    }

    public void MainMenuButtonOnClick()
    {
        PageManager.Open<MainMenuPage>();
    }
}
