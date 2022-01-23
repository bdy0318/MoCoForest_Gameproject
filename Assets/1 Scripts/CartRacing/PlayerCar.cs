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
    public Car car; // ��� ��
    public Transform[] point; // ���� ����Ʈ
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
        // ���̽� ���� ��
        if(car.isRaceStart)
        {
            GetInput();
            Turn();
            Move();
            AfterCollision();
            SpeedUp();
            CarPositionReset();
        }
        // ���̽� ����
        if(car.isRaceFinish)
        {
            isSpeedUp = false;
            // �ӵ� ����
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
        // ���� �Է�
        hAxis = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(hAxis) > 0.15f)
            hAxis = Mathf.Sign(hAxis) * 1;
        // ���� �Է�
        if (!isCarCollision)
            vAxis = Input.GetAxisRaw("Vertical");
        else
            vAxis = 0;

        if (Mathf.Abs(vAxis) > 0.15f)
            vAxis = Mathf.Sign(vAxis) * 1;
    }

    // �� ������
    void Move()
    {
        nowSpeed = Mathf.Lerp(nowSpeed, carSpeed * vAxis, Time.deltaTime); // �ӵ�
        // �� �浹 ��
        if(isCarCollision)
            transform.position += carCollsionDir * nowSpeed * collisionVal * Time.deltaTime;
        else
            transform.position += transform.forward * nowSpeed * collisionVal * Time.deltaTime;
    }

    // �� �¿� ȸ��
    void Turn()
    {
        nowTurn = Mathf.Lerp(nowTurn, hAxis, Time.deltaTime);
        transform.Rotate(new Vector3(0, nowTurn * 50 * collisionVal * Time.deltaTime, 0));
    }

    // �� �浹 ��
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

    // �� ����Ʈ ����
    void CarPositionReset()
    {
        // �� ������ ��� ����� ����Ʈ���� ����
        if ((Mathf.Abs(transform.eulerAngles.z) > 90 && Mathf.Abs(transform.eulerAngles.z) < 270) 
            || (Mathf.Abs(transform.eulerAngles.x) > 90 && Mathf.Abs(transform.eulerAngles.x) < 270))
        {
            if(!isCarReset)
                StartCoroutine(CarReset()); // ���� �ڷ�ƾ ����
            if(isCarResetStart)
            {
                // ���� �� ����Ʈ�� �ƴϸ�
                if (nowPoint != point.Length - 1)
                {
                    // ���� ����Ʈ�� �� ����� ���
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
        // ���� ����Ʈ ����
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
        // ���� �浹
        if (collision.gameObject.tag != "Road" && collision.gameObject.tag != "Car" && !isCarCollision)
        {
            isSpeedUp = false;
            collisionVal = 0.3f;
            isAfterCollision = false;
        }
        // �� �Ǵ� ��ֹ� �浹
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
        // ���� �浹���� ��� ���
        if(collision.gameObject.tag != "Road" && collision.gameObject.tag != "Car" && !isCarCollision && !isAfterCollision)
        {
            isSpeedUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ���� ���� ��
        if (other.gameObject.name == "Start End Point" && nowPoint == point.Length-1 && !car.isRaceFinish)
        {
            car.isRaceFinish = true;
            GameManager.Instance.quest.isGameWin = true;
        }
    }

    // �� �浹 ��
    IEnumerator CarCollision()
    {
        yield return new WaitForSeconds(2f);
        isCarCollision = false;
        isAfterCollision = true;
    }

    // �� ��ġ ���� ���� �Ǻ�
    IEnumerator CarReset()
    {
        isCarReset = true;
        yield return new WaitForSeconds(3f);
        isCarResetStart = true;
    }
}
