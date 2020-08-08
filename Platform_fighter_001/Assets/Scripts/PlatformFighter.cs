using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string[] WeaponSlots;
    public Dictionary<string, bool> Weapons;
    public int Money;
    public List<int> Score;
    public int StageOpen;

    public PlayerInfo()
    {
        WeaponSlots = new string[5];
        Weapons = new Dictionary<string, bool>()
        {
            {"bullet_Normal",  true },
            {"bullet_Fast",  false },
            {"bullet_LargeDamage",  false },
            {"bullet_Explosion",  false },
            {"bullet_Seek",  false }
        };
        Money = 0;
        Score = new List<int>();
        StageOpen = 0;
    }
}

public class GameInfo
{
    public int stageNumber;
    public int totalPoint;
    public int money;
    public bool clear;
    public long time;
}

public class StoreUI
{
    public bool[] BuyButton;

    public StoreUI()
    {
        int NumButtons = 10;

        BuyButton = new bool[NumButtons];
        for (int i = 0; i < NumButtons; i++)
            BuyButton[i] = true;
    }

    public StoreUI(bool[] InventoryInfo) {  
        // 파일 입출력으로 보유 아이템 정보가 있을 때
        BuyButton = InventoryInfo;
    }
}

public class GameUI
{
    public StoreUI storeUI;

    public GameUI()
    {
        storeUI = new StoreUI();
    }
}

public class StageInfo
{
    public float MoneyPerPoint;
    public int ClearBonusMoney;
    public StageInfo(float MoneyPerPoint, int ClearBonusMoney)
    {
        this.MoneyPerPoint = MoneyPerPoint;
        this.ClearBonusMoney = ClearBonusMoney;
    }
}

public class PlatformFighter
{
    PlayerInfo PlayerInfo;
    GameInfo LastGameInfo;
    Dictionary<int, StageInfo> StageInfo;

    public GameUI gameUI;

    private static PlatformFighter platformfighter;

    public static PlatformFighter getPlatformFighter()
    {
        if (platformfighter == null)
            platformfighter = new PlatformFighter();

        // 저장된 데이터 불러와서 platformfighter에 적용하기

        return platformfighter;
    }

    public PlatformFighter()
    {
        Debug.Log("PlatformFighter");
        // 불러오기 시도

        // 실패한 경우 초기값으로 설정
        PlayerInfo = new PlayerInfo();

        LastGameInfo = new GameInfo();

        StageInfo = new Dictionary<int, StageInfo>();
        StageInfo[010101] = new StageInfo(1.0f, 5000);
        StageInfo[010102] = new StageInfo(1.0f, 5000);
        StageInfo[010103] = new StageInfo(1.0f, 5000);
        StageInfo[010104] = new StageInfo(1.0f, 5000);
        StageInfo[010105] = new StageInfo(1.0f, 5000);
        StageInfo[010106] = new StageInfo(1.0f, 5000);
        StageInfo[010107] = new StageInfo(1.0f, 5000);

        gameUI = new GameUI();
    }

    public void addWeapon(string weapon)
    {
        Debug.Log(weapon + " 구매");
        PlayerInfo.Weapons[weapon] = true;
    }

    public bool EquipWeapon(int slot, string weapon)
    {
        if (PlayerInfo.Weapons.ContainsKey(weapon))
        {
            PlayerInfo.WeaponSlots[slot] = weapon;
            Debug.Log("무기" + weapon + " Slot" + slot);
            return true;
        }
        else
        {
            Debug.Log("버그발생! 보유하지 않은 무기 추가 시도!");
            return false;
        }
            
    }

    public void setPlayerInfo(PlayerInfo Pinfo)
    {
        PlayerInfo = Pinfo;
    }

    public PlayerInfo getPlayerInfo()
    {
        return PlayerInfo;
    }

    public void setLastGameInfo(GameInfo Ginfo)
    {
        LastGameInfo = Ginfo;
    }

    public GameInfo getLastGameInfo()
    {
        return LastGameInfo;
    }

    public Dictionary<int, StageInfo> getStageInfo()
    {
        return StageInfo;
    }
}
