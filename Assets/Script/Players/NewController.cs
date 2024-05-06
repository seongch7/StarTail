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

    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float walkSpeed = 4f;
    [SerializeField]
    private float runSpeed = 8f;
    [SerializeField]
    private float dashSpeed = 16f;
    [SerializeField]
    private float jumpForce = 400;
    [SerializeField]
    private float wallJmpForce = 400;
    [SerializeField]
    private int jumpCount = 0;
    [SerializeField]
    private float isRight = -1;
    [SerializeField]
    bool isWalk = false;
    [SerializeField]
    bool isRun = false;
    [SerializeField]
    bool isDash = false;
    [SerializeField]
    bool isJump = false;
    [SerializeField]
    bool isDrop = false;
    bool isHang = false;
    bool isWallJmp = false;
    [SerializeField]
    bool isWall = false;

    private float wallChkDis = 0.15f;
    public Transform wallChk;
    public LayerMask w_Layer;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    List<KeyCode> keys = new List<KeyCode>() { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space, KeyCode.X, KeyCode.LeftShift};

    public enum State
    {
        IDLE,
        WALK,
        RUN,
        DASH,
        JUMP,
        DROP,
        HANG
    }

    public State playerState;
    private string currentState;

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        playerState = State.IDLE;
    }

    private void Update()
    {
        switch(playerState)
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
                rigid.velocity = Vector3.zero;
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
                isDash = true;
                playerState = State.DASH;
                AscnAnimation(AnimClip[(int)state], true, 3f);
                break;
            case State.JUMP:
                playerState = State.JUMP;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
            case State.DROP:
                playerState = State.DROP;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
            case State.HANG:
                isHang = true;
                playerState = State.HANG;
                AscnAnimation(AnimClip[(int)state], true, 1f);
                break;
        }
    }
    void stopCounter()
    {
        isDash = false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }
    void OnKeyboard(KeyCode key)
    {
        if (keys.Contains(key))
        {
            

            if(key == KeyCode.LeftShift)
            {
                isRun = !isRun;
            }

            if(playerState != State.JUMP)
            {
                if (key == KeyCode.Space)
                {
                    isJump = true;
                }
                else if (isHang)
                    return;
            }

            if (playerState != State.DASH)
            {
                if (key == KeyCode.X)
                {
                    isDash = true;
                    Invoke("stopCounter", 0.2f);
                }
                else if (key != KeyCode.X)
                {
                    isWalk = true;
                }
            }
            Move(key);
        }
    }

    void Move(KeyCode key)
    {
        if (isWallJmp)
            return;
        if (isJump)
        {
            setCurrentState(State.JUMP);

            if (isDash)
            {
                if (key != KeyCode.None)
                    return;
                speed = dashSpeed;
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

            if (isDash)
            {
                if (key != KeyCode.None)
                    return;
                speed = dashSpeed;
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
            if(rigid.velocity.x == 0)
            {
                setCurrentState(State.IDLE);
            }
            else if (isDash)
            {
                speed = dashSpeed;
                setCurrentState(State.DASH);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isDrop = false;
            jumpCount = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallChkDis);
    }

    private void FreezeX()
    {
        isWallJmp = false;
        isHang = false;
    }

    /*
    private void stateChange(State state)
    {
        switch(state)
        {
            case State.IDLE:
                playerState = State.IDLE;
                speed = 0f;
                animator.SetInteger("state", 0);
                break;
            case State.WALK:
                playerState = State.WALK;
                speed = walkSpeed;
                animator.SetInteger("state", 1);
                isWalk = true;
                break;
            case State.RUN:
                playerState = State.RUN;
                speed = runSpeed;
                animator.SetInteger("state", 2);
                isRun = true;
                break;
            case State.JUMP:
                playerState = State.JUMP;
                animator.SetInteger("state", 3);
                break;
            case State.DROP:
                playerState = State.DROP;
                animator.SetInteger("state", 4);
                isDrop = true;
                break;
            case State.DASH:
                playerState = State.DASH;
                animator.SetInteger("state", 5);
                isDash = true;
                Invoke("stopCounter", 0.2f);
                break;
            case State.HANG:
                playerState = State.HANG;
                animator.SetInteger("state", 6);
                break;
        }
    }
    */
}
