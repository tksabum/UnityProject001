using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StoreMenuManager : MonoBehaviour
{
    PlatformFighter platformfighter;

    private void Awake()
    {
        platformfighter = PlatformFighter.getPlatformFighter();

        // 현재 보유중인 무기만 구매할 수 있도록 UI 설정
        Dictionary<string, bool> WeaponDic = platformfighter.getPlayerInfo().Weapons;
        foreach (string weapon in WeaponDic.Keys)
        {
            Button buttonObject = GameObject.Find("BTN_" + weapon).GetComponent<Button>();
            Text btnText = buttonObject.GetComponentInChildren<Text>();
            if (WeaponDic[weapon])
            {
                buttonObject.interactable = false;
                btnText.text = "보유중";
            }
            else
            {
                buttonObject.interactable = true;
                btnText.text = "구매";
            }
        }
    }

    public void BuyButton(string btnName)
    {
        // 버튼 오브젝트 가져오기
        Button buttonObject = GameObject.Find("BTN_" + btnName).GetComponent<Button>();
        Text btnText = buttonObject.GetComponentInChildren<Text>();

        // 눌린 버튼 비활성화
        buttonObject.interactable = false;
        btnText.text = "보유중";

        platformfighter.addWeapon(btnName);
    }

    public void BackToStageMenu()
    {
        SceneManager.LoadScene("StageMenuScene");
    }

    public void InventoryMenu()
    {
        SceneManager.LoadScene("InventoryScene");
    }
}