
using UnityEngine;

public static class CollisionController
{
    private const string EnemyTag = "Enemy";
    private const string BoosterTag = "Booster";
    
    public static void OnTriggerEnter(Collider2D collider2D)
    {
        var gameObject = collider2D.gameObject;
        var tag = gameObject.tag;
            
        if (tag.Equals(EnemyTag))
        {
            if (BoosterController.GodMoveBoosterActive)
            {
                EnemySpawner.ReleaseEnemy(gameObject.GetComponent<Enemy>());
                return;
            }

            GameController.GameStarted = false;
            return;
        }

        if (tag.Equals(BoosterTag))
            BoosterController.ActivateBooster(gameObject.GetComponent<BaseBooster>());
    }
}
