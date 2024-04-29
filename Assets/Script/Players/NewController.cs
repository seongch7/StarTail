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
            case State.JUMP:
                if (!isJump)
                {
                    stateChange(State.IDLE);
                }
                break;
            case State.DROP:
                if(!isDrop)
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
        isWalk = false;
    }

    private void stateChange(State state)
    {
        switch(state)
        {
            case State.IDLE:
                playerState = State.IDLE;
                speed = 0f;
                break;
            case State.WALK:
                playerState = State.WALK;
                speed = walkSpeed;
                isWalk = true;
                break;
            case State.RUN:
                playerState = State.RUN;
                speed = runSpeed;
                isRun = true;
                break;
            case State.DASH:
                playerState = State.DASH;
                isDash = true;
                Invoke("stopCounter", 0.2f);
                break;
            case State.JUMP:
                playerState = State.JUMP;
                if(jumpCount < 2)
                    jumpCount++;
                else
                    jumpCount = 0;
                isJump = true;
                break;
            case State.DROP:
                playerState = State.DROP;
                break;
            case State.HANG:
                playerState = State.HANG;
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
            
            if (key == KeyCode.LeftArrow || key == KeyCode.RightArrow)
            {
                isWalk = true;
            }

            if (key == KeyCode.Space)
            {
                stateChange(State.JUMP);
            }
            else if(playerState == State.JUMP && rigid.velocity.y < 0)
            {
                stateChange(State.DROP);
            }

            if (key == KeyCode.X)
            {
                stateChange(State.DASH);
            }
            Move(key);
        }
    }

    void Move(KeyCode key)
    {
        if(playerState == State.DASH)
        {
            speed = dashSpeed;
        }
        else if(isRun)
        {
            stateChange(State.RUN);
        }
        else if(isWalk)
        {
            stateChange(State.WALK);
        }

        if (key == KeyCode.Space)
        {
            if (jumpCount < 2)
            {
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
}
