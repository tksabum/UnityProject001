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

    public float maxHP;
    public float maxSpeed;
    public float normalSpeed;
    public Image healthBarFilled;

    protected float HP;

    private void Update()
    {

    }
}
