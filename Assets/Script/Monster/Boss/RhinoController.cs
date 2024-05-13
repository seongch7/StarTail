using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RhinoController : MonoBehaviour
{
    //애니메이션 enum
    public enum AnimState
    {
        IDLE,
        ATT_UPPERCUT,
        ATT_JUMP,
        ATT_HEADING2,
        ATT_HEADING,
        CC_BOUNCE,
        CC_GROGGY
    }

    //스파인 애니메이션 관련
    private SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    //현재 처리 되는 애니메이션
    private AnimState _AnimState;

    //현재 재생되는 애니메이션
    private string CurrentAnimation;

    //무브 처리
    private Rigidbody2D rig;

    //공격 콜라이더
    private GameObject jumpAttack;
    private GameObject upperAttack;
    private GameObject dashAttack;

    [SerializeField]
    public GameObject player;

    MeshRenderer meshRenderer;

    private float distance;
    private float duration;
    private float dir;

    public bool _isWall = false;

    private void Awake()
    {
        //초기 선언
        rig = GetComponent<Rigidbody2D>();
        meshRenderer = GetComponent<MeshRenderer>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        
        //공격 관련
        jumpAttack = transform.Find("JumpAttack").gameObject;
        upperAttack = transform.Find("UpperAttack").gameObject;
        dashAttack = transform.Find("DashAttack").gameObject;


        //IDLE 애니메이션 실행
        _AnimState = AnimState.IDLE;
        CurrentAnimation = AnimClip[0].name;
        duration = 2.667f;
        dir = -1;

        Flip();
        SetCurrentAnimation(_AnimState);

        StartCoroutine(AnimStart(duration));
    }

    private void Update()
    {
        if(transform.position.x >= 9 || transform.position.x<= -9)
        {
            _isWall = true;
            if(CurrentAnimation == "ATT_HEADING2")
            {
                StartCoroutine(DashAttackEnd());
            }
        }
        else
        {
            _isWall = false;

            if (CurrentAnimation == "ATT_HEADING2")
            {
                dashAttack.SetActive(true);
                rig.velocity = new Vector2(3 * dir, 0);
            }
        }

    }

    private IEnumerator AnimStart(float duration)
    {

        yield return new WaitForSeconds(duration);
        
        distance = (player.transform.position.x - gameObject.transform.position.x > 0) ? player.transform.position.x - gameObject.transform.position.x : gameObject.transform.position.x - player.transform.position.x;
        
        if(CurrentAnimation == "CC_BOUNCE")
        {
            _AnimState = AnimState.IDLE;

        }
        else if (distance < 3f)
        {
            _AnimState = AnimState.ATT_UPPERCUT;
        }
        else if (distance < 8f)
        {
            _AnimState = AnimState.ATT_JUMP;
        }
        else
        {
            _AnimState = AnimState.ATT_HEADING2;
        }

        SetCurrentAnimation(_AnimState);
        Flip();

        //공격 코루틴
        switch (CurrentAnimation)
        {
            case "ATT_JUMP":
                StartCoroutine(JumpAttack());
                break;
            case "ATT_UPPERCUT":
                StartCoroutine(UpperAttack());
                break;
            case "ATT_HEADING2":
                DashAttackStart();
                yield break;

        }

        duration = skeletonAnimation.skeleton.Data.FindAnimation(skeletonAnimation.AnimationName).Duration;
        StartCoroutine(AnimStart(duration));

        yield break;
    }
    private void _AsncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        //현재 애니메이션과 같으면 리턴
        if (animClip.name.Equals(CurrentAnimation))
            return;

        //해당 애니메이션으로 변경
        skeletonAnimation.state.SetAnimation(0, animClip, loop);
       

        //현재 재생되고 있는 애니메이션 값 변경
        CurrentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        switch(_state)
        {
            case AnimState.IDLE:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
            case AnimState.ATT_JUMP:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
            case AnimState.ATT_UPPERCUT:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
            case AnimState.ATT_HEADING2:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
            case AnimState.ATT_HEADING:
                _AsncAnimation(AnimClip[(int)_state], false, 1f);
                break;
            case AnimState.CC_BOUNCE:
                _AsncAnimation(AnimClip[(int)_state], false, 1f);
                break;
        }
    }

    private void Flip()
    {
        float dir = (player.transform.position.x - gameObject.transform.position.x > 0) ? 1 : -1;

        if (dir==1)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

    }

    private IEnumerator JumpAttack()
    {
        //점프 시작
        yield return new WaitForSeconds(1.43f);


        dir = (player.transform.position.x - gameObject.transform.position.x > 0) ? 1 : -1;

        rig.velocity = new Vector2(6 * dir, 0);

        //점프 착지
        yield return new WaitForSeconds(0.77f);

        rig.velocity = new Vector2(0, 0);
        jumpAttack.SetActive(true);

        //공격 판정 0.2초
        yield return new WaitForSeconds(0.2f);

        jumpAttack.SetActive(false);

        yield break;
    }

    private IEnumerator UpperAttack()
    {
        //공격 시작
        yield return new WaitForSeconds(0.85f);
        upperAttack.SetActive(true);

        //공격 판정 0.1초
        yield return new WaitForSeconds(0.1f);

        upperAttack.SetActive(false);

        yield break;
    }

    private void DashAttackStart()
    {
        dir = (player.transform.position.x - gameObject.transform.position.x > 0) ? 1 : -1;

        dashAttack.SetActive(true);
        rig.velocity = new Vector2(3 * dir, 0);

    }
    private IEnumerator DashAttackEnd()
    {
        rig.velocity = new Vector2(0, 0);

        dashAttack.SetActive(false);

        _AnimState = AnimState.CC_BOUNCE;
        SetCurrentAnimation(_AnimState);

        StartCoroutine(Bounce());

        yield break;
    }
    private IEnumerator Bounce()
    {
        rig.velocity = new Vector2(6 * -1 * dir, 5);

        skeletonAnimation.timeScale = 0;

        yield return new WaitForSeconds(0.3f);
        rig.velocity = new Vector2(6 * -1 * dir, 0);

        yield return new WaitForSeconds(0.3f);
        skeletonAnimation.timeScale = 1;

        yield return new WaitForSeconds(1.30f);
        rig.velocity = new Vector2(0,0);
        skeletonAnimation.timeScale = 0;

        yield return new WaitForSeconds(3f);

        skeletonAnimation.timeScale = 1;
        StartCoroutine(AnimStart(0.1f));

        yield break;
    }
    public void OnDamaged()
    {
        rig.velocity = Vector2.zero;
        meshRenderer.materials[0].color = new Color(1, 1, 1, 0.4f);
        Invoke("Regain", 0.1f);
    }

    private void Regain()
    {
        meshRenderer.materials[0].color = new Color(1, 1, 1, 1f);
    }
}
