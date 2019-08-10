using System.IO;
using UnityEngine;

public class Settings : ScriptableObject
{
    [Header("Player")]
    [SerializeField] private float _playerSpeed = 10;

    [Header("Enemy spawner")]
    [SerializeField] private int _countEnemyFromSpawn = 3;
    [SerializeField] private int _spawnDelay = 2;
    [SerializeField] private float _enemySpeedUp = 2;
    [SerializeField] private int _speedUpDelay = 10;
    [SerializeField] private float _defEnemySpeed = 5;
    
    [Header("Booster spawner")]
    [SerializeField] private int _boosterSpawnDelay = 5;
    [SerializeField] private int _boosterDurations = 10;
    
    [Header("Score")]
    [SerializeField] private int _delayToUpScore = 5;
    [SerializeField] private int _upScoreCount = 10;

    public static float PlayerSpeed => Instance._playerSpeed;

    public static int DelayToUpScore => Instance._delayToUpScore;
    
    public static int UpScoreCount => Instance._upScoreCount;

    public static int SpawnDelay => Instance._spawnDelay;
    public static int CountEnemySpawn => Instance._countEnemyFromSpawn;

    public static float EnemySpeedUp => Instance._enemySpeedUp;
    
    public static int SpeedupDelay => Instance._speedUpDelay;

    public static float DefEnemySpeed => Instance._defEnemySpeed;

    public static int BoosterDurations => Instance._boosterDurations;
    
    public static int BoosterSpawnDelay => Instance._boosterSpawnDelay;

    private static Settings _instance;

    private const string Path = "Assets/Resources/ScriptableObjects";

    public static Settings Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = Resources.Load<Settings>("ScriptableObjects/Settings");

            if (_instance != null)
                return _instance;

            _instance = CreateInstance<Settings>();

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            UnityEditor.AssetDatabase.CreateAsset(_instance, $"{Path}/Settings.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return _instance;
        }
    }

    [UnityEditor.MenuItem("TestGame/Settings")]
    private static void Show()
    {
        UnityEditor.Selection.activeObject = Instance;
    }
}