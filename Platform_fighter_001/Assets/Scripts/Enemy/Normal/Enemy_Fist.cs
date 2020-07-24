using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Fist : Enemy_Normal
{
    private void Awake()
    {
        healthBarFilled.fillAmount = 1.0f;
        HP = maxHP;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onHit = false;

        Invoke("Think", 5);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Fist 동작 코드 추가할 위치
    }

}
