using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public float carSpeed;
    public float collisionVal;
    float nowSpeed;
    float nowTurn;
    float hAxis;
    float vAxis;
    public bool isAfterCollision;
    public bool isCarCollision;
    public bool isSpeedUp;
    bool isCarReset;
    bool isCarResetStart;
    public Car car; // 상대 차
    public Transform[] point; // 갱신 포인트
    public int nowPoint;

    Vector3 carCollsionDir;
    Rigidbody rigid;
    private void Start()
    {
        nowSpeed = 0;
        nowTurn = 0;
        nowPoint = 0;
        collisionVal = 1f;
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // 레이스 진행 중
        if(car.isRaceStart)
        {
            GetInput();
            Turn();
            Move();
            AfterCollision();
            SpeedUp();
            CarPositionReset();
        }
        // 레이스 종료
        if(car.isRaceFinish)
        {
            isSpeedUp = false;
            // 속도 감속
            if (collisionVal > 0.01f)
            {
                collisionVal = Mathf.Lerp(collisionVal, 0, Time.deltaTime);
                transform.position += transform.forward * nowSpeed * collisionVal * 0.8f * Time.deltaTime;
            }
            else
                collisionVal = 0;
        }
    }

    void GetInput()
    {
        // 수평 입력
        hAxis = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(hAxis) > 0.15f)
            hAxis = Mathf.Sign(hAxis) * 1;
        // 수직 입력
        if (!isCarCollision)
            vAxis = Input.GetAxisRaw("Vertical");
        else
            vAxis = 0;

        if (Mathf.Abs(vAxis) > 0.15f)
            vAxis = Mathf.Sign(vAxis) * 1;
    }

    // 차 움직임
    void Move()
    {
        nowSpeed = Mathf.Lerp(nowSpeed, carSpeed * vAxis, Time.deltaTime); // 속도
        // 차 충돌 시
        if(isCarCollision)
            transform.position += carCollsionDir * nowSpeed * collisionVal * Time.deltaTime;
        else
            transform.position += transform.forward * nowSpeed * collisionVal * Time.deltaTime;
    }

    // 차 좌우 회전
    void Turn()
    {
        nowTurn = Mathf.Lerp(nowTurn, hAxis, Time.deltaTime);
        transform.Rotate(new Vector3(0, nowTurn * 50 * collisionVal * Time.deltaTime, 0));
    }

    // 차 충돌 후
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
        if(isSpeedUp)
        {
            collisionVal = Mathf.Lerp(collisionVal, 1f, Time.deltaTime);
            if(collisionVal >= 0.96f)
            {
                collisionVal = 1f;
                isSpeedUp = false;
            }
        }
    }

    // 차 포인트 갱신
    void CarPositionReset()
    {
        // 차 뒤집힌 경우 가까운 포인트에서 갱신
        if ((Mathf.Abs(transform.eulerAngles.z) > 90 && Mathf.Abs(transform.eulerAngles.z) < 270) 
            || (Mathf.Abs(transform.eulerAngles.x) > 90 && Mathf.Abs(transform.eulerAngles.x) < 270))
        {
            if(!isCarReset)
                StartCoroutine(CarReset()); // 리셋 코루틴 실행
            if(isCarResetStart)
            {
                // 제일 끝 포인트가 아니면
                if (nowPoint != point.Length - 1)
                {
                    // 다음 포인트가 더 가까운 경우
                    if (Vector3.SqrMagnitude(transform.position - point[nowPoint].position) > Vector3.SqrMagnitude(transform.position - point[nowPoint + 1].position))
                    {
                        transform.position = point[nowPoint+1].position;
                        transform.rotation = point[nowPoint+1].rotation;
                    }
                    else
                    {
                        transform.position = point[nowPoint].position;
                        transform.rotation = point[nowPoint].rotation;
                    }
                }
                else
                    transform.position = point[nowPoint].position;
                isCarReset = false;
                isCarResetStart = false;
            }
        }
        else
        {
            isCarReset = false;
            isCarResetStart = false;
        }
        // 현재 포인트 갱신
        if(nowPoint != point.Length - 1)
        {
            if (Vector3.Distance(point[nowPoint + 1].position, transform.position) < 10f)
            {
                nowPoint++;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 지형 충돌
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "Car" && !isCarCollision)
        {
            isSpeedUp = false;
            collisionVal = 0.3f;
            isAfterCollision = false;
        }
        // 차 또는 장애물 충돌
        if(collision.gameObject.tag == "OtherCar" || collision.gameObject.tag == "Obstacle")
        {
            isSpeedUp = false;
            collisionVal = 0.1f;
            isCarCollision = true;
            carCollsionDir = new Vector3(transform.position.x - collision.transform.position.x, 0, transform.position.z - collision.transform.position.z);
            rigid.AddForce(carCollsionDir * 100, ForceMode.Impulse);
            StartCoroutine(CarCollision());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 지형 충돌에서 벗어난 경우
        if(collision.gameObject.tag != "Road" && collision.gameObject.tag != "Car" && !isCarCollision && !isAfterCollision)
        {
            isSpeedUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 도착 지점 도착 시
        if (other.gameObject.name == "Start End Point" && nowPoint == point.Length-1 && !car.isRaceFinish)
        {
            car.isRaceFinish = true;
            GameManager.Instance.quest.isGameWin = true;
        }
    }

    // 차 충돌 시
    IEnumerator CarCollision()
    {
        yield return new WaitForSeconds(2f);
        isCarCollision = false;
        isAfterCollision = true;
    }

    // 차 위치 리셋 여부 판별
    IEnumerator CarReset()
    {
        isCarReset = true;
        yield return new WaitForSeconds(3f);
        isCarResetStart = true;
    }
}
