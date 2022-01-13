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
        talkData.Add(1000, new string[] { "안녀엉! 동물마을에 온 걸 환여엉해.",
                                          "인간 친구가 우리 마을에 오다니 신기하다아" });
        talkData.Add(2000, new string[] { "앗, 너냐!", "왜 왔냐." });
        talkData.Add(3000, new string[] { ".....", "…마을이 뭔가 허전해 보이는 것 같지 않아?" });
        talkData.Add(4000, new string[] { "와!! 안녕 반가워!!", "너는 참 신기하게 생겼다!!!" });
        talkData.Add(5000, new string[] { "네가 동물마을에서 살고 싶다고 한 인간이지?",
                                          "나는 나보다 강한 자만 인정한다고!"});
        talkData.Add(6000, new string[] { "야, 나한테 가까이 다가오지 마. 날 해칠꺼지?",
                                          "나는 인간이 우리 마을에 오는게 마음에 안들어."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

   

}

