using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;

    private float isRight = -1;
    private Transform target;
    private float dirc;
    private float dis;
    private bool isGround = true;

    public Transform groundChk;
    public LayerMask g_Layer;
    private float groundChkDis = 0.2f;

    void Awake()
    {
        target = FindObjectOfType<PlayerController>().transform;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        dis = Vector3.Distance(transform.position, target.position);
    }
    
    void FixedUpdate()
    {
        dirc = target.position.x - transform.position.x;
        dirc = (dirc < 0) ? -1 : 1;

        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundChkDis, g_Layer);

        if(isGround)
        {
            if (dis <= 10)
            {
                Pursuit();
            }
            else
            {
                anim.SetBool("isMove", false);
                rigid.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            Turn();
            rigid.velocity = new Vector2(dirc * -1, rigid.velocity.y);
        }
        
        if ((dirc > 0 && isRight < 0) || (dirc < 0 && isRight > 0))
        {
            Turn();
        }
    }

    void Turn()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    void Pursuit()
    {
        anim.SetBool("isMove", true);
        rigid.velocity = new Vector2(dirc * 1, rigid.velocity.y);
    }
}
