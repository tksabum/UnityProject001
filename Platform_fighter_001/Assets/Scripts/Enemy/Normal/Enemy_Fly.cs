using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly : Enemy_Normal
{
    public GameObject weapon;

    // 사격 관련
    private bool onShoot;
    private const int phaseDelay = 7, bulletSpread = 6;
    private const float delay = 0.1f;
    private int FireCount;
    // 이동 관련
    private int UnitSpeed, MoveRange;
    private float initPosX;

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
        //Fly 코드
        onShoot = false;
        setNextMove(-1);
        FireCount = 0;
        UnitSpeed = 7;
        MoveRange = 8;
        initPosX = rigid.position.x;
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
    }

    protected void FixedUpdate()
    {
        if (isDie) return;

        // 타격 상태가 아니라면
        if (!onHit)
        {
            // 일반 이동
            rigid.velocity = new Vector2(nextMove * UnitSpeed, rigid.velocity.y);
        }

        if (rigid.position.x < initPosX - MoveRange)
        {
            setNextMove(1);
            spriteRenderer.flipX = nextMove > 0;
        }
        else if (rigid.position.x > initPosX + MoveRange)
        {
            setNextMove(-1);
            spriteRenderer.flipX = nextMove > 0;
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
