using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour //여기서 직접 관리하지 않음
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
            Managers.Resource.Instantiate("EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
