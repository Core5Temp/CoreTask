
public class AfterGamePage : BasePage
{
    public override void Open()
    {
    }

    public override void Close()
    {
    }

    public void OkOnClick()
    {
        PageManager.Open<TryAgainPage>();
    }
}
