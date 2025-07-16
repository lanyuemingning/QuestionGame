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

    // ��ǰѡ�еĴ浵
    public SaveData CurrentSave { get; private set; }
    public string CurrentSaveName { get; private set; }

    public bool CreateNewSave(string saveName)
    {
        if (AllSaves.ContainsKey(saveName))
        {
            Debug.LogWarning($"�浵���� '{saveName}' �Ѵ���");
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
            Debug.LogWarning($"�Ҳ����浵: {saveName}");
            return false;
        }

        CurrentSave = saveData;
        CurrentSaveName = saveName;
        return true;
    }

    // ɾ���浵
    public bool DeleteSave(string saveName)
    {
        if (!AllSaves.ContainsKey(saveName))
            return false;

        AllSaves.Remove(saveName);

        // ���ɾ�����ǵ�ǰ�浵����յ�ǰ�浵����
        if (CurrentSaveName == saveName)
        {
            CurrentSave = null;
            CurrentSaveName = null;
        }

        SaveAllSaves();
        return true;
    }

    // ���浱ǰ�浵
    public void SaveCurrent()
    {
        if (CurrentSave == null) return;;
        SaveAllSaves();
    }
    // �������д浵���ݵ�PlayerPrefs
    private void SaveAllSaves()
    {
        var saveList = new List<SaveData>(AllSaves.Values);
        var wrapper = new SaveDataWrapper { saves = saveList };
        
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("all_game_saves", json);
        PlayerPrefs.Save();
    }

    // ��PlayerPrefs�������д浵
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

    // ����JSON���л��İ�װ��
    [System.Serializable]
    private class SaveDataWrapper
    {
        public List<SaveData> saves;
    }
}
