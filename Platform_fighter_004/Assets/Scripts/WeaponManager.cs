using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    /*
     * 무기 데이터 관리
     * 총알 수 관리
     */

    public GameObject bullet_Normal;
    public GameObject bullet_Fast;
    public GameObject bullet_LargeDamage;
    public Text WeaponUI;

    private int usingWeapon;
    private GameObject[] weapon;
    private int[] weaponType;
    private int NumberOfWeapon;

    private void Awake()
    {
        NumberOfWeapon = 3; // 총 무기 개수
        setWeapon(0);
        weaponType = new int[NumberOfWeapon];
        weaponType[0] = 0;
        weaponType[1] = 0;
        weaponType[2] = 0;

        weapon = new GameObject[NumberOfWeapon];
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

    public void nextWeapon()
    {
        setWeapon((usingWeapon + 1) % NumberOfWeapon);
    }

    // usingWeapon을 변경하고 UI에 적용
    private void setWeapon(int uw)
    {
        usingWeapon = uw;
        WeaponUI.text = "장착한 무기: " + usingWeapon;
    }
}
