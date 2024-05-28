using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeController : MonoBehaviour
{
    public bool isFadein;
    public GameObject panel;
    private Action onCompleteCallback;

    void Start()
    {
        if (!panel)
        {
            throw new MissingComponentException();
        }

        if (isFadein)
        {
            panel.SetActive(true);
            StartCoroutine(CoFadeIn());
        }
        else
        {
            panel.SetActive(false);
        }
    }
    public void FadeOut()
    {
        panel.SetActive(true);
        StartCoroutine(CoFadeOut());
    }

    IEnumerator CoFadeIn()
    {
        float elapsedTime = 0f;
        float fadedTime = 1f;

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.SetActive(false);
        onCompleteCallback?.Invoke();
        yield break;
    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f;
        float fadedTime = 1f;

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        onCompleteCallback?.Invoke();
        yield break;
    }

    public void RegisterCallback(Action callback)
    {
        onCompleteCallback = callback;
    }
}
