using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int coin;
    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;
    bool iDown;
    bool sDown;
    bool isJump;
    bool isCollision;
    public bool isShopping;
    public bool isTalking;
    public int[] hasItem;

    Vector3 moveVec;
    GameObject nearObject;
    [SerializeField]
    Shop shop;
    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Interaction();
    }
    // 입력
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction"); // E key
        sDown = Input.GetButtonDown("Submit"); // Enter or Space key
    }
    // 플레이어 이동
    void Move()
    {
        if (!isTalking)
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        else
            moveVec = new Vector3(0, 0, 0).normalized; // 대화 중인 경우
        
        // 물체 충돌 시 이동 제한
        if(!isCollision && !isTalking)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // 플레이어 회전
    void Turn()
    {
        if(!isTalking)
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // 점프
    void Jump()
    {
        if (jDown && !isJump && !isTalking)
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Interaction()
    {
        // 상점 상호작용
        // 상점 입장
        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking)
        {
            if(nearObject.tag == "Shop")
            {
                shop.Enter(this);
            }
        }
        // 상점 입장 대사 넘김
        else if (sDown && !isShopping && isTalking)
        {
            if(shop.isNext)
            {
                shop.Close();
            }
        }
        // 상점 아이템 상호작용
        else if (iDown && isShopping && nearObject != null && nearObject.tag == "ShopItem" && !isTalking)
        {
            int index = nearObject.GetComponent<Item>().value;
            shop.Buy(index);
        }
        // 상점 아이템 구매 확인 대사 넘김
        else if (sDown && isShopping && isTalking)
        {
            // 구매 여부 선택지 이후 대사 넘김
            if (shop.isNext && shop.isClose && !shop.answerPanel.activeSelf)
                shop.Close();
            // 구매 선택지, 대사 닫기
            else if(shop.isNext && !shop.isClose && shop.answerPanel.activeSelf)
            {
                shop.isClose = true;
                shop.CloseAnswer();
            }
            // 선택지 표시
            else if (shop.isNext && !shop.answerPanel.activeSelf)
            {
                shop.ShowAnswer();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJump = false; // 점프 활성
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // 점프 중지
        }

        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false); // 점프 중지
        }
        else if(other.gameObject.tag != "Shop" && other.gameObject.tag != "Shopping" && other.gameObject.tag != "ShopItem")
            isCollision = true; // 맵에 충돌 중
    }

    private void OnTriggerStay(Collider other)
    {
        // 상점 출입 지점 인식
        if (other.tag == "Shop" && !isShopping)
        {
            nearObject = other.gameObject;
        }
        // 상점 아이템 상호작용 가능 여부 인식
        else if(other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
        // 상점 지역에서 빠져나오는 경우
        if(other.tag == "Shopping" && isShopping)
        {
            isShopping = false;
            shop.Exit();
            nearObject = null;
        }
        // 상점 이용 시 주변에 상호작응 가능한 아이템 없는 경우
        else if (other.tag == "ShopItem" && nearObject != null)
        {
            nearObject = null;
        }
    }
}