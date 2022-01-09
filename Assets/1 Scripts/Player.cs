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
    // �Է�
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction"); // E key
        sDown = Input.GetButtonDown("Submit"); // Enter or Space key
    }
    // �÷��̾� �̵�
    void Move()
    {
        if (!isTalking)
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        else
            moveVec = new Vector3(0, 0, 0).normalized; // ��ȭ ���� ���
        
        // ��ü �浹 �� �̵� ����
        if(!isCollision && !isTalking)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // �÷��̾� ȸ��
    void Turn()
    {
        if(!isTalking)
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // ����
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
        // ���� ��ȣ�ۿ�
        // ���� ����
        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking)
        {
            if(nearObject.tag == "Shop")
            {
                shop.Enter(this);
            }
        }
        // ���� ���� ��� �ѱ�
        else if (sDown && !isShopping && isTalking)
        {
            if(shop.isNext)
            {
                shop.Close();
            }
        }
        // ���� ������ ��ȣ�ۿ�
        else if (iDown && isShopping && nearObject != null && nearObject.tag == "ShopItem" && !isTalking)
        {
            int index = nearObject.GetComponent<Item>().value;
            shop.Buy(index);
        }
        // ���� ������ ���� Ȯ�� ��� �ѱ�
        else if (sDown && isShopping && isTalking)
        {
            // ���� ���� ������ ���� ��� �ѱ�
            if (shop.isNext && shop.isClose && !shop.answerPanel.activeSelf)
                shop.Close();
            // ���� ������, ��� �ݱ�
            else if(shop.isNext && !shop.isClose && shop.answerPanel.activeSelf)
            {
                shop.isClose = true;
                shop.CloseAnswer();
            }
            // ������ ǥ��
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
            isJump = false; // ���� Ȱ��
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // ���� ����
        }

        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false); // ���� ����
        }
        else if(other.gameObject.tag != "Shop" && other.gameObject.tag != "Shopping" && other.gameObject.tag != "ShopItem")
            isCollision = true; // �ʿ� �浹 ��
    }

    private void OnTriggerStay(Collider other)
    {
        // ���� ���� ���� �ν�
        if (other.tag == "Shop" && !isShopping)
        {
            nearObject = other.gameObject;
        }
        // ���� ������ ��ȣ�ۿ� ���� ���� �ν�
        else if(other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
        // ���� �������� ���������� ���
        if(other.tag == "Shopping" && isShopping)
        {
            isShopping = false;
            shop.Exit();
            nearObject = null;
        }
        // ���� �̿� �� �ֺ��� ��ȣ���� ������ ������ ���� ���
        else if (other.tag == "ShopItem" && nearObject != null)
        {
            nearObject = null;
        }
    }
}