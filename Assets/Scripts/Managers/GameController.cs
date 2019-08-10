using System;

using UnityEngine;

public static class GameController
{
    public static event Action<bool> GameStartedChanged;
    public static event Action<int> GameTimeChanged;
    public static event Action<int> ScoreChanged;
    public static event Action<Player> PlayerChanged;
    
    private static bool _gameStarted;
    
    private static int _gameTimer;
    private static int _score;
    private static int _delayToScoreUpDefValue;
    private static int _countScoreUpDefValue;
    
    private static float _toScoreUp;
    private static float _realTimeDelta;
    
    private static Player _player;
    private static Transform _playerContainer;
    
    public static bool GameStarted
    {
        get => _gameStarted;
        set
        {
            _gameStarted = value;

            if (_gameStarted)
                StartGame();
            else
                StopGame();
                    
            GameStartedChanged?.Invoke(_gameStarted);   
        }
    }

    public static int GameTimer
    {
        get => _gameTimer;
        set
        {
            _gameTimer = value; 
            GameTimeChanged?.Invoke(_gameTimer);
        }
    }

    public static int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreChanged?.Invoke(_score);
        }
    }

    public static Player Player
    {
        get => _player;
        set
        {
            _player = value;
            PlayerChanged?.Invoke(_player);
        }
    }

    public static void Init()
    {
        _playerContainer = ApplicationObserver.PlayerContainer;
        
        _countScoreUpDefValue = Settings.UpScoreCount;
        _delayToScoreUpDefValue = Settings.DelayToUpScore;
        
        GlobalTimer.Tick += TickTimer;
    }

    private static void TickTimer(float timeDelta)
    {
        if (!_gameStarted)
            return;

        ScoreUpTime(ref timeDelta);
        _realTimeDelta += timeDelta;
        GameTimer += (int) _realTimeDelta;
    }

    private static void ScoreUpTime(ref float timeDelta)
    {
        if (_toScoreUp < 0)
        {
            Score += _countScoreUpDefValue;
            _toScoreUp = _delayToScoreUpDefValue;
            return;
        }

        _toScoreUp -= timeDelta;
    }

    private static void StartGame()
    {
        _toScoreUp = _delayToScoreUpDefValue;
        
        Player = PoolObjects.Get<Player>();
        Player.Transform.SetParent(_playerContainer);
        Player.Transform.position = Vector3.zero;
        
        _realTimeDelta = 0;
        GameTimer = 0;
        Score = 0;
    }

    private static void StopGame()
    {
        PoolObjects.Release(Player);
        PageManager.Open<AfterGamePage>();
    }
}