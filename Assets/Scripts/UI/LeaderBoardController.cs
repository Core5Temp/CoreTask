using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] private List<LeaderBoardItem> Items;

    private const string FilePath = "LeaderBoard.bin";
    
    private static LeaderBoardController _instance;
    
    private List<LeaderBoardItemData> _data = new List<LeaderBoardItemData>();

    public static bool IsNewRecord()
    {
        for (var i = 0; i < _instance._data.Count; i++)
        {
            if (_instance._data[i].Score < GameController.Score)
                return true;
        }

        return false;
    }

    public static void SaveNewRecord(string name)
    {
        _instance._data.Add(new LeaderBoardItemData(GameController.Score, name));
        _instance._data = _instance._data.OrderByDescending(x => x.Score).ToList();
        _instance._data.RemoveAt(_instance._data.Count - 1);
        
        _instance.UpdateData();
        _instance.Save();
    }
    
    private void Awake()
    {
        _instance = this;

        if (!File.Exists(FilePath))
        {
            InitNewData();
            return;
        }
        
        Load();
        UpdateData();
    }

    private void InitNewData()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            _data.Add(new LeaderBoardItemData(0, string.Empty));
            Items[i].InitValue((i + 1).ToString(), string.Empty, string.Empty);
        }
    }
    
    private void UpdateData()
    {
        for (var i = 0; i < _data.Count; i++)
        {
            var currentData = _data[i];
            Items[i].InitValue((i + 1).ToString(), currentData.Name, currentData.Score.ToString());
        }
    }

    private void Save()
    {
        try
        {
            using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, _data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void Load()
    {
        try
        {
            using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                var binaryFormatter = new BinaryFormatter();
                _data = (List<LeaderBoardItemData>) binaryFormatter.Deserialize(fileStream);
                return;
            }
        }
        catch (FileNotFoundException)
        {
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}

[Serializable]
public class LeaderBoardItemData
{
    public int Score;
    public string Name;

    public LeaderBoardItemData(int score, string name)
    {
        Score = score;
        Name = name;
    }
}