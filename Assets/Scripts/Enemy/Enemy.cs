﻿
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public float Speed { get; set; }

    public Transform Transform { get; private set; }

    private void Awake()
    {
        Transform = transform;
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public void OnGetInPool()
    {
        gameObject.SetActive(true);
    }
}
