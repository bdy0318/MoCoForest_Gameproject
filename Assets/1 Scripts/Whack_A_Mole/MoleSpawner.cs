using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField]
    private MoleFSM[] moles;    //�ʿ� �����ϴ� �δ�����
    [SerializeField]
    private Hole[] holes;       //�δ��� Ȧ
    [SerializeField]
    private float spawnTime;    //�δ��� ���� �ֱ�

    //�ѹ��� �����ϴ� �ִ� �δ��� ��
    public int MaxSpawnMole { set; get; } = 1;

    

    public void Setup()     //�δ��� ������ �ٷ� ���� �ʰ�, ī��Ʈ �ٿ��� �Ϸ�� �Ŀ� ��. -> Gamecontoroller
    {
        StartCoroutine("SpawnMole");
    }

    private IEnumerator SpawnMole()
    {
        while (true)
        {
            //MaxSpawnMole ���ڸ�ŭ �δ��� ����
            StartCoroutine("SpawnMultiMoles");

            //spawnTime �ð����� ���
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator SpawnMultiMoles()
    {
        //0 ~ Moles.Length-1 ������ ��ġ�� �ʴ� ������ ��� ����
        int[] indexs = RandomNumerics(moles.Length, moles.Length);
        int[] indexs2 = RandomNumerics(holes.Length, holes.Length);

        int currentSpawnMole = 0;   //���� ������ �δ��� ����
        int currentIndex = 0;       //indexs �迭 �ε���

       
        //���� �����ؾ��� �δ��� ���ڸ�ŭ �δ��� ����
        while (currentIndex < indexs.Length)
        {
            //�δ����� �ٴڿ� ���� ���� ���尡��(���� ������ �δ����� ������� �ʵ���)
            if (moles[indexs[currentIndex]].MoleState == MoleState.UnderGround)
            {
                //moles[indexs[currentIndex]].transform.position = holes[indexs2[currentIndex]].transform.position;
                moles[indexs[currentIndex]].transform.position = new Vector3(holes[indexs2[currentIndex]].transform.position.x,
                                                               -0.5f, holes[indexs2[currentIndex]].transform.position.z);
                //���õ� �δ����� ���¸� MoveUp���� ����
                moles[indexs[currentIndex]].ChangeState(MoleState.MoveUp);

                //������ �δ��� ���� 1 ����
                currentSpawnMole++;

                yield return new WaitForSeconds(0.1f);
            }

            //�ִ� ���� ���ڸ�ŭ ���������� SpawnMultipleMoles() �ڷ�ƾ �Լ� ����
            if(currentSpawnMole == MaxSpawnMole)
            {
                break;
            }

            currentIndex++;

            yield return null;
        }
    }

    int[] RandomNumerics(int maxCount, int n)
    {
        //0~maxCount������ ���� �� ��ġ�� �ʴ� n���� ������ �ʿ��� �� ���
        int[] defaults = new int[maxCount];     //0~maxCount���� ������� �����ϴ� �迭
        int[] results = new int[n];             //��� ������ �����ϴ� �迭

        //�迭 ��ü�� 0���� maxCount�� ���� ������� ����
        for(int i = 0; i < maxCount; ++ i)
        {
            defaults[i] = i;
        }

        for (int i = 0; i < n; ++i)
        {
            int index = Random.Range(0, maxCount);  //������ ���ڸ� �ϳ� �̾Ƽ�

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
        
    }




}
