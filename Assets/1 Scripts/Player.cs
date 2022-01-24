using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public int coin;
<<<<<<< Updated upstream
<<<<<<< HEAD
    public int smallrock;
    public int maxsmallrock;

=======
=======
    public int smallrock;
    public int maxsmallrock;

>>>>>>> Stashed changes
    public int stone; // ä���� �� ����
>>>>>>> main
    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;
    bool iDown;
    bool iDown1;
    bool sDown;
<<<<<<< HEAD
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;

    bool isJump;
=======
    bool tDown;
    bool isJump; 
>>>>>>> main
    bool isCollision;
<<<<<<< Updated upstream
=======
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;

>>>>>>> Stashed changes
    bool isSwap;
    bool isFireReady;

    public bool isShopping;
    public bool isTalking;
    public bool isInventory;
    public int[] hasItem;
<<<<<<< HEAD
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
=======
    public GameObject selectItem; // �÷��̾ �κ��丮���� ������ ������
    public GameObject[] weapon;
    public bool[] hasWeapons;
    

    GameObject nearObject;
    GameObject nearObject_w;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

    public Shop shop;
    public Inventory inventory;

    Vector3 moveVec;
<<<<<<< Updated upstream
    GameObject nearObject;
>>>>>>> main
=======
>>>>>>> Stashed changes
    Rigidbody rigid;
    Animator anim;

    private void Update()
    {
<<<<<<< Updated upstream
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Swap();
        Interaction();
        weaponInteraction();
<<<<<<< Updated upstream
=======
=======

>>>>>>> Stashed changes
        // īƮ���̽� �߿��� ����X
        if(SceneManager.GetActiveScene().name != "CartRacing")
        {
            GetInput();
            Move();
            Turn();
            Jump();
            Interaction();
        }
>>>>>>> main
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
<<<<<<< Updated upstream
<<<<<<< HEAD
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
=======
=======
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
>>>>>>> Stashed changes
        tDown = Input.GetButtonDown("Inventory"); // Tab key
>>>>>>> main
    }
    // �÷��̾� �̵�
    void Move()
    {
<<<<<<< Updated upstream
<<<<<<< HEAD
        if (!isTalking)
        {
=======
        if (!isTalking && !isInventory)
>>>>>>> main
=======
        if (!isTalking && !isInventory) 
>>>>>>> Stashed changes
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            if (isSwap)
                moveVec = Vector3.zero;
        }
        else
            moveVec = new Vector3(0, 0, 0).normalized; // ��ȭ ���� ���

        // ��ü �浹 �� �̵� ����
<<<<<<< HEAD
        if (!isCollision && !isTalking)
=======
        if(!isCollision && !isTalking && !isInventory)
>>>>>>> main
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // �÷��̾� ȸ��
    void Turn()
    {
<<<<<<< HEAD
        if (!isTalking)
=======
        if(!isTalking && !isInventory)
>>>>>>> main
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // ����
    void Jump()
    {
<<<<<<< HEAD
        if (jDown && !isJump && !isTalking )
=======
        if (jDown && !isJump && !isTalking && !isInventory) //npc��ó���� ���� ����
>>>>>>> main
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
        // 
        // ���� ����
<<<<<<< HEAD

        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking)
=======
        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking && !isInventory)
>>>>>>> main
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
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false; // ���� Ȱ��
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
            
        if (other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // ���� ����
        }
        if (other.gameObject.tag == "Ground")
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
        else if (other.tag == "ShopItem" && isShopping)
        {
            nearObject = other.gameObject;
            if (!isTalking)
                shop.SetEPosition(other);
            else
                shop.showKeyE.SetActive(false);
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
        else if ((other.tag == "ShopItem" || other.tag == "Shop") && nearObject != null)
        {
            nearObject = null;
            shop.showKeyE.SetActive(false);
        }
            
        else if (other.tag == "Weapon")
        {
            nearObject_w = null;
        }
    }
}