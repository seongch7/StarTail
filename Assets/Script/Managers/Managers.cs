using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // ���ϼ��� ����
    public static Managers Instance { get { Init(); return s_instance; } } // ������ �Ŵ����� ȣ��

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
            GameObject go = GameObject.Find("Managers"); // Manager�� �������� ������ Find�ؿ�
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
