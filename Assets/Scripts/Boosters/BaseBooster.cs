
using UnityEngine;

public abstract class BaseBooster : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public Transform Transform { get; private set; }

    protected void Awake()
    {
        Transform = transform;
    }

    public virtual void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnGetInPool()
    {
        gameObject.SetActive(true);
    }
}