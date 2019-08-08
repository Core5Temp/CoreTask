using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
