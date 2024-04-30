using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid;

    [SerializeField]
    private float speed;
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
    private float isRight = 1;
    [SerializeField]
    bool isWalk = false;
    [SerializeField]
    bool isRun = false;
    bool isDash = false;
    [SerializeField]
    bool isJump = false;
    bool isDrop = false;
    bool isHang = false;
    bool isWallJmp = false;

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
                    stateChange(State.IDLE);
                }
                break;
            case State.RUN:
                if (!isRun)
                {
                    stateChange(State.IDLE);
                }
                break;
            case State.DASH:
                if (!isDash)
                {
                    stateChange(State.IDLE);
                }
                break;
            case State.DROP:
                if (!isDrop)
                {
                    stateChange(State.IDLE);
                }
                break;
            case State.JUMP:
                if (!isJump)
                {
                    stateChange(State.IDLE);
                }
                break;
            case State.HANG:
                if (!isHang)
                {
                    stateChange(State.IDLE);
                }
                break;
        }
        if (rigid.velocity.y < 0)
        {
            isJump = false;
            isDrop = true;
        }
        isWalk = false;
    }

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

    void stopCounter()
    {
        isDash = false;
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
            }

            if (playerState != State.DASH)
            {
                if (key == KeyCode.X)
                {
                    stateChange(State.DASH);
                }
                else if (key != KeyCode.X)
                {
                    isWalk = true;
                }
            }
            Move(key);

            /*
            if (key == KeyCode.LeftArrow || key == KeyCode.RightArrow)
            {
                isWalk = true;
            }

            if (key == KeyCode.Space)
            {
                stateChange(State.JUMP);
            }
            else if(rigid.velocity.y < 0)
            {
                stateChange(State.DROP);
            }

            if (key == KeyCode.X)
            {
                stateChange(State.DASH);
            }
            */
        }
    }

    void Move(KeyCode key)
    {
        if (isJump && isDash)
        {
            speed = dashSpeed;
            stateChange(State.DASH);
        }
        else if (isJump && isRun)
        {
            speed = runSpeed;
            stateChange(State.JUMP);
        }
        else if(isJump && isWalk)
        {
            speed = walkSpeed;
            stateChange(State.JUMP);
        }
        else if (isJump)
        {
            stateChange(State.JUMP);
        }
        else if (playerState == State.DASH)
        {
            speed = dashSpeed;
        }
        else if (isRun)
        {
            stateChange(State.RUN);
        }
        else if (isWalk)
        {
            stateChange(State.WALK);
        }

        if(isDrop && isDash)
        {
            speed = dashSpeed;
            stateChange(State.DASH);
        }
        else if (isDrop)
        {
            stateChange(State.DROP);
        }

        if (key == KeyCode.Space)
        {
            if (jumpCount < 2)
            {
                jumpCount++;
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0, jumpForce));
            }
        }

        if (key == KeyCode.LeftArrow)
        {
            if (isRight != 1)
                Flip();
            rigid.velocity = new Vector2(speed * -1, rigid.velocity.y);
        }
        else if (key == KeyCode.RightArrow)
        {
            if (isRight == 1)
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
}
