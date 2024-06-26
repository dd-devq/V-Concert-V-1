using System;
using UnityEngine;
using PlayFab.ClientModels;

namespace UI
{
    public class UIParam
    {
        public object Data;
    }

    public enum UIIndex
    {
        UIAuthentication,
        UIMainMenu,
        UIInventory,
        UIShop,
        UISetting,
        UIItemViewer,
        UICharacterSelection,
        UIModeSelection,
        UISongSelection,
        UIAvatarSelection,
        UILobby,
        UILoading,
        UINavigationTab,
        UIHud,
        UIVictory,
        UIPause,
        UIInfo,
        UILevelEditor,
        UIConcertEditor,
        None
    }

    public class UIItemViewerParam : UIParam
    {
        public string ItemId;
        public string ItemName;
        public string Description;
        public string Currency;
        public uint Price;
    }
}

namespace EventData
{
    public struct RegisterInfo
    {
        public string Username;
        public string Password;
        public string Email;
        public Action RegisterFailCallback;
        public Action RegisterSuccessCallback;
    }

    public struct LoginInfo
    {
        public string Username;
        public string Password;
        public Action LoginFailCallback;
        public Action LoginSuccessCallback;
    }

    public struct AutoLoginInfo
    {
        public Action AutoLoginFailCallback;
        public Action AutoLoginSuccessCallback;
    }

    public struct ResetInfo
    {
        public string Username;
    }

    public struct UserData
    {
        public int Coin;
        public int Gem;
        public string Username;
    }

    public struct RewardData
    {
        public string CoinKey;
        public int CoinAmount;
        public string GemKey;
        public int GemAmount;
    }

    public struct LeaderBoardReqInfo
    {
        public string Name;
        public Action SuccessCallback;
    }

    public struct VolumeData
    {
        public float SoundVolume;
        public float MusicVolume;
    }

    public struct LevelData
    {
        public string SongName;
        public int SongIndex;
    }

    public struct UserLevelData
    {
        public string SongPath;
        public string MIDIPath;
        public string BundlePath;
        public string AnimClip;
    }


    public struct EndLevelData
    {
        public int Coin;
        public int Gem;
        public int Score;
        public bool IsCustom;
    }

    public struct UpdateLeaderBoardReqInfo
    {
        public string Name;
        public int Score;
        public Action SuccessCallback;
    }
}


namespace GameData
{
    [Serializable]
    public struct Song
    {
        public string Title;
        public string Artist;
        public string Cover;
    }

    [Serializable]
    public struct CustomSongData
    {
        public string SongPath;
        public string SongName;
        public string MIDIPath;
        public string AnimationPath;
    }

    [Serializable]
    public struct Item
    {
        public ItemInstance Data;
        public string Image;
    }

    [Serializable]
    public struct Character
    {
        public string Path;
        public string ModelName;
        public string Image;
    }
}

public enum HitType
{
    Perfect,

    //Good,
    //Normal,
    Miss
}


public static class Define
{
    //the gap time between 2 notes
    public static int NoteInterval = 2;
    public static int NumOfActivators = 2;

    public static string MidiFilePath = "/MIDI/";
    public static string AudioFilePath = "/Audio/Music/";
    public static string AudioFileExtension = ".ogg";
    public static string MidiFileExtension = ".mid";

    public static int NormalScore = 10;
    public static int GoodScore = 20;
    public static int PerfectScore = 100;
    public static float HitObjectInterval = 0.2f;


    public enum PrefabName
    {
        NotePrefab
    }

    public enum Tags
    {
        Activator,
        PerfectHit,
        Note,
    }
}