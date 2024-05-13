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
        ATT_HEADING,
        ATT_JUMP,
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
    [SerializeField]
    public GameObject player;

    public float distance;
    public float duration;

    private void Awake()
    {
        //초기 선언
        rig = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();


        //IDLE 애니메이션 실행
        _AnimState = AnimState.IDLE;
        CurrentAnimation = AnimClip[0].name;
        duration = 2.667f;

        Flip();
        SetCurrentAnimation(_AnimState);

        StartCoroutine(AnimStart(duration));
    }

    private IEnumerator AnimStart(float duration) 
    {
        yield return new WaitForSeconds(duration - 0.1f);

        distance = (player.transform.position.x - gameObject.transform.position.x > 0) ? player.transform.position.x - gameObject.transform.position.x : gameObject.transform.position.x - player.transform.position.x;

        if (distance < 4f)
        {
            _AnimState = AnimState.ATT_UPPERCUT;
        }
        else if (distance < 10f)
        {
            _AnimState = AnimState.ATT_JUMP;
        }
        else
        {
            _AnimState = AnimState.IDLE;
        }

        Flip();
        SetCurrentAnimation(_AnimState);

        duration = skeletonAnimation.skeleton.Data.FindAnimation(skeletonAnimation.AnimationName).Duration;
        StartCoroutine(AnimStart(duration));

    }
    private void _AsncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        //현재 애니메이션과 같으면 리턴
        if (animClip.name.Equals(CurrentAnimation))
            return;

        if (loop)
        {
            //해당 애니메이션으로 변경
            skeletonAnimation.state.SetAnimation(0, animClip, loop);
        }
        else
        {
            //해당 애니메이션으로 변경
            skeletonAnimation.state.SetAnimation(0, animClip, loop);

            skeletonAnimation.state.AddAnimation(0, AnimClip[0], true,0);

        }

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
            case AnimState.ATT_HEADING:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
        }
    }

    private void Flip()
    {
        bool dir = (player.transform.position.x - gameObject.transform.position.x > 0) ? true : false;

        if (dir)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
