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
    public GameObject guidePanel; // 조작 가이드 UI
    public Text endText;
    public Image endPanel;
    public float carSpeed; // 최대 속도
    float nowSpeed; // 현재 속도
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
        StartCoroutine(TimeSecond()); // 타이머 실행
    }

    void Update()
    {
        // 레이스 시작 후
        if(isRaceStart || isRaceFinish)
        {
            Turn();
            Move();
            AfterCollision();
            SpeedUp();
            ChangeTarget();
        }
        // 레이스 종료 시
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

    // 차 움직임
    void Move()
    {
        // 초기 스피드 증가
        if (nowSpeed < carSpeed - 0.01f)
            nowSpeed = Mathf.Lerp(nowSpeed, carSpeed, Time.deltaTime * 3f);
        else
            nowSpeed = carSpeed;
        navMeshAgent.speed = nowSpeed * collisionVal;
        // 차 충돌 시
        if (isCarCollision)
        {
            transform.position += carCollsionDir * navMeshAgent.speed * 0.5f * Time.deltaTime;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    // 차 회전
    void Turn()
    {
        prePos = nowPos;
        nowPos = transform.position;
        
        // x축 회전
        if(Mathf.Abs(nowPos.y - prePos.y) > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(nowPos - prePos);
        }
        
        // 좌우 회전
        Vector3 direction = navMeshAgent.desiredVelocity;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            StartCoroutine(TurnSlow(targetAngle));
            // 회전 시 감속
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

    // 충돌 후 움직임
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

    // 속도 증가
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

    // 타겟 변경
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
        // 충돌 시
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Obstacle")
        {
            navMeshAgent.isStopped = true;
            isSpeedUp = false;
            collisionVal = 0.1f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 지형 충돌
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "OtherCar" && !isCarCollision)
        {
            isSpeedUp = false;
            collisionVal = 0.3f;
            isAfterCollision = false;
        }
        // 차 또는 장애물 충돌
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
        // 충돌 종료
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "OtherCar" && !isCarCollision && !isRaceFinish)
        {
            isSpeedUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 레이스 도착지 충돌
        if(other.gameObject.name == "Start End Point" && index != 0)
        {
            isRaceFinish = true;
        }
    }

    // 시작 타이머
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

    // 차 충돌 시
    IEnumerator CarCollision()
    {
        yield return new WaitForSeconds(2f);
        isCarCollision = false;
        isAfterCollision = true;
    }

    // 좌우 회전 판단
    IEnumerator TurnSlow(Quaternion targetAngle)
    {
        yield return new WaitForSeconds(0.2f);
        preForwardAngle = targetAngle;
    }
    
    // 레이스 종료
    IEnumerator EndRace()
    {
        endText.text = (GameManager.Instance.quest.isGameWin) ? "Win!" : "Lose";
        endText.gameObject.SetActive(true);
        AudioManager.Instance.FadeOutMusic();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MocoForest");
    }
}
