using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public static class EnemySpawner
{
    public static event Action<Enemy> EnemySpawned;
    public static event Action<Enemy> EnemyReleased;

    private static readonly List<Enemy> EnemyInGame = new List<Enemy>();

    private static float _timeToSpawn;
    private static float _timeToUpEnemySpeed;

    private static float _speedupDelay;
    private static float _enemySpeedUp;
    private static float _currentEnemySpeed;
    private static float _defEnemySpeed;

    private static int _spawnDelay;
    private static int _countEnemySpawn;

    private static bool _isGameStarted;

    private static Camera _camera;

    private static float _verticalSize;
    private static float _horizontalSize;

    private static Transform _enemyContainer;

    public static void Init()
    {
        _enemyContainer = ApplicationObserver.EnemyContainer;

        _camera = Camera.main;

        _verticalSize = _camera.orthographicSize;
        _horizontalSize = _verticalSize * Screen.width / Screen.height / 2f;

        _spawnDelay = Settings.SpawnDelay;
        _countEnemySpawn = Settings.CountEnemySpawn;
        _speedupDelay = Settings.SpeedupDelay;
        _defEnemySpeed = Settings.DefEnemySpeed;
        _enemySpeedUp = Settings.EnemySpeedUp;

        GameController.GameStartedChanged += GameStarted;

        GameStarted(GameController.GameStarted);
        GlobalTimer.Tick += Tick;
    }

    public static void ReleaseEnemy(Enemy enemy)
    {
        EnemyReleased?.Invoke(enemy);
        PoolObjects.Release(enemy);
        EnemyInGame.Remove(enemy);
    }

    private static Enemy SpawnEnemy()
    {
        var enemy = PoolObjects.Get<Enemy>();
        enemy.Transform.SetParent(_enemyContainer);
        enemy.Speed = _currentEnemySpeed;

        enemy.Transform.position = GetRandomPosition;
        EnemyInGame.Add(enemy);
        EnemySpawned?.Invoke(enemy);
        return enemy;
    }

    private static Vector3 GetRandomPosition =>
        new Vector3(Random.Range(-_horizontalSize, _horizontalSize), Random.Range(-_verticalSize, _verticalSize));

    private static void GameStarted(bool gameStarted)
    {
        _isGameStarted = gameStarted;

        if (gameStarted)
        {
            _timeToSpawn = _spawnDelay;
            _currentEnemySpeed = _defEnemySpeed;
            _timeToUpEnemySpeed = _speedupDelay;
        }
        else
            ReleaseEnemy();
    }

    private static void ReleaseEnemy()
    {
        for (var i = 0; i < EnemyInGame.Count; i++)
        {
            var enemy = EnemyInGame[i];
            EnemyReleased?.Invoke(enemy);
            PoolObjects.Release(enemy);
        }

        EnemyInGame.Clear();
    }

    private static void Tick(float tickValue)
    {
        if (!_isGameStarted)
            return;

        CheckEnemySpawnTimer(ref tickValue);
        CheckEnemySpeedUpTimer(ref tickValue);
    }

    private static void CheckEnemySpawnTimer(ref float tickValue)
    {
        if (_timeToSpawn < 0)
        {
            SpawnEnemyTimer();
            _timeToSpawn = _spawnDelay;
            return;
        }

        _timeToSpawn -= tickValue;
    }

    private static void CheckEnemySpeedUpTimer(ref float tickValue)
    {
        if (_timeToUpEnemySpeed < 0)
        {
            _currentEnemySpeed += _enemySpeedUp;
            _timeToUpEnemySpeed = _speedupDelay;
            return;
        }

        _timeToUpEnemySpeed -= tickValue;
    }

    private static void SpawnEnemyTimer()
    {
        for (var i = 0; i < _countEnemySpawn; i++)
            SpawnEnemy();
    }
}