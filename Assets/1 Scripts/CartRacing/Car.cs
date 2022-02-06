using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    public Transform[] target;
    public Text numberText;
    public GameObject textPanel;
    public GameObject guidePanel; // ���� ���̵� UI
    public Text endText;
    public Image endPanel;
    public float carSpeed; // �ִ� �ӵ�
    float nowSpeed; // ���� �ӵ�
    public float collisionVal;
    bool isAfterCollision;
    bool isCarCollision;
    public bool isSpeedUp;
    public bool isRaceFinish;
    public bool isRaceStart;
    int index;
    int num;
    Color color;
    Vector3 carCollsionDir;
    Vector3 nowPos;
    Vector3 prePos;
    Quaternion preForwardAngle;
    Rigidbody rigid;
    NavMeshAgent navMeshAgent;

    WaitForSeconds seconds = new WaitForSeconds(1f);

    private void Start()
    {
        color = endPanel.color;
        color.a = 0.0f;
        endPanel.color = color;
        num = 5;
        nowSpeed = 0;
        collisionVal = 1f;
        index = 0;
        rigid = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = nowSpeed * collisionVal;
        navMeshAgent.SetDestination(target[index].position);
        navMeshAgent.updateRotation = false;
        navMeshAgent.isStopped = true;
        prePos = transform.position;
        nowPos = transform.position;
        StartCoroutine(TimeSecond()); // Ÿ�̸� ����
    }

    void Update()
    {
        // ���̽� ���� ��
        if(isRaceStart || isRaceFinish)
        {
            Turn();
            Move();
            AfterCollision();
            SpeedUp();
            ChangeTarget();
        }
        // ���̽� ���� ��
        if (isRaceFinish)
        {
            if (color.a <= 0.6f)
            {
                color.a += Time.deltaTime;
                endPanel.color = color;
            }
            else if(AudioManager.Instance.flag)
            {
                StartCoroutine(EndRace());
            }
        }
    }

    // �� ������
    void Move()
    {
        // �ʱ� ���ǵ� ����
        if (nowSpeed < carSpeed - 0.01f)
            nowSpeed = Mathf.Lerp(nowSpeed, carSpeed, Time.deltaTime * 3f);
        else
            nowSpeed = carSpeed;
        navMeshAgent.speed = nowSpeed * collisionVal;
        // �� �浹 ��
        if (isCarCollision)
        {
            transform.position += carCollsionDir * navMeshAgent.speed * 0.5f * Time.deltaTime;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    // �� ȸ��
    void Turn()
    {
        prePos = nowPos;
        nowPos = transform.position;
        
        // x�� ȸ��
        if(Mathf.Abs(nowPos.y - prePos.y) > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(nowPos - prePos);
        }
        
        // �¿� ȸ��
        Vector3 direction = navMeshAgent.desiredVelocity;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            StartCoroutine(TurnSlow(targetAngle));
            // ȸ�� �� ����
            if (Mathf.Abs(preForwardAngle.y - targetAngle.y) > 0.002f && !isRaceFinish)
            {
                isSpeedUp = false;
                collisionVal = Mathf.Lerp(collisionVal, ((0.002f / Mathf.Abs(preForwardAngle.y - targetAngle.y)) < 0.5f) 
                    ? 0.5f : (0.002f / Mathf.Abs(preForwardAngle.y - targetAngle.y)), Time.deltaTime);
            }
            else if(!isRaceFinish)
                isSpeedUp = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * 2.5f);
        }
    }

    // �浹 �� ������
    void AfterCollision()
    {
        if (!isCarCollision && isAfterCollision)
        {
            collisionVal = Mathf.Lerp(collisionVal, 1f, Time.deltaTime);
            if (collisionVal > 0.96f)
            {
                isAfterCollision = false;
                collisionVal = 1f;
            }
        }
    }

    // �ӵ� ����
    void SpeedUp()
    {
        if (isSpeedUp)
        {
            collisionVal = Mathf.Lerp(collisionVal, 1f, Time.deltaTime);
            if (collisionVal >= 0.96f)
            {
                collisionVal = 1f;
                isSpeedUp = false;
            }
        }
    }

    // Ÿ�� ����
    void ChangeTarget()
    {
        if(navMeshAgent.remainingDistance < 5f)
        {
            if(index != target.Length -1)
                navMeshAgent.SetDestination(target[++index].position);
        }
        if (index == target.Length - 1)
        {
            isSpeedUp = false;
            collisionVal = Mathf.Lerp(collisionVal, 0f, Time.deltaTime);
            if (collisionVal < 0.03f)
            {
                collisionVal = 0;
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.isStopped = true;
                isRaceStart = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 ��
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Obstacle")
        {
            navMeshAgent.isStopped = true;
            isSpeedUp = false;
            collisionVal = 0.1f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // ���� �浹
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "OtherCar" && !isCarCollision)
        {
            isSpeedUp = false;
            collisionVal = 0.3f;
            isAfterCollision = false;
        }
        // �� �Ǵ� ��ֹ� �浹
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Obstacle")
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            isSpeedUp = false;
            collisionVal = 0.1f;
            carCollsionDir = new Vector3(transform.position.x - collision.transform.position.x, 0, transform.position.z - collision.transform.position.z);
            isCarCollision = true;
            rigid.AddForce(carCollsionDir * 100, ForceMode.Impulse);
            StartCoroutine(CarCollision());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �浹 ����
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "OtherCar" && !isCarCollision && !isRaceFinish)
        {
            isSpeedUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���̽� ������ �浹
        if(other.gameObject.name == "Start End Point" && index != 0)
        {
            isRaceFinish = true;
        }
    }

    // ���� Ÿ�̸�
    IEnumerator TimeSecond()
    {
        while(num >= 0)
        {
            yield return seconds;
            num--;
            if (num == -1)
                break;
            if (num == 0)
            {
                numberText.fontSize = 100;
                numberText.text = "Start!";
            }
            else
                numberText.text = num.ToString();
        }
        textPanel.SetActive(false);
        guidePanel.SetActive(false);
        isRaceStart = true;
    }

    // �� �浹 ��
    IEnumerator CarCollision()
    {
        yield return new WaitForSeconds(2f);
        isCarCollision = false;
        isAfterCollision = true;
    }

    // �¿� ȸ�� �Ǵ�
    IEnumerator TurnSlow(Quaternion targetAngle)
    {
        yield return new WaitForSeconds(0.2f);
        preForwardAngle = targetAngle;
    }
    
    // ���̽� ����
    IEnumerator EndRace()
    {
        endText.text = (GameManager.Instance.quest.isGameWin) ? "Win!" : "Lose";
        endText.gameObject.SetActive(true);
        AudioManager.Instance.FadeOutMusic();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MocoForest");
    }
}
