using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SaveManager : MonoBehaviour, IController
{
    #region 单例
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 查找场景中是否已存在实例
                _instance = FindObjectOfType<SaveManager>();

                // 如果没有找到，创建一个新的GameObject并附加脚本
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SaveManager).Name);
                    _instance = singletonObject.AddComponent<SaveManager>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // 跨场景不销毁
        }
    }
    #endregion
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    private string solt = "";

    
    public void SaveGame(string Cslot, int level, Dictionary<int, int> TapCount)
    {
        this.solt = Cslot;
        PlayerPrefs.SetInt($"Slot{solt} Level",level);

        if (TapCount != null)
        {
            SerializableDictionary<int, int> serializableDict = new SerializableDictionary<int, int>(TapCount);
            string json = JsonUtility.ToJson(serializableDict); 
            PlayerPrefs.SetString($"Slot{solt} TapCount", json);
        }
        

       
        PlayerPrefs.Save();   
    }

    public Dictionary<int, int> LoadTapCount(string key)
    {
        if (PlayerPrefs.HasKey($"Slot{key} TapCount"))
        {
            string json = PlayerPrefs.GetString($"Slot{key} TapCount");
            SerializableDictionary<int, int> serializableDict = JsonUtility.FromJson<SerializableDictionary<int, int>>(json);
            return serializableDict.ToDictionary();
        }
        return new Dictionary<int, int>(); // 返回空列表如果键不存在
    }
    public int LoadLevel(string key)
    {
        if (PlayerPrefs.HasKey($"Slot{key} Level"))
        {
            int level = PlayerPrefs.GetInt($"Slot{key} Level");
            return level;
        }
        return  0;
    }

    // 辅助类用于JSON序列化
    [System.Serializable]
    private class SerializableDictionary<TKey, TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public SerializableDictionary(Dictionary<TKey, TValue> dict)
        {
            foreach (var pair in dict)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict[keys[i]] = values[i];
            }
            return dict;
        }
    }
    
}

