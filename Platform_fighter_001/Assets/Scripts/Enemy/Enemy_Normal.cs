using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Normal : Enemy
{
    /*
     * 이동 처리
     * 피격 처리
     */

    private int nextMove;

    protected bool onHit;
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;


    private void Update()
    {
        
    }

    protected virtual void FixedUpdate()
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

    protected void Think()
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

    protected void OnDie()
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

    protected void DelMonster()
    {
        Destroy(gameObject);
    }

    protected override void OnDamaged(Vector2 collisionDir, float damage)
    {
        // 상태 변경
        onHit = true;

        // HP
        HP -= damage;
        healthBarFilled.fillAmount = HP / maxHP;
        if (HP <= 0.0f)
            OnDie();

        // Animation
        spriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        anim.SetBool("isDamaged", true);
        Invoke("OffDamaged", 0.2f);
    }

    protected void OffDamaged()
    {
        onHit = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetBool("isDamaged", false);
    }
}
