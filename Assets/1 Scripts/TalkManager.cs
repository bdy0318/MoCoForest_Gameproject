using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    #region Singleton
    public static TalkManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        talkData = new Dictionary<int, string[]>();
        GenrateData();
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton

    public Dictionary<int, string[]> talkData;

    void GenrateData()
    {
        talkData.Add(1000, new string[] { "�ȳ��! ���������� �� �� ȯ������.",
                                          "�ΰ� ģ���� �츮 ������ ���ٴ� �ű��ϴپ�" });
        talkData.Add(2000, new string[] { "��, �ʳ�!", "�� �Գ�." });
        talkData.Add(3000, new string[] { ".....", "�������� ���� ������ ���̴� �� ���� �ʾ�?" });
        talkData.Add(4000, new string[] { "��!! �ȳ� �ݰ���!!", "�ʴ� �� �ű��ϰ� �����!!!" });
        talkData.Add(5000, new string[] { "�װ� ������������ ��� �ʹٰ� �� �ΰ�����?",
                                          "���� ������ ���� �ڸ� �����Ѵٰ�!"});
        talkData.Add(6000, new string[] { "������ ������ �ٰ����� ��. �� ��ĥ����?",
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

