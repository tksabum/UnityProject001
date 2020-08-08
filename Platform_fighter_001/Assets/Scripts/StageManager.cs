using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int StageNumber;

    PlatformFighter platformfighter;
    GameInfo gameInfo;
    System.Diagnostics.Stopwatch Stopwatch;

    private void Awake()
    {
        platformfighter = PlatformFighter.getPlatformFighter();
        gameInfo = platformfighter.getLastGameInfo();
        gameInfo.stageNumber = StageNumber;
        gameInfo.totalPoint = 0;
        gameInfo.money = 0;
        gameInfo.clear = false;
        gameInfo.time = 0L;

        Stopwatch = new System.Diagnostics.Stopwatch();
        Stopwatch.Reset();
        Stopwatch.Start();
    }

    void Update()
    {
        
    }

    public void EnemyKill(GameObject gameObject)
    {
        Enemy_Boss enemyBoss = gameObject.GetComponent<Enemy_Boss>();
        if (enemyBoss != null)
        {
            OnBossDie(gameObject);
        }
        else
        {
            OnEnemyDie(gameObject);
        }
    }

    private void OnEnemyDie(GameObject gameObject)
    {
        // killPoint 획득
        Enemy enemy = gameObject.GetComponent<Enemy>();
        gameInfo.totalPoint += enemy.killPoint;
    }

    private void OnBossDie(GameObject gameObject)
    {
        // killPoint 획득
        Enemy enemy = gameObject.GetComponent<Enemy>();
        gameInfo.totalPoint += enemy.killPoint;

        // 클리어시간 기록
        gameInfo.time = Stopwatch.ElapsedMilliseconds;
        Debug.Log("클리어시간 : " + gameInfo.time + "[ms]");

        // 클리어 기록
        gameInfo.clear = true;

        // 결과화면으로 이동
        SceneManager.LoadScene("GameResultScene");
    }
    
}
