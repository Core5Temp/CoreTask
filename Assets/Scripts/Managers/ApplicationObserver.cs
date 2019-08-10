using System;

using UnityEngine;

using Crystal;


public class ApplicationObserver : MonoBehaviour
{
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _playerContainer;
    [SerializeField] private Transform _boosterContainer;

    public static event Action<float> MonoBehaviorUpdate;
    
    public static Transform EnemyContainer => _instance._enemyContainer;
    
    public static Transform PlayerContainer => _instance._playerContainer;
    
    public static Transform BoosterContainer => _instance._boosterContainer;

    private static ApplicationObserver _instance;

    private void Awake()
    {
        _instance = this;

        PoolObjects.Instance.Init();
        InputManager.Init();
        MovementManager.Init();
        GameController.Init();
        GlobalTimer.Init();
        EnemySpawner.Init();
        BoosterSpawner.Init();
        BoosterController.Init();
    }

    public void Update()
    {
        MonoBehaviorUpdate?.Invoke(Time.deltaTime);

        SafeArea.Sim = Screen.width == 2436 && Screen.height == 1125
            ? SafeArea.SimDevice.iPhoneX
            : SafeArea.SimDevice.None;
    }
}