using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : HPObject
{
    public float maxSpeed;
    public float minSpeed;
    public float jumpPower;
    public float maxHP;
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

    private bool isBullet;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        isBullet = true;
        HP = maxHP;
    }

    private void Update()
    {
        // 마우스 좌클릭 (무기 사용)
        if (Input.GetMouseButton(0)) {
            if (isBullet)
            {
                int weaponType = weaponManager.getWeaponType();
                if (weaponType == 0)
                {
                    // 사용중인 무기 가져오기
                    GameObject weapon = weaponManager.getWeapon();

                    // 총알생성위치 지정
                    weapon.transform.position = transform.position;

                    // 총알발사 방향 계산
                    Vector3 MousePosition = Input.mousePosition;
                    MousePosition = mainCamera.ScreenToWorldPoint(MousePosition);
                    Vector2 direction = MousePosition - transform.position;
                    direction = direction.normalized;

                    // 총알 생성, 생성된 총알 이동
                    GameObject bullet = Instantiate(weapon);
                    float fireDelay = bullet.GetComponent<Bullet>().FireDelay;
                    float bulletSpeed = bullet.GetComponent<Bullet>().Speed;
                    Rigidbody2D bullet_rigid = bullet.GetComponent<Rigidbody2D>();
                    bullet_rigid.velocity = direction * bulletSpeed;

                    isBullet = false;
                    Invoke("chargeBullet", fireDelay);
                }
                else if (weaponType == 1)
                {
                    // 총알 확인, 소모
                    if (weaponManager.useBullet())
                    {
                        
                    }

                    // 사용중인 무기 가져오기
                    GameObject weapon = weaponManager.getWeapon();

                    // 총알생성위치 지정
                    weapon.transform.position = transform.position;

                    // 총알발사 방향 계산
                    Vector3 MousePosition = Input.mousePosition;
                    MousePosition = mainCamera.ScreenToWorldPoint(MousePosition);
                    Vector2 direction = MousePosition - transform.position;
                    direction = direction.normalized;

                    // 총알 생성, 생성된 총알 이동
                    GameObject bullet = Instantiate(weapon);
                    float fireDelay = bullet.GetComponent<Bullet>().FireDelay;
                    float bulletSpeed = bullet.GetComponent<Bullet>().Speed;
                    Rigidbody2D bullet_rigid = bullet.GetComponent<Rigidbody2D>();
                    bullet_rigid.velocity = direction * bulletSpeed;

                    isBullet = false;
                    Invoke("chargeBullet", fireDelay);
                }
            }
        }
        // 마우스 우클릭 (무기 전환)
        if (Input.GetMouseButtonDown(1))
        {
            weaponManager.nextWeapon();
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

    private void chargeBullet()
    {
        isBullet = true;
    }
}
