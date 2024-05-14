using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    MeshRenderer meshRenderer;
    BoxCollider2D boxCollider;

    
    private float damage = 1f;
    private float isRight = -1;
    private Transform target;
    private float dirc;
    private float dis;
    private bool isGround = true;

    public Transform groundChk;
    public LayerMask g_Layer;
    private float groundChkDis = 0.2f;

    private bool canDmg;
    void Start()
    {
        target = FindObjectOfType<NewController>().transform;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        dis = Vector3.Distance(transform.position, target.position);
    }
    //물리기반은 fixedupadate
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

    public void OnDamaged()
    {
        rigid.velocity = Vector2.zero;
        meshRenderer.materials[0].color = new Color(1, 1, 1, 0.4f);
        Invoke("Regain", 0.1f);
    }

    private void Regain()
    {
        meshRenderer.materials[0].color = new Color(1, 1, 1, 1f);
    }
    void Pursuit()
    {
        anim.SetBool("isMove", true);
        rigid.velocity = new Vector2(dirc * 1, rigid.velocity.y);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col != null)
        {
            if (col.gameObject.layer == 7)
            {
                col.gameObject.GetComponent<NewController>().OnDamaged(transform.position);
                col.gameObject.GetComponent<LivingEntity>().HealthDown(damage);
            }
        }
    }
}
