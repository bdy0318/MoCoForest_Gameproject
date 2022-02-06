using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    #region Singleton
    public static SaveLoadManager instance;
    // 게임 매니저 인스턴스 접근
    public static SaveLoadManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton

    [System.Serializable]
    public class Data
    {
        // 위치
        public float posX;
        public float posY;
        public float posZ;

        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;

        // 보유 아이템
        public int coin;
        public int stone;
        public int[] hasItem;
        public bool[] hasWeapons;

        // 대화 저장
        public string[] talk1;
        public string[] talk2;
        public string[] talk3;
        public string[] talk4;
        public string[] talk5;
        public string[] talk6;

        // 퀘스트
        public int nowQust;
        public bool isComplete;
        public bool isGameWin;
        public int thirdQuest;
        public bool[] sixthQuest;

        // 데코레이션
        public bool[] nearDeco;
        public int[] itemIndex;
    }

    public Deco[] deco;
    public bool isLoad;
    public Data data;
    
    // 세이브
    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        data.posX = GameManager.Instance.player.transform.position.x;
        data.posY = GameManager.Instance.player.transform.position.y;
        data.posZ = GameManager.Instance.player.transform.position.z;

        data.rotX = GameManager.Instance.player.transform.rotation.x;
        data.rotY = GameManager.Instance.player.transform.rotation.y;
        data.rotZ = GameManager.Instance.player.transform.rotation.z;
        data.rotW = GameManager.Instance.player.transform.rotation.w;

        data.coin = GameManager.Instance.player.coin;
        data.stone = GameManager.Instance.player.stone;
        data.hasItem = GameManager.Instance.player.hasItem;
        data.hasWeapons = GameManager.Instance.player.hasWeapons;

        data.talk1 = GameManager.Instance.talkManager.talkData[1000];
        data.talk2 = GameManager.Instance.talkManager.talkData[2000];
        data.talk3 = GameManager.Instance.talkManager.talkData[3000];
        data.talk4 = GameManager.Instance.talkManager.talkData[4000];
        data.talk5 = GameManager.Instance.talkManager.talkData[5000];
        data.talk6 = GameManager.Instance.talkManager.talkData[6000];

        data.nowQust = GameManager.Instance.quest.nowQuest;
        data.isComplete = GameManager.Instance.quest.isComplete;
        data.isGameWin = GameManager.Instance.quest.isGameWin;
        data.thirdQuest = GameManager.Instance.quest.thirdQuest;
        data.sixthQuest = GameManager.Instance.quest.sixthQuest;

        for(int i=0; i<deco.Length; i++)
        {
            if (deco[i].isComplete)
            {
                data.nearDeco[i] = true;
                data.itemIndex[i] = deco[i].itemIndex;
                continue;
            }
            data.itemIndex[i] = 0;
        }

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(Application.persistentDataPath + "의 위치에 저장했습니다.");
    }

    // 로드
    public void Load()
    {
        string path = Application.persistentDataPath + "/SaveData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as Data;

            Vector3 playerPos = new Vector3(data.posX, data.posY, data.posZ);
            Quaternion playerRot = new Quaternion(data.rotX, data.rotY, data.rotZ, data.rotW);
            GameManager.Instance.player.transform.position = playerPos;
            GameManager.Instance.player.transform.rotation = playerRot;

            GameManager.instance.player.coin = data.coin;
            GameManager.instance.player.stone = data.stone;
            GameManager.instance.player.hasItem = data.hasItem;
            GameManager.instance.player.hasWeapons = data.hasWeapons;
            GameManager.Instance.player.selectItem = null;
            if (GameManager.Instance.player.equipWeapon != null)
            {
                GameManager.Instance.player.equipWeapon.gameObject.SetActive(false);
                GameManager.Instance.player.equipWeaponIndex = -1;
                GameManager.Instance.player.equipWeapon = null;
            }

            GameManager.Instance.talkManager.talkData[1000] = data.talk1;
            GameManager.Instance.talkManager.talkData[2000] = data.talk2;
            GameManager.Instance.talkManager.talkData[3000] = data.talk3;
            GameManager.Instance.talkManager.talkData[4000] = data.talk4;
            GameManager.Instance.talkManager.talkData[5000] = data.talk5;
            GameManager.Instance.talkManager.talkData[6000] = data.talk6;

            GameManager.Instance.quest.nowQuest = data.nowQust;
            GameManager.Instance.quest.isComplete = data.isComplete;
            GameManager.Instance.quest.isGameWin = data.isGameWin;
            GameManager.Instance.quest.thirdQuest = data.thirdQuest;
            GameManager.Instance.quest.sixthQuest = data.sixthQuest;

            if(GameManager.Instance.quest.nowQuest >= 3)
            {
                GameManager.Instance.quest.Decoration.SetActive(true);
                for (int i = 0; i < GameManager.Instance.decoObj.Count; i++)
                    Destroy(GameManager.Instance.decoObj[i]);

                for(int i=0; i < data.nearDeco.Length; i++)
                {
                    deco[i].itemIndex = data.itemIndex[i];
                    if (data.nearDeco[i])
                    {
                        deco[i].Decorate();
                        deco[i].gameObject.SetActive(false);
                    }
                    else if(deco[i].nearDecoZone)
                        deco[i].gameObject.SetActive(true);
                }
            }
            else
            {
                GameManager.Instance.quest.Decoration.SetActive(false);
                for (int i = 0; i < data.nearDeco.Length; i++)
                {
                    deco[i].itemIndex = 0;
                    deco[i].isComplete = false;
                    deco[i].gameObject.SetActive(true);
                }
            }
            stream.Close();

            isLoad = true;
        }
    }

    // 로드 가능한지 판단
    public bool IsSave()
    {
        string path = Application.persistentDataPath + "/SaveData.dat";
        if (File.Exists(path))
            return true;
        else
            return false;
    }
}
