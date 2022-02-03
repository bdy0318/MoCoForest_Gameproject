using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int coin;
    public int smallrock;
    public int maxsmallrock;

    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;
    bool iDown;
    bool iDown1;
    bool sDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;

    bool isJump;
    bool isCollision;
    bool isSwap;
    bool isFireReady;

    public bool isShopping;
    public bool isTalking;
    public int[] hasItem;
    public GameObject[] weapon;
    public bool[] hasWeapons;
    

    Vector3 moveVec;
    GameObject nearObject;
    GameObject nearObject_w;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

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
        Attack();
        Swap();
        Interaction();
        weaponInteraction();
    }
    // �Է�
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interaction"); // E key
        iDown1 = Input.GetButtonDown("weaponInteraction"); //Q key
        sDown = Input.GetButtonDown("Submit"); // Enter or Space key
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    // �÷��̾� �̵�
    void Move()
    {
        if (!isTalking)
        {
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            if (isSwap)
                moveVec = Vector3.zero;
        }
        else
            moveVec = new Vector3(0, 0, 0).normalized; // ��ȭ ���� ���

        // ��ü �浹 �� �̵� ����
        if (!isCollision && !isTalking)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // �÷��̾� ȸ��
    void Turn()
    {
        if (!isTalking)
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // ����
    void Jump()
    {
        if (jDown && !isJump && !isTalking )
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Attack() 
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }

    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0) && !isJump )
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1) && !isJump )
            return;
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if ( (sDown1 || sDown2 )&& !isJump)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapon[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.1f);
        }
        if (sDown3 && !isJump)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
                equipWeapon = null;
            }
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void weaponInteraction()
    {
        //����ȹ��
        if (iDown1 && nearObject_w != null)
        {
            if (nearObject_w.tag == "Weapon")
            {
                Item item = nearObject_w.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                Destroy(nearObject_w);
            }
        }
    }
    void Interaction()
    {
        // ���� ��ȣ�ۿ�
        // ���� ����

        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking)
        {
            if (nearObject.tag == "Shop")
            {
                shop.Enter(this);
            }
        }
        // ���� ���� ��� �ѱ�
        else if (sDown && !isShopping && isTalking)
        {
            if (shop.isNext)
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
                
            else if (shop.isNext && !shop.isClose && shop.answerPanel.activeSelf)
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
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false; // ���� Ȱ��
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.quest.isMapChanged && other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // ���� ����
        }

        if (!GameManager.Instance.quest.isMapChanged && other.gameObject.tag == "Ground")

        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Rock:
                    smallrock += item.value;
                    if (smallrock > maxsmallrock)
                        smallrock = maxsmallrock;
                    break;
            }
            Destroy(other.gameObject);
        }
            
        else if (other.gameObject.tag != "Shop" && other.gameObject.tag != "Shopping" && other.gameObject.tag != "ShopItem")
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
        else if (other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
        }
            
        else if (other.tag == "Weapon")
        {
            nearObject_w = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        isCollision = false;
        // ���� �������� ���������� ���
        if (other.tag == "Shopping" && isShopping)
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
            
        else if (other.tag == "Weapon")
        {
            nearObject_w = null;
        }
    }
}