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
    //������ �ִϸ��̼� ����
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    //�ִϸ��̼� enum
    public enum AnimState
    {
        IDLE,
        ATT_UPPERCUT,
        ATT_HEADING,
        ATT_JUMP,
        CC_BOUNCE,
        CC_GROGGY
    }

    //���� ó�� �Ǵ� �ִϸ��̼�
    private AnimState _AnimState;

    //���� ����Ǵ� �ִϸ��̼�
    private string CurrentAnimation;

    //���� ó��
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
        //���� �ִϸ��̼ǰ� ������ ����
        if (animClip.name.Equals(CurrentAnimation))
            return;

        //�ش� �ִϸ��̼����� ����
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //���� ����ǰ� �ִ� �ִϸ��̼� �� ����
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
