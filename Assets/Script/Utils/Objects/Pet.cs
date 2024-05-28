using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Pet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float distance; //추적 시작할 거리
    [SerializeField]
    private float telDistance; //텔포할 거리
    [SerializeField]
    private float runDis; // 달리기 시작할 거리
    [SerializeField]
    public float chkDistance;
    [SerializeField]
    private Transform wallChk;
    private int jumpCount = 0;
    private float dirc; // 로타랑 홍이 사이 거리, 이동 방향
    private float absDirc; // 로타랑 홍이 사이 거리 절댓값

    private float isRight = 1;
    private bool hit;
    private bool hitDia;
    
    Transform player;
    Animator anim;
    Rigidbody2D rb;
    public LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Rota3").transform;
        anim = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(7, 10);
    }


    void FixedUpdate()
    {
        dirc = player.position.x - transform.position.x;
        absDirc = Mathf.Abs(dirc);
        dirc = (dirc < 0) ? -1 : 1;

        if (absDirc > distance)
        {
            if(absDirc > runDis)
            {
                rb.velocity = new Vector2(dirc * speed * 2, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(dirc * speed, rb.velocity.y);
            }

            DirectionPet(); // 바라보는 방향 조정
            
            hit = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, chkDistance, groundLayer);
            hitDia = Physics2D.Raycast(wallChk.position, new Vector2(1 * DirectionPet(), 1), chkDistance, groundLayer);
            
            if (player.position.y - transform.position.y <= 0)
                hitDia = new RaycastHit2D(); // 반환값 비우기
            
            if (hit || hitDia)
            {
                if(jumpCount < 2)
                {
                    jumpCount++;
                    rb.velocity = Vector2.up * jumpPower;
                }
            }
        }

        Animation();
        if(Vector2.Distance(player.position, transform.position) > telDistance)
        {
            transform.position = player.position;
            //tel.gameObject.SetActive(true);
            //tel.Play();
        }
        /*if (!tel.isPlaying)
        {
            tel.gameObject.SetActive(false);
        }*/
    }

    //public ParticleSystem tel;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            jumpCount = 0;
        }
    }

    float DirectionPet()
    {
        if(transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isRight = isRight * -1;
            return 1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isRight = isRight * -1;
            return -1;
        }
    }

    void Animation()
    {
        if(Mathf.Abs(rb.velocity.x) >= Mathf.Abs(dirc * speed * 1.5f))
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isWalk", false);
        }
        else if (rb.velocity.normalized.x != 0)
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }
            

        if (rb.velocity.y > 0)
            anim.SetBool("isJump", true);
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isDrop", true);
        }
        else
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isDrop", false);
        }
    }
}
