using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void StartClick()
    {
        Debug.Log("Game Start");
        SceneManager.LoadScene("SampleScene");
    }

    public void StageButton()
    {
        Debug.Log("Stage Menu");
        SceneManager.LoadScene("StageMenuScene");
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료");

        // 에디터 종료
        // UnityEditor.EditorApplication.isPlaying = false; 
        // ####### wasd가 안먹히게되는 버그가 있는 듯

        // 실제 게임 종료
        // Application.Quit();
    }
}