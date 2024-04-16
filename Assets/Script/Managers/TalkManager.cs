using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "Hello Rota!" , "This is Unity World!qqqqqq\nasdasdsdddddd\nqwdqwdasdasd\nwrvcbcvhhghdfhd"});
    }

    public string GetTalk(int id, int talkIndex)//문자열을 배열로 지정했기 때문에 몇번 째 문장을 가져올 것인지에 대한 index
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
