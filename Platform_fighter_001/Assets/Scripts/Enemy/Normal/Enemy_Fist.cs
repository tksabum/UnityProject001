using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Fist : Enemy_Normal
{
    public float damage;
    public int damageType;

    private float dashPoint;
    private bool onDash, onDashing;

    private void Awake()
    {
        healthBarFilled.fillAmount = 1.0f;
        HP = maxHP;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onHit = false;
        stop = false;
        isDie = false;

        ////////////////////
        //Fist 코드
        onDash = false;
        onDashing = false;
        setNextMove(-1);
        ////////////////////
        
        Invoke("Think", 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)  // Player 
        {
            HPObject player = collision.gameObject.GetComponent<HPObject>();
            player.attacked(collision.transform.position, damage, damageType);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 대쉬 상태
        if (onDash && onDashing)
        {
            // 대쉬 종료
            if (Mathf.Abs(dashPoint - rigid.position.x) > detectRange)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                OffDash();
            }
            // 대쉬
            else
                rigid.velocity = new Vector2(headDirc * 10, rigid.velocity.y);
        }

        // 플레이어 탐색
        if (!onDash)
        {
            RaycastHit2D PlayerHit = PlayerInRange();
            if (PlayerHit.collider != null)
            {
                // 대쉬 모드 시작
                onDash = true;
                stop = true;
                Invoke("OnDash", 2);

                // 대쉬 모드에서는 다른 생각 x
                CancelInvoke("Think");
            }
        }
    }

    void OnDash()
    {
        dashPoint = rigid.position.x;
        onDashing = true;
    }

    void OffDash()
    {
        onDashing = false;
        onDash = false;
        stop = false;

        // 다시 생각 시작
        Invoke("Think", 2);
    }

    protected override void changeDirEvent()
    {
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        OffDash();
    }
}
