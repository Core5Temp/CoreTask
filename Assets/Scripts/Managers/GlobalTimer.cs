using System;

public static class GlobalTimer
{
   public static event Action<float> Tick;

   private static bool _timerStoped;
   
   private static float _timeDelta;

   public static void Init()
   {
      ApplicationObserver.MonoBehaviorUpdate += MonoBehaviorUpdate;
      StartTimer();
   }
   
   private static void MonoBehaviorUpdate(float deltaTime)
   {
      if(_timerStoped)
         return;
        
      _timeDelta += deltaTime;

      if (_timeDelta < 1) 
         return;

      OnTick();
      _timeDelta -= 1;
   }
   
   private static void OnTick()
   {
      if (_timerStoped)
         return;

      Tick?.Invoke(_timeDelta);
   }


   private static void StartTimer()
   { 
      _timerStoped = false;
   }
}
