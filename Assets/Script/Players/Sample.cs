using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RhinoController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    //스파인 애니메이션 관련
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

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

    //현재 처리 되는 애니메이션
    private AnimState _AnimState;

    //현재 재생되는 애니메이션
    private string CurrentAnimation;

    //무브 처리
    private Rigidbody2D rig;
    [SerializeField]
    public GameObject player;

    float distance;

    private void Awake()
    {

        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

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

        SetCurrentAnimation(_AnimState);

    }
    private void _AsncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        //현재 애니메이션과 같으면 리턴
        if (animClip.name.Equals(CurrentAnimation))
            return;

        //해당 애니메이션으로 변경
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값 변경
        CurrentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        switch (_state)
        {
            case AnimState.IDLE:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
            case AnimState.ATT_JUMP:
                _AsncAnimation(AnimClip[(int)_state], true, 1f);
                break;
        }
    }
    public void Jump()
    {

    }
}
