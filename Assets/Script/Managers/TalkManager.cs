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
        talkData.Add(1001, new string[] { "qweqweqwe", "123456789" });
    }

    public string GetTalk(int id, int talkIndex)//���ڿ��� �迭�� �����߱� ������ ��� ° ������ ������ �������� ���� index
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
