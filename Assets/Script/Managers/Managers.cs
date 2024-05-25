using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성의 보장
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 호출

    private static InputManager _input = new InputManager();
    private static ResourceManager _resource = new ResourceManager();

    public static InputManager Input { get { return _input; } }
    public static ResourceManager Resource { get { return _resource; } }

    void Start()
    {
        if(s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Init();
    }

    
    void Update()
    {
        Input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("Managers"); // Manager가 많아져도 원본을 Find해옴
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }

    public static void Clear()
    {
        Input.Clear();
    }
}
