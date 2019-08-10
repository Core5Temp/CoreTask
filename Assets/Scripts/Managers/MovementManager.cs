using System.Collections.Generic;

using UnityEngine;

public static class MovementManager
{
    private static readonly List<Enemy> Enemies = new List<Enemy>();
    
    private static float _playerSpeed;
    private static float _playerSpeedModificator;
    private static bool _isGameStarted;

    private static Player _player;
    private static Vector2 _playerTargetPosition;

    public static void Init()
    {
        ApplicationObserver.MonoBehaviorUpdate += MonoBehaviorUpdate;
        GameController.GameStartedChanged += GameStartedChanged;
        GameController.PlayerChanged += PlayerChanged;
        InputManager.PlayerTargetPositionChanged += PlayerTargetPositionChanged;
        EnemySpawner.EnemySpawned += EnemySpawned;
        EnemySpawner.EnemyReleased += EnemyReleased;

        BoosterController.SpeedModificatorChanged += (speedModificator) => _playerSpeedModificator = speedModificator;

        _playerSpeedModificator = BoosterController.SpeedModificator;
        _playerSpeed = Settings.PlayerSpeed;

        GameStartedChanged(GameController.GameStarted);
        PlayerChanged(GameController.Player);
    }

    private static void GameStartedChanged(bool isStarted)
    {
        _isGameStarted = isStarted;
        _playerSpeedModificator = 1f;
    }

    private static void PlayerChanged(Player player)
    {
        _player = player;
        _playerTargetPosition = Vector2.zero;
    }

    private static void PlayerTargetPositionChanged(Vector2 targetPosition)
    {
        _playerTargetPosition = targetPosition;
    }

    private static void MonoBehaviorUpdate(float timeDeltaTime)
    {
        if (!_isGameStarted)
            return;

        if (((Vector2) _player.transform.position - _playerTargetPosition).sqrMagnitude > 0.4f)
        {
            var playerSpeed = _playerSpeed * _playerSpeedModificator;
            Move(_player.Transform, ref timeDeltaTime, ref playerSpeed);
            Rotate(_player.Transform, ref timeDeltaTime, ref playerSpeed, ref _playerTargetPosition);
        }

        UpdateEnemy(ref timeDeltaTime);
    }

    private static void Move(Transform moveTransform, ref float timeDeltaTime, ref float speed)
    {
        moveTransform.position += timeDeltaTime * speed * moveTransform.right;
    }

    private static void Rotate(Transform rotateTransform, ref float timeDeltaTime, ref float speed, ref Vector2 target)
    {
        var difference = target - (Vector2) rotateTransform.position;
        difference.Normalize();

        var rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var quaternion = Quaternion.AngleAxis(rotation, Vector3.forward);
        rotateTransform.rotation = Quaternion.Slerp(rotateTransform.rotation,
            quaternion, timeDeltaTime * speed);
    }

    private static void UpdateEnemy(ref float timeDeltaTime)
    {
        for (var i = 0; i < Enemies.Count; i++)
        {
            var enemy = Enemies[i];
            var enemySpeed = enemy.Speed;
            Move(enemy.Transform, ref timeDeltaTime, ref enemySpeed);

            var transformPosition = (Vector2) _player.Transform.position;
            Rotate(enemy.Transform, ref timeDeltaTime, ref enemySpeed, ref transformPosition);
        }
    }

    private static void EnemySpawned(Enemy enemy) =>  Enemies.Add(enemy);

    private static void EnemyReleased(Enemy enemy) =>  Enemies.Remove(enemy);
}