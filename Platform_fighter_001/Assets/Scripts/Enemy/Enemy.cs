using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : HPObject
{
    /*
     * 공통되는 변수
     * 
     * 체력, 이동속도
     */

    public int killPoint;
    public float maxSpeed;
    public float normalSpeed;
    public Image healthBarFilled;

    protected float HP;

    protected void DelMonster(float delay)
    {
        GameObject stageManager = GameObject.Find("StageManager");
        stageManager.GetComponent<StageManager>().EnemyKill(gameObject);
        Invoke("_DelMonster", delay);
    }

    private void _DelMonster()
    {
        Destroy(gameObject);
    }
}
