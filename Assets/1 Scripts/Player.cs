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
    public int stone; // 채집한 돌 개수

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
    bool tDown;
    bool fDown;
    bool isJump; 
    bool isCollision;
    bool isSwap;
    bool isFireReady;
    public bool isShopping;
    public bool isTalking;
    public bool isInventory;
    public bool isMenu;
    public int[] hasItem;
    public GameObject[] weapon;
    public bool[] hasWeapons;
    public GameObject selectItem; // 플레이어가 인벤토리에서 선택한 아이템

    public Shop shop;
    public Inventory inventory;

    Vector3 moveVec;
    GameObject nearObject;
    GameObject nearObject_w;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    Rigidbody rigid;
    Animator anim;

    private void Update()
    {
        // 카트레이싱 중에는 실행X
        if(SceneManager.GetActiveScene().name != "CartRacing")
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
    }
    // 입력
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
        tDown = Input.GetButtonDown("Inventory"); // Tab key
        sDown1 = Input.GetButtonDown("Swap1"); //숫자1번
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    // 플레이어 이동
    void Move()
    {
        if (!isTalking && !isInventory && !isSwap)
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        else
            moveVec = new Vector3(0, 0, 0).normalized; // 대화 중인 경우
        
        // 물체 충돌 시 이동 제한
        if(!isCollision && !isTalking && !isInventory && !isSwap)
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }
    // 플레이어 회전
    void Turn()
    {
        if(!isTalking && !isInventory)
            transform.LookAt(Vector3.MoveTowards(transform.position, transform.position + moveVec, Time.deltaTime));
    }
    // 점프
    void Jump()
    {
        if (jDown && !isJump && !isTalking && !isInventory && !isMenu) //npc근처에서 점프 금지
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

        if (fDown && isFireReady && !isSwap && !isTalking && !isInventory)
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
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1) && !isJump)
            return;
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if ((sDown1 || sDown2) && !isJump)
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
        //무기획득
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
        // 상점 상호작용
        // 
        // 상점 입장
        if (iDown && nearObject != null && !isJump && !isShopping && !isTalking && !isInventory)
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
        // 상점 구매
        //
        // 상점 아이템 상호작용
        else if (iDown && isShopping && nearObject != null && nearObject.tag == "ShopItem" && !isTalking && !isInventory && !shop.isSell)
        {
            int index = nearObject.GetComponent<Item>().value;
            shop.Buy(index);
        }
        // 상점 아이템 구매 확인 대사 넘김
        else if (sDown && isShopping && isTalking && !shop.isSell)
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
        // 상점 아이템 판매
        //
        // 판매 입장
        if (iDown && isShopping && nearObject != null && nearObject.tag == "Shop" && !isTalking && !isInventory)
        {
            shop.Sell();
        }
        else if (sDown && isShopping && isTalking && shop.isSell)
        {
            // 판매할 아이템 종류 선택 표시(돌맹이 or 인벤토리)
            if (!shop.isClose && shop.isNext)
            {
                shop.ShowSellAnswer();
            }
            // 아이템 판매 선택
            else if (!shop.isClose && !shop.isNext) {
                // 판매 아이템 종류 선택 시
                if(shop.sellChoosePanel.activeSelf)
                {
                    shop.CloseSellAnswer();
                }
                // 아이템 판매 개수 선택 시
                else if (!shop.sellCountPanel.activeSelf && !isInventory)
                {
                    shop.isNext = true;
                    shop.isClose = true;
                }
            }
            // 판매 종료
            else if (shop.isClose && shop.isNext && !shop.sellCountPanel.activeSelf)
            {
                shop.Close();
            }
        }

        // 인벤토리
        //
        // 플레이어 이야기 중인 경우
        if(isTalking)
        {
            inventory.btnInventory.SetActive(false);
        }
        // 인벤토리가 열리는 경우
        else if(!isTalking && !isInventory)
        {
            inventory.btnInventory.SetActive(true);
        }
        // tab 키 사용시 인벤토리 열기
        if(tDown && !isTalking && !isInventory)
        {
            inventory.ShowInventory();
        }
        // tab 키 사용시 인벤토리 닫기
        else if(tDown && !isTalking && inventory.panelInventroy.activeSelf)
        {
            isInventory = false;
            inventory.ShowBtn();
        }
        // 인벤토리 선택 버튼
        else if(sDown && isInventory && inventory.btnInventory.activeSelf)
        {
            isInventory = false;
        }

        // 메뉴
        //
        // 메뉴 버튼 선택
        if(sDown && !Menu.Instance.pausePanel.activeSelf && Menu.Instance.isEnd && isMenu)
        {
            isMenu = false;
            Menu.Instance.isEnd = false;
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
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Stone:
                    stone += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }

        if (!GameManager.Instance.quest.isMapChanged && other.gameObject.transform.position.y < transform.position.y)
        {
            anim.SetBool("isJump", false); // 점프 중지
        }

        else if(!GameManager.Instance.quest.isMapChanged && other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false); // 점프 중지
        }
        else if (other.gameObject.tag != "Shop" && other.gameObject.tag != "Shopping" && other.gameObject.tag != "ShopItem")
            isCollision = true; // 맵에 충돌 중
    }

    private void OnTriggerStay(Collider other)
    {
        // 상점 출입, 판매 지점 인식
        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
            if (!isTalking)
                shop.SetEPosition(other);
            else
                shop.showKeyE.SetActive(false);
        }
        // 상점 아이템 상호작용 가능 여부 인식
        else if(other.tag == "ShopItem" && isShopping)
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
        // 상점 지역에서 빠져나오는 경우
        if(other.tag == "Shopping" && isShopping)
        {
            isShopping = false;
            shop.Exit();
            nearObject = null;
        }
        // 상점 이용 시 주변에 상호작응 가능한 아이템 없는 경우
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