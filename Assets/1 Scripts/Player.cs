using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
    public float speed;
    public int coin;
    public int stone; // ä���� �� ����
    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;
    bool iDown;
    bool sDown;
    bool tDown;
    bool isJump; 
    bool isCollision;
    public bool isShopping;
    public bool isTalking;
    public bool isInventory;
    public int[] hasItem;
    public GameObject selectItem; // �÷��̾ �κ��丮���� ������ ������

    public Shop shop;
    public Inventory inventory;

    Vector3 moveVec;
    GameObject nearObject;
    Rigidbody rigid;
    Animator anim;

    private void Update()
    {
        // īƮ���̽� �߿��� ����X
        if(SceneManager.GetActiveScene().name != "CartRacing")
        {
            GetInput();
            Move();
            Turn();
            Jump();
            Interaction();
        }
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
        tDown = Input.GetButtonDown("Inventory"); // Tab key
    }
    // �÷��̾� �̵�
    void Move()
    {
        if (!isTalking && !isInventory)
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        else
            moveVec = new Vector3(0, 0, 0).normalized; // ��ȭ ���� ���
        
        // ��ü �浹 �� �̵� ����
        if(!isCollision && !isTalking && !isInventory)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // �÷��̾� ȸ��
    void Turn()
    {
        if(!isTalking && !isInventory)
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // ����
    void Jump()
    {
        if (jDown && !isJump && !isTalking && !isInventory) //npc��ó���� ���� ����
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Interaction()
    {
        // ���� ��ȣ�ۿ�
        // 
        // ���� ����
        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking && !isInventory)
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
        // ���� ����
        //
        // ���� ������ ��ȣ�ۿ�
        else if (iDown && isShopping && nearObject != null && nearObject.tag == "ShopItem" && !isTalking && !isInventory && !shop.isSell)
        {
            int index = nearObject.GetComponent<Item>().value;
            shop.Buy(index);
        }
        // ���� ������ ���� Ȯ�� ��� �ѱ�
        else if (sDown && isShopping && isTalking && !shop.isSell)
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
        // ���� ������ �Ǹ�
        //
        // �Ǹ� ����
        if (iDown && isShopping && nearObject != null && nearObject.tag == "Shop" && !isTalking && !isInventory)
        {
            shop.Sell();
        }
        else if (sDown && isShopping && isTalking && shop.isSell)
        {
            // �Ǹ��� ������ ���� ���� ǥ��(������ or �κ��丮)
            if (!shop.isClose && shop.isNext)
            {
                shop.ShowSellAnswer();
            }
            // ������ �Ǹ� ����
            else if (!shop.isClose && !shop.isNext) {
                // �Ǹ� ������ ���� ���� ��
                if(shop.sellChoosePanel.activeSelf)
                {
                    shop.CloseSellAnswer();
                }
                // ������ �Ǹ� ���� ���� ��
                else if (!shop.sellCountPanel.activeSelf && !isInventory)
                {
                    shop.isNext = true;
                    shop.isClose = true;
                }
            }
            // �Ǹ� ����
            else if (shop.isClose && shop.isNext && !shop.sellCountPanel.activeSelf)
            {
                shop.Close();
            }
        }

        // �κ��丮
        //
        // �÷��̾� �̾߱� ���� ���
        if(isTalking)
        {
            inventory.btnInventory.SetActive(false);
        }
        // �κ��丮�� ������ ���
        else if(!isTalking && !isInventory)
        {
            inventory.btnInventory.SetActive(true);
        }
        // tab Ű ���� �κ��丮 ����
        if(tDown && !isTalking && !isInventory)
        {
            inventory.ShowInventory();
        }
        // tab Ű ���� �κ��丮 �ݱ�
        else if(tDown && !isTalking && inventory.panelInventroy.activeSelf)
        {
            isInventory = false;
            inventory.ShowBtn();
        }
        // �κ��丮 ���� ��ư
        else if(sDown && isInventory && inventory.btnInventory.activeSelf)
        {
            isInventory = false;
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
        if (!GameManager.Instance.quest.isMapChanged && other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // ���� ����
        }

        if (!GameManager.Instance.quest.isMapChanged && other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false); // ���� ����
        }
        else if (other.gameObject.tag != "Shop" && other.gameObject.tag != "Shopping" && other.gameObject.tag != "ShopItem")
            isCollision = true; // �ʿ� �浹 ��
    }

    private void OnTriggerStay(Collider other)
    {
        // ���� ����, �Ǹ� ���� �ν�
        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
            if (!isTalking)
                shop.SetEPosition(other);
            else
                shop.showKeyE.SetActive(false);
        }
        // ���� ������ ��ȣ�ۿ� ���� ���� �ν�
        else if(other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
            if (!isTalking)
                shop.SetEPosition(other);
            else
                shop.showKeyE.SetActive(false);
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
        else if ((other.tag == "ShopItem" || other.tag == "Shop") && nearObject != null)
        {
            nearObject = null;
            shop.showKeyE.SetActive(false);
        }
    }
}