using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;
    bool isJump;
    bool isCollision;

    Vector3 moveVec;

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
    }
    // 입력
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
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
        else
            isCollision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
    }
}
