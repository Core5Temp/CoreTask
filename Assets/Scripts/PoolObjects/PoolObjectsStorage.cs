
using System.IO;
using System.Collections.Generic;
    
using UnityEngine;

public class PoolObjectsStorage : ScriptableObject
{
    public List<MonoBehaviour> ObjectsInPool = new List<MonoBehaviour>();

    private static PoolObjectsStorage _instance;

    private const string Path = "Assets/Resources/ScriptableObjects";

    public static PoolObjectsStorage Instance
    {
        get
        {
            if(_instance != null)
                return _instance;

            _instance = Resources.Load<PoolObjectsStorage>("ScriptableObjects/PoolObjectsStorage");
                
            if(_instance != null)
                return _instance;

            _instance = CreateInstance<PoolObjectsStorage>();
            
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            UnityEditor.AssetDatabase.CreateAsset(_instance, $"{Path}/PoolObjectsStorage.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return _instance;

        }
    }
    
    [UnityEditor.MenuItem("TestGame/PoolObject")]
    private static void Show()
    {
        UnityEditor.Selection.activeObject = Instance;
    }
}