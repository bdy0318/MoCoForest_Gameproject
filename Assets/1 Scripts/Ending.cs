using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject endingCam;
    public Image fadePanel;
    public Text titlePanel;
    public GameObject enter;
    public Transform[] playerPos;
    public Transform[] cameraPos;
    public Transform[] point;
    [TextArea(1, 2)]
    public string[] talk;
    public bool isLerping;
    public bool isStart;
    public bool isFadeIn;
    public float angle;
    public float nowAngle;
    int eventNum;
    Color color;
    Player player;
    Quest quest;
    Animator anim;
    Rigidbody rigid;
    WaitForSeconds waitTime = new WaitForSeconds(4f);

    public void Start()
    {
        player = GameManager.Instance.player;
        quest = GameManager.Instance.quest;
        anim = player.GetComponent<Animator>();
        rigid = player.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        isStart = true;
        color = fadePanel.color;
    }

    private void Update()
    {
        // ���� ��
        if(isStart)
        {
            // ���̵� �ƿ�
            if(!isFadeIn)
                color.a += Time.deltaTime;
            // ���̵� ��
            else
                color.a -= Time.deltaTime;
            fadePanel.color = color;

            // ���̵� �� ���� ��
            if(!isFadeIn && color.a >= 1)
            {
                player.transform.position = playerPos[0].position;
                player.transform.rotation = playerPos[0].rotation;
                endingCam.SetActive(true);
                isFadeIn = true;
            }
            // ���̵� �ƿ� ���� ��
            else if(isFadeIn && color.a <= 0)
            {
                isStart = false;
                isFadeIn = false;
                StartCoroutine(PlayeEnding());
            }
        }
        else if(isLerping)
        {
            // ù ��° �̺�Ʈ
            if (eventNum == 1)
                endingCam.transform.RotateAround(point[0].position, Vector3.up, angle * Time.deltaTime);
            // �� ��° �̺�Ʈ
            else if (eventNum == 2)
            {
                endingCam.transform.RotateAround(point[1].position, Vector3.up, angle * Time.deltaTime);
            }
            // ������ �̺�Ʈ
            else if (eventNum == 3)
            {
                endingCam.transform.position += Vector3.up * 0.8f * Time.deltaTime;
                // Ÿ��Ʋ ǥ��
                if (endingCam.transform.position.y > 10 && titlePanel.color.a < 1)
                {
                    color = titlePanel.color;
                    color.a += Time.deltaTime * 0.3f;
                    titlePanel.color = color;
                }
                // ���� ǥ��
                else if (endingCam.transform.position.y > 20)
                {
                    enter.SetActive(true);
                    if (AudioManager.Instance.flag)
                    {
                        AudioManager.Instance.FadeOutMusic();
                    }
                }
                else if (endingCam.transform.position.y > 30)
                    isLerping = false;
                // ���� ǥ�� �� ���� ��
                if(enter.activeSelf && Input.GetButtonDown("Submit"))
                {
                    rigid.isKinematic = false;
                    player.gameObject.SetActive(false);
                    SceneManager.LoadScene("ObjectDestroy");
                }
            }
            // �� ��°
            else if (eventNum == 4)
            {
                endingCam.transform.position += Vector3.back * 0.3f * Time.deltaTime;
            }
            // �� ��°
            else if (eventNum == 5)
            {
                endingCam.transform.position += Vector3.left * 0.3f * Time.deltaTime;
            }
        } 
    }

    public void FirstTalk()
    {
        quest.npc[0].pressE.SetActive(false);
        quest.npc[0].nameText.text = quest.npc[0].Name;
        quest.npc[0].pressE.SetActive(false);
        quest.npc[0].DialogueText.text = talk[0];
        quest.npc[0].NpcPannel.SetActive(true);
    }

    public void SecondTalk()
    {
        quest.npc[3].pressE.SetActive(false);
        quest.npc[3].nameText.text = quest.npc[3].Name;
        quest.npc[3].pressE.SetActive(false);
        quest.npc[3].DialogueText.text = talk[1];
        quest.npc[3].NpcPannel.SetActive(true);
        eventNum = 2;
        angle = 3;
        player.transform.position = playerPos[1].position;
        player.transform.rotation = playerPos[1].rotation;
        endingCam.transform.position = cameraPos[0].position;
        endingCam.transform.rotation = cameraPos[0].rotation;
    }
    public void ThirdTalk()
    {
        quest.npc[1].pressE.SetActive(false);
        quest.npc[1].nameText.text = quest.npc[1].Name;
        quest.npc[1].pressE.SetActive(false);
        quest.npc[1].DialogueText.text = talk[2];
        quest.npc[1].NpcPannel.SetActive(true);
        eventNum = 3;
        player.transform.position = playerPos[2].position;
        player.transform.rotation = playerPos[2].rotation;
        endingCam.transform.position = cameraPos[1].position;
        endingCam.transform.rotation = cameraPos[1].rotation;
    }

    public void FourthTalk()
    {
        quest.npc[2].pressE.SetActive(false);
        quest.npc[2].nameText.text = quest.npc[2].Name;
        quest.npc[2].pressE.SetActive(false);
        quest.npc[2].DialogueText.text = talk[3];
        quest.npc[2].NpcPannel.SetActive(true);
        eventNum = 4;
        player.transform.position = playerPos[3].position;
        player.transform.rotation = playerPos[3].rotation;
        endingCam.transform.position = cameraPos[2].position;
        endingCam.transform.rotation = cameraPos[2].rotation;
    }

    public void Fifthtalk()
    {
        quest.npc[4].pressE.SetActive(false);
        quest.npc[4].nameText.text = quest.npc[4].Name;
        quest.npc[4].pressE.SetActive(false);
        quest.npc[4].DialogueText.text = talk[4];
        quest.npc[4].NpcPannel.SetActive(true);
        eventNum = 5;
        player.transform.position = playerPos[4].position;
        player.transform.rotation = playerPos[4].rotation;
        endingCam.transform.position = cameraPos[3].position;
        endingCam.transform.rotation = cameraPos[3].rotation;
    }

    IEnumerator PlayeEnding()
    {
        anim.SetTrigger("doHello");
        yield return new WaitForSeconds(1f);
        eventNum = 1;
        angle = -1;
        isLerping = true;
        yield return new WaitForSeconds(2f);
        FirstTalk();
        yield return waitTime;
        isLerping = false;
        SecondTalk();
        
        isLerping = true;
        yield return waitTime;
        isLerping = false;
        FourthTalk();
        
        isLerping = true;
        yield return waitTime;
        isLerping = false;
        Fifthtalk();
        
        isLerping = true;
        yield return waitTime;
        isLerping = false;

        ThirdTalk();
        
        yield return new WaitForSeconds(1f);
        isLerping = true;
        yield return new WaitForSeconds(5f);
        quest.npc[1].NpcPannel.SetActive(false);
    }
}
