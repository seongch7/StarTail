using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    MeshRenderer meshRenderer;
    BoxCollider2D boxCollider;
    public int nextMove;//행동지표 결정
    public float isRight = -1;
    private Transform target;
    private float dirc;
    private float dis;
    [SerializeField]
    private bool isGround = true;
    public Transform groundChk;
    public LayerMask g_Layer;
    [SerializeField]
    private float groundChkDis;
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
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
            Invoke("Update", 2f);
        }
        
        if ((dirc > 0 && isRight < 0) || (dirc < 0 && isRight > 0))
        {
            Turn();
        }
    }

    private void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("walkSpeed", nextMove);

        float nextThinkTime = Random.Range(2, 5);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    public void OnDamaged()
    {
        meshRenderer.materials[0].color = new Color(1, 1, 1, 0.4f);
        //meshRenderer.flipY = true;
        boxCollider.enabled = false;
        anim.SetBool("isMove", false);
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("DeActive", 2);
    }
    
    void DeActive()
    {
        gameObject.SetActive(false);
    }

    void Pursuit()
    {
        anim.SetBool("isMove", true);
        rigid.velocity = new Vector2(dirc * 1, rigid.velocity.y);
    }
}
