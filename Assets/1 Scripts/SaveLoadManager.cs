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
        public float posX;
        public float posY;
        public float posZ;

        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;
    }

    public Data data;
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

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(Application.persistentDataPath + "의 위치에 저장했습니다.");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "SaveData.dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as Data;

        }
    }

    public bool IsSave()
    {
        string path = Application.persistentDataPath + "SaveData.dat";
        if (File.Exists(path))
            return true;
        else
            return false;
    }
}
