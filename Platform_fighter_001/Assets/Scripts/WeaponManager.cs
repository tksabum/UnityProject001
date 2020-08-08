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
    private Dictionary<string, GameObject> weapon;
    private int[] weaponType;
    private int NumberOfWeapon;
    private int bullet;

    // 무기 슬롯 관리

    /* 
     * 1번 - 주먹에서 나가는
     * 2번 - 어깨에서 나가는
     * 3번 - 등에 달린 기계총에서 나가는
     * 4번 - 미사일 1번
     * 5번 - 미사일 2번
     * 
     * 각 슬롯에 보유한 무기 중 원하는 무기를 달아서 게임 시작
     * 게임 진행 중 슬롯별로 활성화/비활성화 가능
     * 어떤 슬롯에서 사용하는 총알이 없을 경우 해당 슬롯 비활성화
     */

    private string[] weaponSlot;    // 무기 종류 저장
    private bool[] weaponSlotActivated;  // 활성화되어있는 슬롯 표시

    public Vector2[] BulletStartPos;
    public int NumWeaponSlot;
    public Toggle[] weaponSlotUI;

    PlatformFighter platformfighter;

    /*
     * Weapon type
     * 
     * type 0
     *  사용할 때 총알을 소모하지 않는 기본 총
     * 
     * type 1
     *  기본 총과 발사 방법이 동일하지만 총알을 소모하는 총
     */

    private void Awake()
    {
        NumberOfWeapon = 5; // 총 무기 개수
        NumWeaponSlot = 5;  // 무기 슬롯 개수

        platformfighter = PlatformFighter.getPlatformFighter();
        weaponSlot = platformfighter.getPlayerInfo().WeaponSlots;
        weaponSlotActivated = new bool[NumWeaponSlot];

        // 슬롯별 총알 생성 위치를 다르게 하기 위함
        BulletStartPos = new Vector2[5];

        BulletStartPos[0] = new Vector2(0, 0);
        BulletStartPos[1] = new Vector2(-0.1f, 0.2f);
        BulletStartPos[2] = new Vector2(-0.1f, 0.4f);
        BulletStartPos[3] = new Vector2(-0.1f, 0.6f);
        BulletStartPos[4] = new Vector2(-0.1f, 0.8f);

        for (int i = 0; i < NumberOfWeapon; i++)
        {
            if (weaponSlot[i] != null)
            {
                weaponSlotActivated[i] = true;
                weaponSlotUI[i].isOn = true;
            }
            else
            {
                weaponSlotActivated[i] = false;
                weaponSlotUI[i].isOn = false;
            }
        }

        weaponType = new int[NumberOfWeapon];
        weaponType[0] = 0;
        weaponType[1] = 1;
        weaponType[2] = 1;
        weaponType[3] = 1;
        weaponType[4] = 1;

        weapon = new Dictionary<string, GameObject>()
        {
            {"bullet_Normal",  bullet_Normal },
            {"bullet_Fast",  bullet_Fast },
            {"bullet_LargeDamage",  bullet_LargeDamage },
            {"bullet_Explosion",  bullet_Explosion },
            {"bullet_Seek",  bullet_Seek }
        };

        // 총알 준비
        bullet = 10000;

        UpdateUI(); // 무기들의 값 초기화 후 마지막에 둘 것
    }

    public string[] getWeaponSlot()
    {
        return weaponSlot;
    }

    public bool[] getWeaponSlotActivated()
    {
        return weaponSlotActivated;
    }


    public void setWeaponSlotActivated()
    {
        // weaponSlotActivated;
        for (int i = 0; i < NumberOfWeapon; i++)
        {
            weaponSlotActivated[i] = weaponSlotUI[i].isOn;
        }
    }

    private void Update()
    {

    }

    public GameObject getWeapon(string weaponName)
    {
        return weapon[weaponName];
    }


    private void UpdateUI()
    {
        string UItext = "";
        for (int i = 0; i < NumberOfWeapon; i++)
        {
            if (weaponSlotActivated[i])
            {
                UItext += "무기슬롯 " + (i+1).ToString() + " 에너지 소모량 :" + getWeapon(weaponSlot[i]) + "\n";
            }
        }
        WeaponUI.text = UItext;
    }

    public bool useBullet(int comsumption)
    {
        if (bullet >= comsumption)
        {
            bullet -= comsumption;
            return true;
        }
        return false;
    }

    public void addBullet(int _amount)
    {
        bullet += _amount;
        UpdateUI();
    }
}