using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    /*
     * 무기 데이터 관리
     * 총알 수 관리
     */

    public GameObject bullet_Normal;
    public GameObject bullet_Fast;
    public GameObject bullet_LargeDamage;

    private int usingWeapon;
    private GameObject[] weapon;
    private int[] weaponType;

    private void Awake()
    {
        usingWeapon = 2;
        weaponType = new int[3];
        weaponType[0] = 0;
        weaponType[1] = 0;
        weaponType[2] = 0;

        weapon = new GameObject[3];
        weapon[0] = bullet_Normal;
        weapon[1] = bullet_Fast;
        weapon[2] = bullet_LargeDamage;
    }

    private void Update()
    {

    }

    public GameObject getWeapon()
    {
        return weapon[usingWeapon];
    }
    
    public int getWeaponType()
    {
        return weaponType[usingWeapon];
    }
}
