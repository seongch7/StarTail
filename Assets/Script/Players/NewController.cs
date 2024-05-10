using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NewController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid;
    private SkeletonAnimation mySkeleton;

    private float damage = 5f;
    private float speed = 4f;
    private float walkSpeed = 4f;
    private float runSpeed = 8f;
    private float dashSpeed = 16f;
    private float jumpForce = 400;
    private float wallJmpForce = 400;
    private int jumpCount = 0;
    private float isRight = -1;

    bool isWalk = false;
    bool isRun = false;
    bool isDash = false;
    bool isJump = false;
    bool isDrop = false;
    bool isHang = false;
    bool isAttack = false;
    bool isWallJmp = false;
    bool isWall = false;

    bool canDmg = false;
    bool canMove = true;
    bool canDamaged = true;

    private float wallChkDis = 0.15f;
    public Transform wallChk;
    public LayerMask w_Layer;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    List<KeyCode> keys = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space, KeyCode.X, KeyCode.LeftShift, KeyCode.Z };

    public enum State
    {
        IDLE,
        WALK,
        RUN,
        DASH,
        JUMP,
        DROP,
        HANG,
        ATTACK
    }

    public State playerState;
    private string currentState;

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        mySkeleton = GetComponent<SkeletonAnimation>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        playerState = State.IDLE;
    }

    private void Update()
    {
        switch (playerState)
        {
            case State.IDLE:
                break;
            case State.WALK:
                if (!isWalk)
                {
                    setCurrentState(State.IDLE);
                }
                break;
            case State.RUN:
                if (!isRun)
                {
                    setCurrentState(State.IDLE);
                }
                break;
            case State.DASH:
                if (!isDash)
                {
                    if (isJump)
                    {
                        setCurrentState(State.JUMP);
                    }
                    else if (isDrop)
                    {
                        setCurrentState(State.DROP);
                    }
                    else
                        setCurrentState(State.IDLE);
                }
                break;
            case State.DROP:
                if (!isDrop)
                {
                    setCurrentState(State.IDLE);
                }
                break;
            case State.JUMP:
                if (!isJump)
                {
                    setCurrentState(State.DROP);
                }
                break;
            case State.HANG:
                if (!isHang)
                {
                    setCurrentState(State.IDLE);
                }
                break;
            case State.ATTACK:
                if (!isAttack)
                {
                    setCurrentState(State.IDLE);
                }
                break;
        }

        if (rigid.velocity.y < 0)
        {
            isJump = false;
            isDrop = true;
        }

        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallChkDis, w_Layer);
        isWalk = false;

        if (isWall)
        {
            isWallJmp = false;
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            setCurrentState(State.HANG);
        }
    }

    private void AscnAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        if (animClip.name.Equals(currentState))
            return;

        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        currentState = animClip.name;
    }

    private void setCurrentState(State state)
    {
        switch (state)
        {
            case State.IDLE:
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                playerState = State.IDLE;
                AscnAnimation(AnimClip[(int)state], true, 1f);
                break;
            case State.WALK:
                playerState = State.WALK;
                AscnAnimation(AnimClip[(int)state], true, 1.5f);
                break;
            case State.RUN:
                playerState = State.RUN;
                AscnAnimation(AnimClip[(int)state], true, 2f);
                break;
            case State.DASH:
                playerState = State.DASH;
                AscnAnimation(AnimClip[(int)state], true, 3f);
                Invoke("stopCounter", 0.2f);
                break;
            case State.JUMP:
                playerState = State.JUMP;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
            case State.DROP:
                isDrop = true;
                playerState = State.DROP;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
            case State.HANG:
                isHang = true;
                playerState = State.HANG;
                AscnAnimation(AnimClip[(int)state], true, 1f);
                break;
            case State.ATTACK:
                isAttack = true;
                playerState = State.ATTACK;
                rigid.velocity = new Vector2(0,rigid.velocity.y);
                AscnAnimation(AnimClip[(int)state], false, 2f);
                Invoke("CanDmg", 0.2f);
                Invoke("stopCounter", 0.3f);
                break;
        }
    }

    void OnKeyboard(KeyCode key)
    {
        if (!canMove)
            return;

        if (keys.Contains(key))
        {
            if (key == KeyCode.LeftShift)
            {
                isRun = !isRun;
            }

            if (playerState != State.JUMP)
            {
                if (key == KeyCode.Space)
                {
                    isJump = true;
                }
                else if (isHang)
                    return;
                else if(playerState != State.DROP)
                {
                    if (playerState != State.ATTACK)
                    {
                        if (key == KeyCode.Z)
                        {
                            setCurrentState(State.ATTACK);
                            return;
                        }
                        else if (playerState != State.DASH)
                        {
                            if (key != KeyCode.X)
                            {
                                isWalk = true;
                            }
                        }
                    }
                }
            }
            Move(key);
        }
    }

    void Move(KeyCode key)
    {
        if (isWallJmp || isDash || isAttack)
            return;

        
        if (isJump)
        {
            setCurrentState(State.JUMP);

            if (key == KeyCode.X)
            {
                isDash = true;
                rigid.velocity = new Vector2(isRight * dashSpeed, rigid.velocity.y);
                setCurrentState(State.DASH);
            }
            else if (isRun)
            {
                speed = runSpeed;
            }
            else if (isWalk)
            {
                speed = walkSpeed;
            }
        }
        else if (isDrop)
        {
            setCurrentState(State.DROP);

            if (key == KeyCode.X)
            {
                isDash = true;
                rigid.velocity = new Vector2(isRight * dashSpeed, rigid.velocity.y);
                setCurrentState(State.DASH);
            }
            else if (isRun)
            {
                speed = runSpeed;
            }
            else if (isWalk)
            {
                speed = walkSpeed;
            }
        }
        else
        {
            if (key == KeyCode.X)
            {
                isDash = true;
                rigid.velocity = new Vector2(isRight * dashSpeed, rigid.velocity.y);
                setCurrentState(State.DASH);
            }
            else if (rigid.velocity.x == 0)
            {
                setCurrentState(State.IDLE);
            }
            else if (isRun)
            {
                speed = runSpeed;
                setCurrentState(State.RUN);
            }
            else if (isWalk)
            {
                speed = walkSpeed;
                setCurrentState(State.WALK);
            }
        }
        

        if (key == KeyCode.Space)
        {
            if (isHang)
            {
                isWallJmp = true;
                Invoke("FreezeX", 0.3f);
                rigid.velocity = new Vector2(-isRight * wallJmpForce, 0.9f * wallJmpForce);
                Flip();
            }
            if (jumpCount < 2)
            {
                jumpCount++;
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0, jumpForce));
            }
        }

        if (key == KeyCode.LeftArrow)
        {
            if (isRight == 1)
                Flip();

            rigid.velocity = new Vector2(speed * -1, rigid.velocity.y);
        }
        else if (key == KeyCode.RightArrow)
        {
            if (isRight != 1)
                Flip();

            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
    }
    private void Flip()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y > 0.7f)
        {
            isDrop = false;
            jumpCount = 0;
        }
    }

    private void CanDmg()
    {
        canDmg = true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col != null)
        {
            if (col.CompareTag("Enemy") && canDmg)
            {
                col.gameObject.GetComponent<MonsterMove>().OnDamaged();
                col.gameObject.GetComponent<LivingEntity>().HealthDown(damage);
                canDmg = false;
            }
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (!canDamaged)
            return;

        mySkeleton.skeleton.SetColor(Color.red);
        canDamaged = false;
        canMove = false;

        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        if (dirc == isRight)
            Flip();

        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(dirc, 0.5f) * 4, ForceMode2D.Impulse);
        setCurrentState(State.DROP);

        Invoke("Regain", 0.1f);
        Invoke("CanMove", 0.5f);
        Invoke("OffDamaged", 2);
    }

    private void Regain()
    {
        mySkeleton.skeleton.SetColor(Color.white);
    }
    private void CanMove()
    {
        canMove = true;
    }
    private void OffDamaged()
    {
        meshRenderer.material.color = new Color(1, 1, 1, 1f);
        canDamaged = true;
    }

    private void FreezeX()
    {
        isWallJmp = false;
        isHang = false;
    }
    void stopCounter()
    {
        isAttack = false;
        isDash = false;
        canDmg = false;
        setCurrentState(State.IDLE);
    }
}