using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy_Normal
{
    public GameObject weapon;

    private bool onShoot;
    private const int phaseDelay = 7, bulletSpread = 6;
    private const float delay = 0.1f;
    private int FireCount;
    private List<Vector2> EveryDirections;

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
        FireCount = 0;
        EveryDirections = new List<Vector2>();
        EveryDirections.Add(new Vector2(-1, -1));
        EveryDirections.Add(new Vector2(-1, 0));
        EveryDirections.Add(new Vector2(-1, 1));
        EveryDirections.Add(new Vector2(0, -1));
        EveryDirections.Add(new Vector2(0, 1));
        EveryDirections.Add(new Vector2(1, -1));
        EveryDirections.Add(new Vector2(1, 0));
        EveryDirections.Add(new Vector2(1, 1));

        Invoke("OnFire", weapon.GetComponent<Bullet>().FireDelay);
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
    }

    void OnFire()
    {
        if (onShoot) return;
        // 총알생성위치 지정
        weapon.transform.position = transform.position;

        foreach (Vector2 dirc in EveryDirections)
        {
            // 총알 생성, 생성된 총알 이동
            GameObject go = Instantiate(weapon);
            Rigidbody2D bullet_rigid = go.GetComponent<Rigidbody2D>();
            bullet_rigid.velocity = dirc * weapon.GetComponent<Bullet>().Speed;
        }

        FireCount += 1;
        if (FireCount < bulletSpread) 
            Invoke("OnFire", delay);
        else
        {
            Invoke("OnFire", phaseDelay);
            FireCount = 0;
        }
    }
}
