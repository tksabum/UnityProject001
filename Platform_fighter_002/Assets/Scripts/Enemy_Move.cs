using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Move : MonoBehaviour
{
    // 이동 관련 변수
    public int nextMove, headDirc;
    public int dashRange;
    public float dashPoint;
    public bool onDash, onDashing, onHit;

    // 데미지 관련 변수
    private int HP;
    public int maxHP;
    public int enemyDamage;
    public GameObject healthBarBackground;
    public Image healthBarFilled;

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
        onDash = false;
        dashRange = 3;

        nextMove = -1;
        headDirc = -1;

        healthBarFilled.fillAmount = 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)        // 총알
        {
            Debug.Log("Hit!");
            Bullet_Normal bullet = collision.gameObject.GetComponent<Bullet_Normal>();
            OnDamaged(collision.transform.position, bullet.damage);
        }
        else if (collision.gameObject.layer == 10)  // Player 
        {
            Debug.Log("공격받음!");
            Player_Move player = collision.gameObject.GetComponent<Player_Move>();
            player.OnDamaged(collision.transform.position, enemyDamage);
        }
    }

    void OnDamaged(Vector2 targetPos, int bulletDamage)
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

    void FixedUpdate()
    {
        // 타격, 대쉬 상태가 아니라면
        if (!onHit && !onDash)
        {
            // 일반 이동
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }
        // 대쉬 상태
        else if (onDashing)
        {
            // 대쉬 종료
            if (Mathf.Abs(dashPoint - rigid.position.x) > dashRange)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                OffDash();
            }
            // 대쉬
            else
                rigid.velocity = new Vector2(headDirc * 10, rigid.velocity.y);
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
        if (!onDash)
        {
            RaycastHit2D PlayerHit = PlayerInRange();
            if (PlayerHit.collider != null)
            {
                Debug.Log("플레이어 발견!");
                // 대쉬 모드 시작
                onDash = true;
                Invoke("OnDash", 2);

                // 대쉬 모드에서는 다른 생각 x
                CancelInvoke("Think");
            }
        }   
    }

    void OnDash()
    {
        Debug.Log("대쉬 시작!");
        dashPoint = rigid.position.x;
        onDashing = true;
    }

    void OffDash()
    {
        Debug.Log("대쉬 끝!");
        onDashing = false;
        onDash = false;

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
        Debug.DrawRay(PlayerVec, vDirc * dashRange, new Color(1, 0, 0));
        RaycastHit2D PlayerHit = Physics2D.Raycast(PlayerVec, vDirc, dashRange, LayerMask.GetMask("Player"));

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
