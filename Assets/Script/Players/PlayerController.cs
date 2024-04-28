using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    public GameManager gameManager;
    BoxCollider2D BoxCollider;
    MeshRenderer rend;
    Animator anim;
    Vector2 dirVec;
    GameObject scanObject;

    //플레이어 이동 관련 변수
    float xInput;
    private int jumpCount = 0;
    private float dashTime;
    public float isRight = 1;
    private bool isDash = false;
    [SerializeField]
    private float defaultTime;
    [SerializeField]
    private float defaultSpeed; //초기화용 속도
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float dashSpeed; // 대쉬 속도
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float ownMaxSpeed;
    [SerializeField]
    private float runMaxSpeed;

    
    //벽체크 관련 변수
    [SerializeField]
    private float wallChkDis;
    [SerializeField]
    private float wallJumpPower;
    [SerializeField]
    private bool isWallJump;
    [SerializeField]
    private bool isWall;
    public Transform wallChk;
    public LayerMask w_Layer;
    [SerializeField]
    private bool isGround = true;
    public Transform groundChk;
    public LayerMask g_Layer;
    [SerializeField]
    private float groundChkDis;

    //오브젝트 체크 관련 변수
    public Transform ObjChk;
    public LayerMask o_Layer;
    public float objChkDis;

    //레이어 관련 변수
    private bool isEntry = false;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<MeshRenderer>();
        anim = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isEntry == true)
        {
            gameManager.Invoke("OpenWay", 1);
            gameManager.Invoke("ActiveExit", 3);
            gameManager.ChangeLayer("Foreground");
        }

        xInput = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallChkDis, w_Layer);
        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundChkDis, g_Layer);

        if (!isWallJump)
            if ((xInput > 0 && isRight < 0) || (xInput < 0 && isRight > 0))
            {
                Flip();
            }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isDash = true;
        }

        if (dashTime <= 0)
        {
            maxSpeed = defaultSpeed;
            if (isDash)
                dashTime = defaultTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
            maxSpeed = dashSpeed;
        }
        isDash = false;

        Animation();

        if (Input.GetKeyDown(KeyCode.E) && scanObject != null)
        {
            gameManager.Action(scanObject);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.normalized.x * 0.5f, playerRigidbody.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (isWall)
        {
            isWallJump = false;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);

            if (Input.GetAxis("Jump") != 0)
            {
                isWallJump = true;
                Invoke("FreezeX", 0.3f);
                playerRigidbody.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                Flip();
            }
        }

        if (!isWallJump)
            if (dashTime > 0)
                playerRigidbody.velocity = new Vector2(xInput * dashSpeed, playerRigidbody.velocity.y);
            else
                playerRigidbody.AddForce(Vector2.right * xInput, ForceMode2D.Impulse);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            maxSpeed = runMaxSpeed;

            if(dashTime > 0)
                maxSpeed = dashSpeed;
            if (playerRigidbody.velocity.x > maxSpeed)
                playerRigidbody.velocity = new Vector2(runMaxSpeed, playerRigidbody.velocity.y);
            else if (playerRigidbody.velocity.x < maxSpeed * (-1))
                playerRigidbody.velocity = new Vector2(runMaxSpeed * (-1), playerRigidbody.velocity.y);
        }
        else
        {
            maxSpeed = ownMaxSpeed;

            if (dashTime > 0)
                maxSpeed = dashSpeed;
            if (playerRigidbody.velocity.x > maxSpeed)
                playerRigidbody.velocity = new Vector2(maxSpeed, playerRigidbody.velocity.y);
            else if (playerRigidbody.velocity.x < maxSpeed * (-1))
                playerRigidbody.velocity = new Vector2(maxSpeed * (-1), playerRigidbody.velocity.y);
        }

        RaycastHit2D rayHit = Physics2D.Raycast(ObjChk.position, Vector2.right * isRight, 0.7f, LayerMask.GetMask("Object"));

        if(rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            anim.SetBool("isJump", false);
            jumpCount = 0;
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if(playerRigidbody.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                playerRigidbody.velocity = Vector2.zero;
                OnDamaged(collision.transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            //점수
            gameManager.stagePoint += 100;
            //아이템 비활성화
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Finish")
        {
            //다음 스테이지
            gameManager.NextStage();
        }

        if (collision.gameObject.name == "Entry")
            isEntry = true;

        if (collision.gameObject.name == "Exit")
        {
            gameManager.CloseWay();
            gameManager.ChangeLayer("BackGround");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Entry")
            isEntry = false;
    }

    private void OnAttack(Transform enemy)
    {
        gameManager.stagePoint += 100;

        playerRigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        MonsterMove monsterMove = enemy.GetComponent<MonsterMove>();
        monsterMove.OnDamaged();// 가시도 enemy 태그이기 때문에 위에서 밟으면 오류 발생, 나중에 만들 땐 가시같은 함정은 태그를 따로 설정할 필요 있음
    }
    
    public void OnDamaged(Vector2 targetPos)
    {
        gameManager.HealthDown();

        gameObject.layer = 8;

        float dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        playerRigidbody.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        Invoke("OffDamaged", 2);
    }

    
    private void OffDamaged()
    {
        gameObject.layer = 7;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallChkDis);
        Gizmos.DrawRay(groundChk.position, Vector2.down * groundChkDis);
    }

    private void FreezeX()
    {
        isWallJump = false;
    }

    private void Flip()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    public void OnDie()
    {
        BoxCollider.enabled = false;
        playerRigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        playerRigidbody.velocity = Vector2.zero;
    }
    private void Animation()
    {
        if (isWall) // 벽점프
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isHang", true);

            if(isWallJump)
            {
                anim.SetBool("isHang", false);
                anim.SetBool("isJump", true);
            }
            return;
        }
        else if (!isWall && playerRigidbody.velocity.y < 0)
        {
            anim.SetBool("isHang", false);
            anim.SetBool("isDrop", true);
            return;
        }

        if (!isGround) // 점프
        {
            if (playerRigidbody.velocity.y > 0)
                anim.SetBool("isJump", true);
            else if (playerRigidbody.velocity.y < 0)
                anim.SetBool("isDrop", true);
        }
        else
        {
            anim.SetBool("isJump", false);
            anim.SetBool("isDrop", false);
        }

        if (UnityEngine.Input.GetKey(KeyCode.LeftShift)) //달리기
        {
            if (playerRigidbody.velocity.normalized.x != 0)
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", true);
            }
            else
                anim.SetBool("isRun", false);
        }

        else // 걷기
        {
            if (playerRigidbody.velocity.normalized.x != 0)
            {
                anim.SetBool("isRun", false);
                anim.SetBool("isWalk", true);
            }
            else
                anim.SetBool("isWalk", false);
        }

        if (dashTime > 0) // 대쉬
            anim.SetBool("isDash", true);
        else
            anim.SetBool("isDash", false);
    }
}
