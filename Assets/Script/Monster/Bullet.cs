using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(gameObject, 10f);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController controller = other.GetComponent<PlayerController>();

            if(controller != null)
            {
                controller.OnDamaged(transform.position);
            }
        }
    }
}
