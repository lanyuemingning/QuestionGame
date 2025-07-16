using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SaveModel : AbstractModel
{
    public class SaveData
    {
        public int level;
        public string saveName;
        public List<int> TapCount;
    }
    protected override void OnInit()
    {
        LoadAllSaves();
    }

    public Dictionary<string, SaveData> AllSaves { get; private set; } = new Dictionary<string, SaveData>();

    // 当前选中的存档
    public SaveData CurrentSave { get; private set; }
    public string CurrentSaveName { get; private set; }

    public bool CreateNewSave(string saveName)
    {
        if (AllSaves.ContainsKey(saveName))
        {
            Debug.LogWarning($"存档名称 '{saveName}' 已存在");
            return false;
        }

        var newSave = new SaveData
        {
            level = 0,
            saveName = saveName,
            TapCount = new List<int>(),

        };

        AllSaves.Add(saveName, newSave);
        CurrentSave = newSave;
        CurrentSaveName = saveName;

        SaveAllSaves();
        return true;
    }
    public bool LoadSave(string saveName)
    {
        if (!AllSaves.TryGetValue(saveName, out var saveData))
        {
            Debug.LogWarning($"找不到存档: {saveName}");
            return false;
        }

        CurrentSave = saveData;
        CurrentSaveName = saveName;
        return true;
    }

    // 删除存档
    public bool DeleteSave(string saveName)
    {
        if (!AllSaves.ContainsKey(saveName))
            return false;

        AllSaves.Remove(saveName);

        // 如果删除的是当前存档，清空当前存档引用
        if (CurrentSaveName == saveName)
        {
            CurrentSave = null;
            CurrentSaveName = null;
        }

        SaveAllSaves();
        return true;
    }

    // 保存当前存档
    public void SaveCurrent()
    {
        if (CurrentSave == null) return;;
        SaveAllSaves();
    }
    // 保存所有存档数据到PlayerPrefs
    private void SaveAllSaves()
    {
        var saveList = new List<SaveData>(AllSaves.Values);
        var wrapper = new SaveDataWrapper { saves = saveList };
        
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("all_game_saves", json);
        PlayerPrefs.Save();
    }

    // 从PlayerPrefs加载所有存档
    private void LoadAllSaves()
    {
        AllSaves.Clear();
        
        if (PlayerPrefs.HasKey("all_game_saves"))
        {
            string json = PlayerPrefs.GetString("all_game_saves");
            var wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
            
            foreach (var save in wrapper.saves)
            {
                AllSaves[save.saveName] = save;
            }
        }
    }

    // 用于JSON序列化的包装类
    [System.Serializable]
    private class SaveDataWrapper
    {
        public List<SaveData> saves;
    }
}
