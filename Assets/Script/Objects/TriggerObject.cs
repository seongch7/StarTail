using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField]
    private CapsuleCollider2D playerCollider;
    [SerializeField]
    private CircleCollider2D objectCollider;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(playerCollider, objectCollider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float dirc = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc * 1f, 1f), ForceMode2D.Impulse);
        }
    }
}
