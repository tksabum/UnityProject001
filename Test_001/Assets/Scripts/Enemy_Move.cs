using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    public int nextMove;
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 지형확인
        Vector2 frontVec = new Vector2(rigid.position.x + 0.5f * nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            nextMove = -nextMove;
            animator.SetInteger("WalkSpeed", nextMove);
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
        animator.SetInteger("WalkSpeed", nextMove);
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove > 0;
        }

        // 재귀호출
        Invoke("Think", Random.Range(2.0f, 5.0f));
    }
}
