using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InventoryScene : MonoBehaviour
{
    PlatformFighter platformfighter;
    public Dropdown[] dropDown;

    private void Awake()
    {
        platformfighter = PlatformFighter.getPlatformFighter();

        Dictionary<string, bool> WeaponDic = platformfighter.getPlayerInfo().Weapons;

        foreach (string weapon in WeaponDic.Keys)
        {
            if (WeaponDic[weapon])
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = weapon;
                for (int i = 0; i < 5; i++)
                    dropDown[i].options.Add(option);
            }
        }

        string[] weaponSlot = platformfighter.getPlayerInfo().WeaponSlots;

        for (int i = 0; i < weaponSlot.Length; i++)
        {
            if (weaponSlot[i] != null)
            {
                // 드롭다운 값 지정.
                dropDown[i].value = dropDown[i].options.FindIndex(option => option.text == weaponSlot[i]);
            }
        }
    }

    public void StoreMenu()
    {
        SceneManager.LoadScene("StoreMenuScene");
    }

    public void SlotArmed(int slotNum)
    {
        string[] weaponSlots = platformfighter.getPlayerInfo().WeaponSlots;

        if (dropDown[slotNum - 1].options[dropDown[slotNum - 1].value].text == "비워두기")
        {
            weaponSlots[slotNum - 1] = null;
            Debug.Log("슬롯" + slotNum.ToString() + "무장 해제됨");
        }
        else
        {
            weaponSlots[slotNum - 1] = dropDown[slotNum - 1].options[dropDown[slotNum - 1].value].text;
            Debug.Log("슬롯" + slotNum.ToString() + "장비됨. 무장: " + dropDown[slotNum - 1].options[dropDown[slotNum - 1].value].text);
        }
    }
}
