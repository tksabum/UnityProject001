﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{

    void Update()
    {
        
    }

    // 외부에서 데미지를 줄때 호출하는 함수
    // 이 함수가 호출되면 Enemy 오브젝트의 상태에 따라 적절한 처리를 함
    public void attacked(Vector2 collisionDir, float damage, int damageType)
    {
        Debug.Log("attacked!!");
        OnDamaged(collisionDir, damage);
    }

    protected virtual void OnDamaged(Vector2 collisionDir, float damage)
    {
        // 반드시 비워둘 것
    }
}
