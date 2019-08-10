using System.Collections.Generic;

using UnityEngine;

public static class BoosterSpawner
{
    private const int CountSpawnBoosters = 3;

    private static readonly List<BaseBooster> BoostersInMap = new List<BaseBooster>();

    private static bool _gameStarted;

    private static int _defSpawnDelay;

    private static Camera _camera;

    private static float _verticalSize;
    private static float _horizontalSize;
    private static float _timeToSpawnBooster;

    private static Transform _boosterContainer;

    public static void Init()
    {
        _boosterContainer = ApplicationObserver.BoosterContainer;

        _defSpawnDelay = Settings.BoosterSpawnDelay;

        _camera = Camera.main;

        _verticalSize = _camera.orthographicSize;
        _horizontalSize = _verticalSize * Screen.width / Screen.height / 2f;

        GameController.GameStartedChanged += GameStartedChanged;
        GlobalTimer.Tick += Tick;
    }

    public static void ReleaseBooster(BaseBooster baseBooster)
    {
        PoolObjects.Release(baseBooster);
        BoostersInMap.Remove(baseBooster);
    }

    public static BaseBooster SpawnRandomBooster()
    {
        var booster = GetRandomBooster();
        booster.Transform.SetParent(_boosterContainer);
        booster.Transform.position = new Vector3(Random.Range(-_horizontalSize, _horizontalSize),
            Random.Range(-_verticalSize, _verticalSize));
        BoostersInMap.Add(booster);
        return booster;
    }

    private static void Tick(float value)
    {
        if (!_gameStarted)
            return;

        if (_timeToSpawnBooster < 0)
        {
            SpawnBoostersTimer();
            _timeToSpawnBooster = _defSpawnDelay;
            return;
        }

        _timeToSpawnBooster -= value;
    }

    private static void SpawnBoostersTimer()
    {
        for (var i = 0; i < CountSpawnBoosters; i++)
            SpawnRandomBooster();
    }

    private static BaseBooster GetRandomBooster()
    {
        var randomValue = Random.Range(1, 4);
        switch (randomValue)
        {
            case 1:
                return PoolObjects.Get<AddScore>();
            case 2:
                return PoolObjects.Get<SuperSpeed>();
            case 3:
                return PoolObjects.Get<GodMode>();
        }

        Debug.LogError("BoosterType not found " + randomValue);
        return PoolObjects.Get<AddScore>();
    }

    private static void GameStartedChanged(bool gameStarted)
    {
        _gameStarted = gameStarted;
        if (_gameStarted)
            _timeToSpawnBooster = _defSpawnDelay;
        else
            ReleaseAllBoosters();
    }

    private static void ReleaseAllBoosters()
    {
        for (var i = 0; i < BoostersInMap.Count; i++)
            PoolObjects.Release(BoostersInMap[i]);

        BoostersInMap.Clear();
    }
}