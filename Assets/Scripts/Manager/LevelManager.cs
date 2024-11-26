using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using LitJson;
using System.Runtime.CompilerServices;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoSingleton<LevelManager>
{
    TextAsset 
        trail, trailTag, animal, animalTag, 
        color, colorTag, job, jobTag, friend, friendTag,
        romance, romanceTag, child, childTag, childEasy, childEasyTag,
        zombie, island, war, sm, superhero, superheroTag, hentai, xp,
        eq;

    Dictionary<int, LevelData> trailLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> trailTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> animalLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> animalTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> colorLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> colorTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> jobLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> jobTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> friendLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> friendTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> romanceLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> romanceTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> childLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> childTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> childEasyLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> childEasyTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> zombieLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> islandLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> warLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> smLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> hentaiLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> superheroLevel = new Dictionary<int, LevelData>();
    Dictionary<int, TagData> superheroTagDic = new Dictionary<int, TagData>();
    Dictionary<int, LevelData> xpLevel = new Dictionary<int, LevelData>();
    Dictionary<int, LevelData> eqLevel = new Dictionary<int, LevelData>();


    Dictionary<GameType, Dictionary<int, LevelData>> levelDic = new Dictionary<GameType, Dictionary<int, LevelData>>();
    Dictionary<GameType, Dictionary<int, TagData>> levelTagDic = new Dictionary<GameType, Dictionary<int, TagData>>();
    private void Awake()
    {
        trail = Resources.Load<TextAsset>("Text/Trail/Trail");
        trailTag = Resources.Load<TextAsset>("Text/Trail/TrailTag");
        animal = Resources.Load<TextAsset>("Text/Animal/Animal");
        animalTag = Resources.Load<TextAsset>("Text/Animal/AnimalTag");
        color = Resources.Load<TextAsset>("Text/Color/Color");
        colorTag = Resources.Load<TextAsset>("Text/Color/ColorTag");
        job = Resources.Load<TextAsset>("Text/Job/Job");
        jobTag = Resources.Load<TextAsset>("Text/Job/JobTag");
        friend = Resources.Load<TextAsset>("Text/Friend/Friend");
        friendTag = Resources.Load<TextAsset>("Text/Friend/FriendTag");
        romance = Resources.Load<TextAsset>("Text/Romance/Romance");
        romanceTag = Resources.Load<TextAsset>("Text/Romance/RomanceTag");
        child = Resources.Load<TextAsset>("Text/Child/Child");
        childTag = Resources.Load<TextAsset>("Text/Child/ChildTag");
        childEasy = Resources.Load<TextAsset>("Text/ChildEasy/ChildEasy");
        childEasyTag = Resources.Load<TextAsset>("Text/ChildEasy/ChildEasyTag");
        zombie = Resources.Load<TextAsset>("Text/ZombieSurvival/Zombie");
        island = Resources.Load<TextAsset>("Text/IslandSurvival/Island");
        war = Resources.Load<TextAsset>("Text/WarSurvival/War");
        sm = Resources.Load<TextAsset>("Text/SM/SM");
        hentai = Resources.Load<TextAsset>("Text/Hentai/Hentai");
        superhero = Resources.Load<TextAsset>("Text/SuperHero/SuperHero");
        superheroTag = Resources.Load<TextAsset>("Text/SuperHero/SuperHeroTag");
        xp = Resources.Load<TextAsset>("Text/XP/XP");
        eq = Resources.Load<TextAsset>("Text/EQ/EQ");

    }

    public void ReadAllCfg()
    {
        ReadCfg();
    }

    public void ReadCfg()
    {
        ReadLevelCfg(trail.text, trailLevel, trailTag.text, trailTagDic, GameType.Trail);
        ReadLevelCfg(animal.text, animalLevel, animalTag.text, animalTagDic, GameType.Animal);
        ReadLevelCfg(color.text, colorLevel, colorTag.text, colorTagDic, GameType.Color);
        ReadLevelCfg(job.text, jobLevel, jobTag.text, jobTagDic, GameType.Job);
        ReadLevelCfg(friend.text, friendLevel, friendTag.text, friendTagDic, GameType.Friend);
        ReadLevelCfg(romance.text, romanceLevel, romanceTag.text, romanceTagDic, GameType.Romance);
        ReadLevelCfg(child.text, childLevel, childTag.text, childTagDic, GameType.Child);
        ReadLevelCfg(childEasy.text, childEasyLevel, childEasyTag.text, childEasyTagDic, GameType.ChildEasy);
        ReadLevelCfg(zombie.text, zombieLevel, "", null, GameType.Zombie);
        ReadLevelCfg(island.text, islandLevel, "", null, GameType.Island);
        ReadLevelCfg(war.text, warLevel, "", null, GameType.War);
        ReadLevelCfg(sm.text, smLevel, "", null, GameType.SM);
        ReadLevelCfg(hentai.text, hentaiLevel, "", null, GameType.Hentai);
        ReadLevelCfg(superhero.text, superheroLevel, superheroTag.text, superheroTagDic, GameType.SuperHero);
        ReadLevelCfg(xp.text, xpLevel, "", null, GameType.XP);
        ReadLevelCfg(eq.text, eqLevel, "", null, GameType.EQ);
    }

    void ReadLevelCfg(string json, Dictionary<int, LevelData> dic, string tagJson, Dictionary<int, TagData> tagDic, GameType gameType)
    {
        dic.Clear();
        //LevelListData levelListData = JsonConvert.DeserializeObject<LevelListData>(trail.text); //newTwonsoft.json
        LevelListData levelListData = JsonMapper.ToObject<LevelListData>(json);//litjson

        for (int i = 0; i < levelListData.Level.Count; i++)
        {
            LevelData data = new LevelData();

            data.Question = levelListData.Level[i].Question;
            data.SelectionNum = levelListData.Level[i].SelectionNum;
            data.SelectionTxt_1 = levelListData.Level[i].SelectionTxt_1;
            data.SelectionTxt_2 = levelListData.Level[i].SelectionTxt_2;
            data.SelectionTxt_3 = levelListData.Level[i].SelectionTxt_3;
            data.SelectionTxt_4 = levelListData.Level[i].SelectionTxt_4;
            data.SelectionImg_1 = levelListData.Level[i].SelectionImg_1;
            data.SelectionImg_2 = levelListData.Level[i].SelectionImg_2;
            data.SelectionImg_3 = levelListData.Level[i].SelectionImg_3;
            data.SelectionImg_4 = levelListData.Level[i].SelectionImg_4;
            data.SelectionTag_1 = levelListData.Level[i].SelectionTag_1;
            data.SelectionTag_2 = levelListData.Level[i].SelectionTag_2;
            data.SelectionTag_3 = levelListData.Level[i].SelectionTag_3;
            data.SelectionTag_4 = levelListData.Level[i].SelectionTag_4;
            data.SelectPercent_1 = levelListData.Level[i].SelectPercent_1;
            data.SelectPercent_2 = levelListData.Level[i].SelectPercent_2;
            data.SelectPercent_3 = levelListData.Level[i].SelectPercent_3;
            data.SelectPercent_4 = levelListData.Level[i].SelectPercent_4;

            dic.Add(i, data);
        }

        if(tagDic != null)
        {
            tagDic.Clear();
            TagListData tagListData = JsonMapper.ToObject<TagListData>(tagJson);
            for (int i = 0; i < tagListData.TagList.Count; i++)
            {
                TagData data = new TagData();

                data.Id = tagListData.TagList[i].Id;
                data.TypeName = tagListData.TagList[i].TypeName;
                data.DetailDesc = tagListData.TagList[i].DetailDesc;
                data.FunDesc = tagListData.TagList[i].FunDesc;
                data.Tag = tagListData.TagList[i].Tag;

                tagDic.Add(data.Id, data);
            }
            levelTagDic.Add(gameType, tagDic);

        }


        levelDic.Add(gameType, dic);

    }

    public int GetSelectionLevelTotalNum(GameType gameType)
    {
        int num = 0;
        Dictionary<int, LevelData> dic;
        levelDic.TryGetValue(gameType, out dic);
        if(dic != null)
        {
            num = dic.Count;
        }
        //switch (gameType)
        //{
        //    case GameType.Trail:
        //        return trailLevel.Count;
        //    case GameType.Animal:
        //        return animalLevel.Count;
        //    case GameType.Color:
        //        return colorLevel.Count;
        //    case GameType.Job:
        //        return jobLevel.Count;
        //    case GameType.Friend:
        //        return friendLevel.Count;
        //    case GameType.Romance:
        //        return romanceLevel.Count;
        //    case GameType.Child:
        //        return childLevel.Count;
        //}

        return num;
    }

    public LevelData GetLevelData(GameType gameType, int level)
    {
        LevelData levelData;
        Dictionary<int, LevelData> dic;
        levelDic.TryGetValue(gameType, out dic);
        if (dic != null)
        {
            dic.TryGetValue(level, out levelData);
            return levelData;
        }

        return null;
    }

    public List<int> GetSelectionTags(GameType gameType, int level, int selection)
    {
        LevelData levelData = GetLevelData(gameType, level);
        if (levelData != null)
        {
            switch (selection)
            {
                case 1:
                    return levelData.SelectionTag_1;
                case 2:
                    return levelData.SelectionTag_2;
                case 3:
                    return levelData.SelectionTag_3;
                case 4:
                    return levelData.SelectionTag_4;
            }
        }
    

        return null;
    }

    public TagData GetTagData(GameType gameType, int tag)
    {
        TagData tagData;
        switch (gameType)
        {
            case GameType.Trail:
                trailTagDic.TryGetValue(tag, out tagData);
                return tagData;
        }

        return null;
    }
}
