using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ͽ� ���, ���� ���, ����->���� �̵�, ����->���� �̵�
public enum MoleState { UnderGround = 0, OnGround, MoveUp, MoveDown}
//�δ��� ����(�⺻, ����-, �ð�(����)+)
public class MoleFSM : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;  //�޺� �ʱ�ȭ
    [SerializeField]
    private float waitTimeOnGround;     //���鿡 �ö�ͼ� ����������� ��ٸ��� �ð�
    [SerializeField]
    private float limitMinY;            //������ �� �ִ� �ּ� y ��ġ
    [SerializeField]
    private float limitMaxY;            //�ö�� �� �ִ� �ִ� y ��ġ

    private Movement movement;      //��/�Ʒ� �̵��� ���� Movement

    public int MoleType;            //�δ��� ���� - 0: Normal, 1: Dog, 2: Cat

    // �δ����� ���� ����(set�� MoleFSM Ŭ���� ���ο�����)
    public MoleState MoleState { private set; get; }

    private void Awake()
    {
        movement = GetComponent<Movement>();

        ChangeState(MoleState.UnderGround);
    }

    public void ChangeState(MoleState newState)
    {
        // ������ ������̴� ���� ����
        StopCoroutine(MoleState.ToString());
        // ���� ����
        MoleState = newState;
        // ���ο� ���� ���
        StartCoroutine(MoleState.ToString());
    }

    //�δ����� �ٴڿ��� ����ϴ� ���·� ���ʿ� �ٴ� ��ġ�� �δ��� ��ġ ����
    private IEnumerator UnderGround()
    {
        // �̵������� (0,0,0) [����]
        movement.MoveTo(Vector3.zero);
        //�δ����� y��ġ�� Ȧ�� �����ִ� limitMinY ��ġ�� ����
        transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

        yield return null;
    }

    //�δ����� Ȧ ������ �����ִ� ���·� waitTimeOnGround���� ���
    private IEnumerator OnGround()
    {
        // �̵������� (0,0,0) [����]
        movement.MoveTo(Vector3.zero);
        //�δ����� y��ġ�� Ȧ�� �����ִ� limitMaxY ��ġ�� ����
        transform.position = new Vector3(transform.position.x, limitMaxY, transform.position.z);

        //waitTimeOnGround �ð� ���� ���
        yield return new WaitForSeconds(waitTimeOnGround);

        //�δ����� ���¸� MoveDown���� ����
        ChangeState(MoleState.MoveDown);

    }

    //�δ����� Ȧ ������ ������ ����(maxYPosOnGround ��ġ���� ���� �̵�)
    private IEnumerator MoveUp()
    {
        //�̵����� (0,1,0) [��]
        movement.MoveTo(Vector3.up);

        while (true)
        {
            //�δ����� y��ġ�� limitMaxY�� �����ϸ� ���º���
            if(transform.position.y >= limitMaxY)
            {
                //OnGround ���·� ����
                ChangeState(MoleState.OnGround);
            }

            yield return null;
        }
    }

    //�δ����� Ȧ�� ���� ����(minYPosUnderGround ��ġ���� �Ʒ��� �̵�)
    private IEnumerator MoveDown()
    {
        //�̵����� (0,-1,0) [�Ʒ�]
        movement.MoveTo(Vector3.down);

        while (true)
        {
            //�δ����� y��ġ�� limitMinY�� �����ϸ� �ݺ��� ����
            if(transform.position.y <= limitMinY)
            {
                break;
            }

            yield return null;
        }

        //// ��ġ�� ������ ���ϰ� �������� �� �δ����� �Ӽ��� �Ϲ��̸� �޺� �ʱ�ȭ
        //if(MoleType == 0)   
        //{
        //    gameController.Combo = 0;
        //}

        //UnderGround ���·� ����
        ChangeState(MoleState.UnderGround);
    }
}
