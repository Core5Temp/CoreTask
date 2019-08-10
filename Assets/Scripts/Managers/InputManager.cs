using System;

using UnityEngine;

public static class InputManager
{
    private static bool _gameStarted;
    private static Camera _camera;

    public static event Action<Vector2> PlayerTargetPositionChanged;

    public static void Init()
    {
        GameController.GameStartedChanged += GameStartedChanged;
        ApplicationObserver.MonoBehaviorUpdate += MonoBehaviorUpdate;

        GameStartedChanged(GameController.GameStarted);

        _camera = Camera.main;
    }

    private static void GameStartedChanged(bool isStarted)
    {
        _gameStarted = isStarted;
    }

    private static void MonoBehaviorUpdate(float timeDeltaTime)
    {
        if (!_gameStarted)
            return;

        if (Input.GetMouseButtonDown(0))
            PlayerTargetPositionChanged?.Invoke(_camera.ScreenToWorldPoint(Input.mousePosition));
    }
}