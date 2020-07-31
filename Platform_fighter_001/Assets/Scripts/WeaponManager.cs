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
    public GameObject bullet_Explosion;
    public GameObject bullet_Seek;
    public Text WeaponUI;

    private int usingWeapon;
    private GameObject[] weapon;
    private int[] weaponType;
    private int NumberOfWeapon;
    private int[] bullet;

    /*
     * Weapon type
     * 
     * type 0
     *  사용할 때 총알을 소모하지 않는 기본 총
     * 
     * type 1
     *  기본 총과 발사 방법이 동일하지만 총알을 소모하는 총
     * 
     * 
     */

    private void Awake()
    {
        NumberOfWeapon = 5; // 총 무기 개수

        weaponType = new int[NumberOfWeapon];
        weaponType[0] = 0;
        weaponType[1] = 1;
        weaponType[2] = 1;
        weaponType[3] = 1;
        weaponType[4] = 1;

        weapon = new GameObject[NumberOfWeapon];
        weapon[0] = bullet_Normal;
        weapon[1] = bullet_Fast;
        weapon[2] = bullet_LargeDamage;
        weapon[3] = bullet_Explosion;
        weapon[4] = bullet_Seek;

        bullet = new int[NumberOfWeapon];
        bullet[0] = -1;
        bullet[1] = 0;
        bullet[2] = 0;
        bullet[3] = 10;
        bullet[4] = 10;

        setWeapon(0); // 무기들의 값 초기화 후 마지막에 둘 것
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
        int now = usingWeapon;
        for (int i = now + 1; i < now + NumberOfWeapon + 1; i++)
        {
            int next = i % NumberOfWeapon;
            if (bullet[next] != 0)
            {
                setWeapon(next);
                break;
            }
        }
    }

    // usingWeapon을 변경하고 UI에 적용
    private void setWeapon(int uw)
    {
        usingWeapon = uw;
        int b = bullet[usingWeapon];
        WeaponUI.text = "장착한 무기: " + usingWeapon + "\n" + "[" + (b == -1 ? "∞" : "" + b) + "]";
    }

    public bool useBullet()
    {
        bool returnvalue = false;

        // 총알 사용
        if (bullet[usingWeapon] > 0)
        {
            bullet[usingWeapon] -= 1;
            returnvalue = true;
        }

        // 총알을 다썼다면 무기변경
        if (bullet[usingWeapon] == 0)
        {
            setWeapon(0);
        }
        // 총알을 다 쓰지 않았어도 UI업데이트를 위해 setWeapon 호출
        else
        {
            setWeapon(usingWeapon);
        }

        return returnvalue;
    }

    public void addBullet(int _weaponType, int _amount)
    {
        bullet[_weaponType] += _amount;
        setWeapon(usingWeapon); // UI업데이트
    }
}
