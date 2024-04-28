using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public float CharPerSeconds; // 1�ʴ� ���� ���� ��
    //public GameObject EndCursor;
    string targetMsg; // ���� ��ȭ ���� ������ ����
    [SerializeField]
    private Text msgText;
    int index;

    private void Awake()
    {
        msgText = GetComponent<Text>();
    }
    public void SetMsg(string msg)
    {
        targetMsg = msg;
        EffectStart();
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
       // EndCursor.SetActive(false);

        Invoke("Effecting", 1 / CharPerSeconds);
    }

    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        
        msgText.text += targetMsg[index]; // ���ڿ��� �迭ó�� char���� inedx�� ���� ����
        index++;

        Invoke("Effecting", 1 / CharPerSeconds);
    }

    void EffectEnd()
    {
    }
}
