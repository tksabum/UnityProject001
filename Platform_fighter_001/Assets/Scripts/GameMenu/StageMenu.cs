using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageMenu : MonoBehaviour
{
    public void EnterStore()
    {
        Debug.Log("상점 입장");
        SceneManager.LoadScene("StoreMenuScene");
    }

    public void StartStage()
    {
        string stage = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("스테이지 입장 " + stage);
        SceneManager.LoadScene(stage+"Scene");
    }

    public void BackToGameMenu()
    {
        Debug.Log("게임메뉴로");
        SceneManager.LoadScene("GameMenuScene");
    }
}