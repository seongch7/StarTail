using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    public enum State
    {
        STOP,
        LEFT,
        RIGHT
    }

    public State objectState;
    private string CurrentAnimation;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;
    private string currentState;
    private float duration;

    private void Awake()
    {
        objectState = State.STOP;
        CurrentAnimation = AnimClip[0].name;
        duration = 0f;

        setCurrentState(objectState);

        StartCoroutine(AnimStart(duration));
    }

    private IEnumerator AnimStart(float duration)
    {
        yield return new WaitForSeconds(duration);

        Invoke("Reset", duration);
        setCurrentState(objectState);

        duration = skeletonAnimation.skeleton.Data.FindAnimation(skeletonAnimation.AnimationName).Duration;
        StartCoroutine (AnimStart(duration));

        yield break;
    }

    private void Reset()
    {
        objectState = State.STOP;
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

    void setCurrentState(State state)
    {
        switch (state)
        {
            case State.STOP:
                objectState = State.STOP;
                AscnAnimation(AnimClip[(int)state], true, 1f);
                break;
            case State.LEFT:
                objectState = State.LEFT;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
            case State.RIGHT:
                objectState = State.RIGHT;
                AscnAnimation(AnimClip[(int)state], false, 1f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            float dirc = transform.position.x - col.gameObject.transform.position.x > 0 ? 1 : -1;
            if (dirc == 1)
                objectState = State.RIGHT;
            else
                objectState = State.LEFT;
        }
    }
}
