using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gun : Enemy_Normal
{
    public GameObject weapon;

    private bool onShoot;

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
        //Gun 코드
        onShoot = false;
        setNextMove(-1);
        ////////////////////

        Invoke("Think", 5);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isDie) return;

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
        // 총알생성위치 지정
        weapon.transform.position = transform.position;

        // 총알발사 방향 계산
        Vector2 direction = new Vector2(headDirc, 0);
        direction = direction.normalized;

        // 총알 생성, 생성된 총알 이동
        GameObject go = Instantiate(weapon);
        Rigidbody2D bullet_rigid = go.GetComponent<Rigidbody2D>();
        bullet_rigid.velocity = direction * weapon.GetComponent<Bullet>().Speed;

        Invoke("chargeBullet", weapon.GetComponent<Bullet>().FireDelay);
    }

    void chargeBullet()
    {
        onShoot = false;

        // 다시 생각 시작
        Invoke("Think", 2);
    }
}
