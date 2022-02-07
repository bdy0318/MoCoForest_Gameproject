using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Deco : MonoBehaviour
{
    //아이템 건설
    public GameObject Decoration; //퀘스트 시작할때 꾸미기 기능활성화
    public GameObject[] decoObj; //꾸미기 아이템 종류
    public Transform decoPos; //꾸미기 아이템 설치 장소
    public GameObject decoZone; //설치 장소 표시


    public GameObject showKey; //아이템 선택 tab키, 설치 e키
    public Text showKeyText;
    public int itemIndex;
    public int num;

    public bool nearDecoZone;
    public bool isComplete;

    public Player player;
    public Inventory inventory;
    public Quest quest;

    private void Start()
    {
        player = GameManager.Instance.player;
        quest = GameManager.instance.quest;
    }

    void Update()
    {
        if (player.selectItem != null && nearDecoZone)
            showKeyText.text = "E키를 눌러 선택한 아이템으로 마을을 꾸미기";

        if (Input.GetKeyDown(KeyCode.E) && nearDecoZone) //꾸미기 버튼
        {
            SelectDeco();
            showKey.SetActive(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nearDecoZone = true;
            showKeyText.text = "Tab키로 인벤토리를 열어 아이템 선택하기";
            showKey.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nearDecoZone = false;
            showKey.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player.isInventory)
            {
                showKey.SetActive(false);
            }
            else
                showKey.SetActive(true);

        }
    }

    private void SelectDeco()
    {   
        if(player.selectItem != null) //아이템이 선택되면
        {
            // 선택 아이템 인덱스
            int index = player.selectItem.GetComponent<Item>().value; 
            itemIndex = index;

            if (itemIndex >= 0) //돌 제외
            {
                player.hasItem[itemIndex] -= 1;
                player.selectItem = null;

                Decorate();

                //데코존 파괴, 퀘스트 진행도
                quest.thirdQuest += 1;
                decoZone.SetActive(false);
                
            }
        }
    }
    public void Decorate()
    {
        //위치, 크기 변경
        GameObject newObject = Instantiate(decoObj[itemIndex],decoPos.position, decoPos.rotation);
        newObject.transform.localScale = new Vector3(2, 2, 2);
        if(newObject.GetComponent<Item>().type == Item.Type.Pot)
        {
            newObject.transform.GetChild(0).localScale = new Vector3(2, 2, 2);
        }
        newObject.AddComponent<DontDestroyOnLoad>();
        GameManager.Instance.decoObj.Add(newObject);
        GameManager.Instance.decoList.Add(num);
        isComplete = true;
    }




}
