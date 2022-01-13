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
    bool isJump;
    bool isCollision;
    public bool isShopping;
    public bool isTalking;

    Vector3 moveVec;
    [SerializeField]
    GameObject nearObject;
    [SerializeField]
    Shop shop;
    NPC npc;
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
        if(!isTalking)
        {
            Move();
            Turn();
            Jump();
        }
        Interaction();
    }
    // 입력
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
    }
    // 플레이어 이동
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        // 물체 충돌 시 이동 제한
        if(!isCollision)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // 플레이어 회전
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }
    // 점프
    void Jump()
    {
        if (jDown && !isJump)
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Interaction()
    {
        // 상점 상호작용
        if(iDown && nearObject != null && !isJump && !isShopping && !isTalking)
        {
            if(nearObject.tag == "Shop")
            {
                shop.Enter(this);
            }
        }
        else if (iDown && !isShopping && isTalking)
        {
            if(shop.isNext)
            {
                shop.Close();
            }
        }
        else if (iDown && isShopping && nearObject != null && nearObject.tag == "ShopItem")
        {
            if (shop.isNext && shop.isClose)
                shop.Close();

            else if (shop.isNext)
            {
                shop.ShowAnswer();
            }
            else
            {
                int index = nearObject.GetComponent<Item>().value;
                shop.Buy(index);
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
            isCollision = true;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop" && !isShopping)
        {
            nearObject = other.gameObject;
        }
        else if(other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
        if(other.tag == "Shopping" && isShopping)
        {
            isShopping = false;
            shop.Exit();
            nearObject = null;
        }
        else if (other.tag == "ShopItem" && nearObject != null)
        {
            nearObject = null;
        }
    }

}