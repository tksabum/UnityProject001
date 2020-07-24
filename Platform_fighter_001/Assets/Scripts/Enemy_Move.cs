using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    public int nextMove;
    public int HP;
    public Dictionary<string, int> bulletClass =
        new Dictionary<string, int>();
    public bool onHit;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);

        HP = 10;
        onHit = false;
        bulletClass.Add("testBullet1", 1);
        bulletClass.Add("testBullet5", 5);
        bulletClass.Add("testBullet10", 10);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("testBullet"))
        {
            Debug.Log("Hit!");
            string bulletName = collision.gameObject.tag.ToString();
            OnDamaged(collision.transform.position, bulletName);
        }
    }

    void ReduceHP(int damage)
    {

    }

    void OnDamaged(Vector2 targetPos, string bulletName)
    {
        // 상태 변경
        onHit = true;

        // 플레이어 튕겨남
        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        float scalar = 2f;
        rigid.AddForce(new Vector2(dirc * 1000f, 0), ForceMode2D.Impulse);

        // HP
        int damage;
        bulletClass.TryGetValue(bulletName, out damage);
        HP -= damage;

        // Animation
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        anim.SetBool("isDamaged", true);
        Invoke("OffDamaged", 0.2f);
    }

    void OffDamaged()
    {
        onHit = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetBool("isDamaged", false);
    }

    void FixedUpdate()
    {
        // 타격을 받은 상태가 아니라면
        if (!onHit)
        {
            // 이동
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
        
    }

    void Think()
    {
        // 이동방향 결정
        nextMove = Random.Range(-1, 2);

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
