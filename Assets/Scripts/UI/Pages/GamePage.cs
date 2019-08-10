
using UnityEngine;
using UnityEngine.UI;

public class GamePage : BasePage
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _timerText;

    private void Start()
    {
        GameController.ScoreChanged += (score) => _scoreText.text = $"Score:{score}";
        GameController.GameTimeChanged += (time) => _timerText.text = $"Time:{time}";
    }

    public override void Open()
    {
        GameController.GameStarted = true;
    }

    public override void Close()
    {
    }
}
