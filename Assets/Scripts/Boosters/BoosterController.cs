using System;

public static class BoosterController
{
    public static event Action<float> SpeedModificatorChanged;
    public static event Action<bool> GodModeBoosterChanged;

    private const int AddScoreBoosterValue = 10;
    private const float BaseSpeedModificator = 1.5f;

    private static bool _godMoveBoosterActive;

    private static bool _gameStarted;
    private static bool _speedBoosterActive;

    private static float _speedBoosterDuration;
    private static float _godModeBoosterDuration;

    private static int _boosterDurations;

    public static float SpeedModificator => SpeedBoosterActive ? BaseSpeedModificator : 1f;

    public static bool GodMoveBoosterActive
    {
        get => _godMoveBoosterActive;
        private set
        {
            _godMoveBoosterActive = value;
            GodModeBoosterChanged?.Invoke(_godMoveBoosterActive);
        }
    }

    public static bool SpeedBoosterActive
    {
        get => _speedBoosterActive;
        private set
        {
            _speedBoosterActive = value;
            SpeedModificatorChanged?.Invoke(SpeedModificator);
        }
    }
    
    public static void Init()
    {
        _boosterDurations = Settings.BoosterDurations;

        GameController.GameStartedChanged += GameStarted;
        GlobalTimer.Tick += Tick;
    }

    private static void Tick(float tickValue)
    {
        if (!_gameStarted)
            return;

        CheckSuperSpeed(ref tickValue);
        CheckGodMode(ref tickValue);
    }

    private static void CheckSuperSpeed(ref float tickValue)
    {
        if (!SpeedBoosterActive)
            return;

        if (_speedBoosterDuration < 0)
        {
            SpeedBoosterActive = false;
            return;
        }

        _speedBoosterDuration -= tickValue;
    }

    private static void CheckGodMode(ref float tickValue)
    {
        if (!GodMoveBoosterActive)
            return;

        if (_godModeBoosterDuration < 0)
        {
            GodMoveBoosterActive = false;
            return;
        }

        _godModeBoosterDuration -= tickValue;
    }

    private static void GameStarted(bool gameStarted)
    {
        _gameStarted = gameStarted;
        
        if (!_gameStarted)
            return;
        
        _speedBoosterDuration = 0;
        _godModeBoosterDuration = 0f;
        SpeedBoosterActive = false;
        GodMoveBoosterActive = false;
    }

    public static void ActivateBooster(BaseBooster baseBooster)
    {
        switch (baseBooster)
        {
            case AddScore _:
                GameController.Score += AddScoreBoosterValue;
                break;
            case GodMode _:
                _godModeBoosterDuration = _boosterDurations;
                GodMoveBoosterActive = true;
                break;
            case SuperSpeed _:
                _speedBoosterDuration = _boosterDurations;
                SpeedBoosterActive = true;
                break;
        }

        BoosterSpawner.ReleaseBooster(baseBooster);
    }
}