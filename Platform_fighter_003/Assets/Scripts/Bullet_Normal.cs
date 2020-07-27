using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Normal : MonoBehaviour
{
    CircleCollider2D bulletCollider;
    public int damage;
    public int damageType;

    private void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 레이어 :" + collision.gameObject.layer);
        // 상속활용해서 체력을 가진 오브젝트라면 이벤트가 발생하도록 하면 좋을듯

        if (collision.gameObject.layer == 10)       // layer 10 : Player
        {
            collision.gameObject.GetComponent<Player_Move>().OnDamaged(collision.transform.position, damage, damageType);
        }
        else if (collision.gameObject.layer == 11)  // layer 11 : Enemy
        {
            if (collision.gameObject.tag == "EnemyFist")
                collision.gameObject.GetComponent<Enemy_Move>().OnDamaged(collision.transform.position, damage, damageType);
            else if (collision.gameObject.tag == "EnemyGun")
                collision.gameObject.GetComponent<Enemy_Gun>().OnDamaged(collision.transform.position, damage, damageType);
        }

        Destroy(gameObject); // 총알 삭제
    }
}
