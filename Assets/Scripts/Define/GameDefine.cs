using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

namespace GameDefine 
{
    public enum GameType
    {
        Trail = 1,
        Animal = 2,
        Color = 3,
        Job = 4,
        Friend = 5,
        Romance = 6,
        Child = 7,
        ChildEasy = 8,
        Zombie = 9,
        Island = 10,
        War = 11,
        SuperHero = 12,
        SM = 13,
        Hentai = 14,
        XP = 15,
        EQ = 16
    }

    public enum LanguageType
    {
        zh = 0,
        ja = 1,
        en = 2,
        ko = 3,
    }

    public enum TrailTag
    {
        Emotion = 0,
        Ethics  = 1,
        Moraity = 2,
        Observe = 3,
        Careful = 4,
    }

    public enum AnimalTag
    {
        Lion = 0,
        Owl = 1,
        Monkey = 2,
        Elephant = 3,
        Dolphin = 4,
    }

    public enum GameAllType
    {
        Selection = 0,
        Survival = 1,
    }

    public static class GameConst
    {
        public static int totalLeveNum = 6;
    }
}
