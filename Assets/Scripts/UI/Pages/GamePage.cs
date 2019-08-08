
public class GamePage : BasePage
{
    public override void Open()
    {
    }

    public override void Close()
    {
    }

    public void LooseButtonOnClick()
    {
        PageManager.Open<AfterGamePage>();
    }
}
