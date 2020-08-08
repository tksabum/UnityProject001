using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int damageType;
    public float FireDelay; // 연사 속도
    public float Speed; // 투사체 속도
    public float EnergyConsumption; // 에너지 소모량

    protected int targetLayer;

    protected const int LAYER_NOTPLAYER = -10;

    protected const int LAYER_PLAYER = 10;
    protected const int LAYER_ENEMY = 11;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 collisionDir = transform.position - collision.gameObject.transform.position;
        collisionDir = collisionDir.normalized;

        if (collision.gameObject.layer == targetLayer || (targetLayer < 0 && collision.gameObject.layer != -targetLayer))
        {
            HPObject target = collision.gameObject.GetComponent<HPObject>();
            attack(target, collisionDir);
        }

        Destroy(gameObject); // 총알 삭제
    }

    protected virtual void attack(HPObject target, Vector2 collisionDir)
    {
        target.attacked(collisionDir, damage, damageType);
    }
}