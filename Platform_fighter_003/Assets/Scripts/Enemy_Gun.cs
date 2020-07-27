using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Gun : MonoBehaviour
{
    // 이동 관련 변수
    public int nextMove, headDirc;
    public int dectectRange;
    public bool onHit, onShoot;

    // 데미지 관련 변수
    public int HP;
    public int maxHP;
    public int enemyDamage, damageType;
    public GameObject healthBarBackground;
    public Image healthBarFilled;

    // 사격 제어
    public GameObject bullet_Normal;
    public float ROF_Normal; // 연사 속도
    public float SOB_Normal; // 투사체 속도

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);

        HP = maxHP;
        onHit = false;
        onShoot = false;

        nextMove = -1;
        headDirc = -1;

        healthBarFilled.fillAmount = 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)  // Player 
        {
            Debug.Log("공격받음!");
            Player_Move player = collision.gameObject.GetComponent<Player_Move>();
            player.OnDamaged(collision.transform.position, enemyDamage, damageType);
        }
    }

    public void OnDamaged(Vector2 targetPos, int bulletDamage, int damageType)
    {
        // 상태 변경
        onHit = true;

        // HP
        HP -= bulletDamage;
        healthBarFilled.fillAmount = (float)HP / maxHP;
        if (HP < 1)
            OnDie();

        // Animation
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        anim.SetBool("isDamaged", true);
        CancelInvoke("OffDamaged");
        Invoke("OffDamaged", 0.2f);
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

        Invoke("DelMonster", 5);
    }

    void DelMonster()
    {
        Destroy(gameObject);
    }

    void OffDamaged()
    {
        onHit = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetBool("isDamaged", false);
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        // 타격 상태가 아니라면
        if (!onHit)
        {
            // 일반 이동
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }

        // 지형확인
        Vector2 frontVec = new Vector2(rigid.position.x + 0.5f * nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            nextMove = -nextMove;
            anim.SetInteger("WalkSpeed", nextMove);
            if (nextMove != 0)
            {
                spriteRenderer.flipX = nextMove > 0;
            }
        }

        // 플레이어 탐색
        if (!onShoot)
        {
            RaycastHit2D PlayerHit = PlayerInRange();
            if (PlayerHit.collider != null)
            {
                Debug.Log("플레이어 발견!");
                // 슈팅 모드 시작
                onShoot = true;
                // 슈팅 모드에서는 다른 생각 x
                CancelInvoke("Think");
                OnFire();
            }
        }
    }

    void OnFire()
    {
        Debug.Log("사격 시작!");
        // 총알생성위치 지정
        bullet_Normal.transform.position = transform.position;

        // 총알발사 방향 계산
        Vector2 direction = new Vector2(headDirc, 0);
        direction = direction.normalized;

        // 총알 생성, 생성된 총알 이동
        GameObject go = Instantiate(bullet_Normal);
        Rigidbody2D bullet_rigid = go.GetComponent<Rigidbody2D>();
        bullet_rigid.velocity = direction * SOB_Normal;

        Invoke("chargeBullet", ROF_Normal);
    }

    void chargeBullet()
    {
        Debug.Log("사격 끝!");
        onShoot = false;

        // 다시 생각 시작
        Invoke("Think", 2);
    }

    RaycastHit2D PlayerInRange()
    {
        Vector2 PlayerVec = new Vector2(rigid.position.x + 0.5f * headDirc, rigid.position.y);

        Vector3 vDirc;

        if (headDirc > 0)
            vDirc = Vector3.right;
        else
            vDirc = Vector3.left;
        Debug.DrawRay(PlayerVec, vDirc * dectectRange, new Color(1, 0, 0));
        RaycastHit2D PlayerHit = Physics2D.Raycast(PlayerVec, vDirc, dectectRange, LayerMask.GetMask("Player"));

        return PlayerHit;
    }

    void Think()
    {
        // 이동방향 결정
        nextMove = Random.Range(-1, 2);
        if (nextMove != 0)
            headDirc = nextMove;

        // 애니메이션 처리
        anim.SetInteger("WalkSpeed", nextMove);
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove > 0;
        }

        // 재귀호출
        Invoke("Think", Random.Range(2.0f, 5.0f));
    }
}
