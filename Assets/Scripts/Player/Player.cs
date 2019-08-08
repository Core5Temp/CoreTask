using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void OnReturnToPool()
    {
        
    }

    public void OnGetInPool()
    {
        
    }
}
