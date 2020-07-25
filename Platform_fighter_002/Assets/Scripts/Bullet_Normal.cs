using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Normal : MonoBehaviour
{
    CircleCollider2D bulletCollider;
    public int damage;

    private void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collsion" + collision.gameObject.name);
        // 상속활용해서 체력을 가진 오브젝트라면 이벤트가 발생하도록 하면 좋을듯


        Destroy(gameObject); // 총알 삭제
    }
}
