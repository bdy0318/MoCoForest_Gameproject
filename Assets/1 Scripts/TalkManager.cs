using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenrateData();
    }

    void GenrateData()
    {
        talkData.Add(1000, new string[] { "�ȳ��! ���������� �� �� ȯ������.",
                                          "�ΰ� ģ���� �츮 ������ ���ٴ� �ű��ϴپ�" });
        talkData.Add(2000, new string[] { "��, �ʳ�!", "�� �Գ�." });
        talkData.Add(3000, new string[] { ".....", "�������� ���� ������ ���̴� �� ���� �ʾ�?" });
        talkData.Add(4000, new string[] { "��!! �ȳ� �ݰ���!!", "�ʴ� �� �ű��ϰ� �����!!!" });
        talkData.Add(5000, new string[] { "�װ� ������������ ��� �ʹٰ� �� �ΰ�����?",
                                          "���� ������ ���� �ڸ� �����Ѵٰ�!"});
        talkData.Add(6000, new string[] { "��, ������ ������ �ٰ����� ��. �� ��ĥ����?",
                                          "���� �ΰ��� �츮 ������ ���°� ������ �ȵ��."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

   

}

