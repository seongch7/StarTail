using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class NewController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rigid;
    private SkeletonAnimation mySkeleton;
    private GameObject currentOneWayPlatform;
    [SerializeField]
    private CapsuleCollider2D playerCollider;
    [SerializeField]
    private PolygonCollider2D attackCollider;
    
    //이동 관련 변수
    private float damage = 1f;
    private float speed = 2f;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float dashSpeed = 10f;
    private float jumpForce = 600;
    private float wallJmpForce = 10f;
    private int jumpCount = 0;
    private float isRight = -1;
    private float groundChkDis = 0.35f;
    private float wallChkDis = 0.15f;
    private float slopeDownAngle;
    bool isWalk = false;
    bool isRun = false;
    bool isDash = false;
    bool isJump = false;
    bool isDrop = false;
    bool isHang = false;
    bool isAttack = false;
    bool isWallJmp = false;
    bool isWall = false;
    bool isGround = true;
    bool isDamaged = false;
    bool isOnSlope = false;
    bool canDmg = false;
    bool canMove = true;
    bool canDamaged = true;
    bool down = false;
    bool coolTime = true;

    //레이어 체크 변수
    public Transform wallChk;
    public Transform groundChk;
    
    public LayerMask w_Layer;
    public LayerMask g_Layer;
    public LayerMask s_Layer;

    //공격 콜라이더
    private GameObject attack;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    private Vector2 slopeNormalPerp;
    List<KeyCode> keys = new List<KeyCode>()
    { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow,
        KeyCode.Space, KeyCode.D, KeyCode.LeftShift, KeyCode.A };

    public enum State
    {
        IDLE,
        WALK,
        RUN,
        DASH,
        JUMP,
        DROP,
        HANG,
        ATTACK,
        DAMAGED
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
        attackCollider.enabled = false;
        attack = transform.Find("Attack").gameObject;
        DontDestroyOnLoad(this);
        
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
                if (!isRun || !isWalk)
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
            case State.DAMAGED:
                if (!isDamaged)
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

        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundChkDis, g_Layer);
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallChkDis, w_Layer);

        //경사로 체크
        RaycastHit2D hit = Physics2D.Raycast(groundChk.position, Vector2.down, groundChkDis, s_Layer);
        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != 0)
            {
                isOnSlope = true;
            }
        }
        else
            isOnSlope = false;

        if ((isGround && !isDamaged) || isOnSlope)
        {
            isDrop = false; // 애니메이션 제한
        }

        if (isWall)
        {
            isWallJmp = false;
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            setCurrentState(State.HANG);
        }

        isWalk = false;
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
                rigid.velocity = new Vector2(0, 0);
                playerState = State.IDLE;
                AscnAnimation(AnimClip[(int)state], true, 1f);
                break;
            case State.WALK:
                playerState = State.WALK;
                AscnAnimation(AnimClip[(int)state], true, 1.5f);
                break;
            case State.RUN:
                playerState = State.RUN;
                AscnAnimation(AnimClip[(int)state], true, 1.5f);
                break;
            case State.DASH:
                coolTime = false;
                playerState = State.DASH;
                AscnAnimation(AnimClip[(int)state], true, 2f);
                Invoke("stopCounter", 0.2f);
                Invoke("CoolDown", 1f);
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
                AscnAnimation(AnimClip[(int)state], false, 1.3f);
                StartCoroutine(Attack());
                break;
            case State.DAMAGED:
                playerState = State.DAMAGED;
                AscnAnimation(AnimClip[(int)state], false, 1.7f);
                break;
        }
    }

    void OnKeyboard(KeyCode key)
    {
        
        if (keys.Contains(key))
        {
            if(key == KeyCode.DownArrow)
            {
                down = !down;
            }
            if (key == KeyCode.LeftShift)
            {
                isRun = !isRun;
            }

            if (playerState != State.JUMP)
            {
                if (key == KeyCode.Space && down)
                {
                    if (currentOneWayPlatform != null)
                    {
                        StartCoroutine(DisableCollision()); //하단 점프
                        setCurrentState(State.DROP);
                        return;
                    }
                }
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
                        if (key == KeyCode.A)
                        {
                            setCurrentState(State.ATTACK);
                            return;
                        }
                        else if (playerState != State.DASH)
                        {
                            if (key != KeyCode.D)
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
        if (isWallJmp || isDash || isAttack || !canMove)
            return;

        if (isJump)
        {
            setCurrentState(State.JUMP);

            if (key == KeyCode.D && coolTime)
            {
                isDash = true;
                rigid.velocity = new Vector2(isRight * dashSpeed, 0);
                setCurrentState(State.DASH);
            }
            else if (isRun)
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }
        else if (isDrop)
        {
            setCurrentState(State.DROP);

            if (key == KeyCode.D && coolTime)
            {
                isDash = true;
                rigid.velocity = new Vector2(isRight * dashSpeed, 0);
                setCurrentState(State.DASH);
            }
            else if (isRun)
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }
        else
        {
            if (key == KeyCode.D && coolTime)
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
            else
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
                rigid.velocity = new Vector2(-isRight * wallJmpForce, 0.9f * wallJmpForce + 3f);
                Flip();
            }
            else if (jumpCount < 2)
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
            if (isGround && isOnSlope)
            {
                rigid.velocity = new Vector2(speed * slopeNormalPerp.x, rigid.velocity.y * slopeNormalPerp.y * -1);
            }
            else
                rigid.velocity = new Vector2(speed * -1, rigid.velocity.y);
        }
        else if (key == KeyCode.RightArrow)
        {
            if (isRight != 1)
                Flip();
            if(isGround && isOnSlope)
            {
                rigid.velocity = new Vector2(speed * slopeNormalPerp.x * -1, rigid.velocity.y * slopeNormalPerp.y);
            }
            else
                rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }
    }
    
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.38f); // 공격 선딜

        attack.SetActive(true);

        yield return new WaitForSeconds(0.15f); // 공격 판정시간

        attack.SetActive(false);

        yield return new WaitForSeconds(0.17f);

        isAttack = false;
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.contacts[0].normal.y > 0.7f)
        {
            isDrop = false;
            jumpCount = 0;
        }

        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = col.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        //발판 아랫점프
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

    }
    public void OnDamaged(Vector2 targetPos)
    {
        if (!canDamaged)
            return;

        mySkeleton.skeleton.SetColor(Color.red);
        isDamaged = true;
        gameObject.layer = 8;
        canMove = false;

        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        if (dirc == isRight)
            Flip();

        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(dirc, 1f) * 6, ForceMode2D.Impulse);
        setCurrentState(State.DAMAGED);

        Invoke("Regain", 0.1f);
        Invoke("CanMove", 0.5f); // 경직시간
        Invoke("OffDamaged", 1f); // 무적시간
    }

    private void Regain()
    {
        mySkeleton.skeleton.SetColor(Color.white);
    }

    private void CanMove()
    {
        isDamaged = false;
        canMove = true;
    }

    private void CanDmg()
    {
        attackCollider.enabled = true;
        canDmg = true;
    }

    private void OffDamaged()
    {
        gameObject.layer = 7;
    }

    private void FreezeX()
    {
        isWallJmp = false;
        isHang = false;
    }

    private void Flip()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    void stopCounter()
    {
        isDash = false;
        setCurrentState(State.IDLE);
    }

    void CoolDown()
    {
        coolTime = true;
    }
}