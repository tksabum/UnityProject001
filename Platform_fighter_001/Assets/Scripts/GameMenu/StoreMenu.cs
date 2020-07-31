using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerInfo
{
    public List<string> Weapons;
    public int Money;
    public int Score;
}

public class StoreMenu : MonoBehaviour
{
    private PlayerInfo GetPlayerInfo()
    {
        // 파일에서 읽어와서 값 지정
        PlayerInfo info = new PlayerInfo();
        info.Weapons = new List<string>();  // 예시
        info.Money = 0;                     // 예시
        info.Score = 0;                     // 예시

        // 무기, 돈, 점수, (+ 스킬, 능력치)
        return info;
    }

    public void BuyButton()
    {
        // 버튼의 이름(hierachy)을 가져와서 처리
        string btn = EventSystem.current.currentSelectedGameObject.name;
        string weapon = btn.Substring(9);
        Debug.Log("무기" + weapon + " 구매함");
    }

    public void BackToStageMenu()
    {
        Debug.Log("상점 퇴장");
        SceneManager.LoadScene("StageMenuScene");
    }
}
