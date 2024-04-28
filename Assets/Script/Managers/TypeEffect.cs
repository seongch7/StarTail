using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public float CharPerSeconds; // 1초당 나올 글자 수
    //public GameObject EndCursor;
    string targetMsg; // 원본 대화 내용 저장할 변수
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
        
        msgText.text += targetMsg[index]; // 문자열도 배열처럼 char값에 inedx로 접근 가능
        index++;

        Invoke("Effecting", 1 / CharPerSeconds);
    }

    void EffectEnd()
    {
    }
}
