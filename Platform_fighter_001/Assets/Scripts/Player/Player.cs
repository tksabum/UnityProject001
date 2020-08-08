using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : HPObject
{
    public float maxSpeed;
    public float minSpeed;
    public float jumpPower;
    public WeaponManager weaponManager;
    public Image healthBarFilled;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Camera mainCamera;

    private float HP;

    // 연사속도 (Rate of Fire)
    // 총알속도 (Speed of Bullet)
    // 임시로 플레이어의 코드에 둠
    // 무기추가시 무기로 옮기자

    private bool[] isBullet;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        isBullet = new bool[5];
        isBullet[0] = true;
        isBullet[1] = true;
        isBullet[2] = true;
        isBullet[3] = true;
        isBullet[4] = true;

        HP = maxHP;
    }

    private Vector2 CalBulletDirc()
    {
        // 총알발사 방향 계산
        Vector3 MousePosition = Input.mousePosition;
        MousePosition = mainCamera.ScreenToWorldPoint(MousePosition);
        Vector2 direction = MousePosition - transform.position;
        return direction.normalized;
    }

    private GameObject InitBullet(GameObject weapon, int slot)
    {
        // 총알생성위치 지정
        float RX = weaponManager.BulletStartPos[slot].x;
        float RY = weaponManager.BulletStartPos[slot].y;

        weapon.transform.position = new Vector3(transform.position.x + RX, transform.position.y + RY);

        return Instantiate(weapon);
    }

    private void Update()
    {
        // 마우스 좌클릭 (무기 사용)
        if (Input.GetMouseButton(0)) {
            string[] weaponSlot = weaponManager.getWeaponSlot();
            bool[] weaponSlotActivated = weaponManager.getWeaponSlotActivated();

            Vector2 BulletDirc = CalBulletDirc();

            // 활성화된 슬롯의 개수만큼 발사
            for (int index=0; index < weaponManager.NumWeaponSlot; index++)
            {
                if (weaponSlotActivated[index] & isBullet[index])
                {
                    string weaponType = weaponSlot[index];
                    // 무기 준비
                    GameObject weapon = weaponManager.getWeapon(weaponType);
                    // 총알 생성
                    GameObject bullet = InitBullet(weapon, index);

                    float fireDelay = bullet.GetComponent<Bullet>().FireDelay;
                    float bulletSpeed = bullet.GetComponent<Bullet>().Speed;
                    Rigidbody2D bullet_rigid = bullet.GetComponent<Rigidbody2D>();
                    bullet_rigid.velocity = BulletDirc * bulletSpeed;

                    isBullet[index] = false;
                    Invoke("chargeBullet" + index.ToString(), fireDelay);
                }
            }
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.W) && !animator.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        // 감속
        if (!Input.GetButton("Horizontal") && (Mathf.Abs(rigid.velocity.x) < minSpeed))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        

        // 방향 바꾸기
        if (rigid.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rigid.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Animation
        if (rigid.velocity.normalized.x == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void FixedUpdate()
    {
        // 가속
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxSpeed)
        {
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
        }

        // 착지
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null) 
            {
                if (rayHit.distance < 0.6f)
                {
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected override void OnDamaged(Vector2 targetPos, float damage)
    {
        // 레이어 변경
        gameObject.layer = 12;

        // 플레이어 흐려짐
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 플레이어 튕겨남
        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc * 1000f, 7), ForceMode2D.Impulse);

        // 애니메이션 처리
        animator.SetTrigger("doDamaged");

        // 체력, 체력바
        HP -= damage;
        healthBarFilled.fillAmount = HP / maxHP;
        if (HP < 1)
            OnDie();

        // 무적시간 종료처리
        Invoke("OffDamaged", 2);
    }
    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disable
        GetComponent<CapsuleCollider2D>().enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 7, ForceMode2D.Impulse);

        // 부활, 목숨없으면 겜종료
    }

    private void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void chargeBullet0()
    {
        isBullet[0] = true;
    }

    private void chargeBullet1()
    {
        isBullet[1] = true;
    }

    private void chargeBullet2()
    {
        isBullet[2] = true;
    }

    private void chargeBullet3()
    {
        isBullet[3] = true;
    }

    private void chargeBullet4()
    {
        isBullet[4] = true;
    }
}
