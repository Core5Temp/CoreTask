using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    private static readonly Dictionary<Type, Queue<MonoBehaviour>> Pool = new Dictionary<Type, Queue<MonoBehaviour>>();

    private static readonly Dictionary<Type, MonoBehaviour> DefaultPrefabsInPool = new Dictionary<Type, MonoBehaviour>();

    private static PoolObjects _instance;
    private static List<MonoBehaviour> _objectsInPool;

    public static PoolObjects Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            var type = typeof(PoolObjects);
            var gameObject = new GameObject(type.Name, type);
            
            _instance = gameObject.GetComponent<PoolObjects>();
            DontDestroyOnLoad(gameObject);
            
            gameObject.transform.position = Vector3.one * 1000;
            
            return _instance;
        }
    }

    public  void Init()
    {
        _objectsInPool = PoolObjectsStorage.Instance.ObjectsInPool;

        for (var i = 0; i < _objectsInPool.Count; i++)
        {
            var gO = _objectsInPool[i];
            var type = gO.GetComponent<IPoolable>().GetType();
            DefaultPrefabsInPool.Add(type, gO);
        }
    }

    public static T Get<T>() where T : MonoBehaviour, IPoolable
    {
        var type = typeof(T);
        
        if (!Pool.ContainsKey(type))
            return Create<T>();
        
        var queue = Pool[type];
        if (queue == null || queue.Count == 0)
            return Create<T>();

        var returnObject = Pool[type].Dequeue() as T;
        returnObject.OnGetInPool();
            
        return returnObject as T;
    }

    public static T Create<T>() where T : MonoBehaviour, IPoolable
    {
        var type = typeof(T);

        if (!DefaultPrefabsInPool.ContainsKey(type))
        {
            Debug.LogError("Prefab not found");
            return null;
        }

        var prefab = DefaultPrefabsInPool[type];
        var instantiate = Instantiate(prefab, Vector3.zero, Quaternion.identity, Instance.transform);
        var pO = instantiate.GetComponent<IPoolable>();
        
        pO.OnReturnToPool();
        return pO as T;
    }

    public static void Release<T>(T poolableObject) where T : MonoBehaviour, IPoolable
    {
        var type = poolableObject.GetType();

        Transform transform1;
        (transform1 = poolableObject.transform).SetParent(Instance.transform);
        transform1.localPosition = Vector3.zero;
        transform1.localScale = Vector3.one;

        if (Pool.ContainsKey(type))
        {
            if(!Pool[type].Contains(poolableObject))
                Pool[type].Enqueue(poolableObject);
        }
        else
        {
            var queue = new Queue<MonoBehaviour>();
            queue.Enqueue(poolableObject);
            Pool.Add(type, queue);
        }
        
        poolableObject.OnReturnToPool();
    }
}