using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private float            maxY;                 // ��ġ�� �ִ� y��ġ
    [SerializeField]
    private float            minY;                 // ��ġ�� �ּ� y��ġ
    [SerializeField]
    private GameObject       moleHitEffectPrefab;  // �δ��� Ÿ�� ȿ�� ������
    [SerializeField]
    private AudioClip[]      audioClips;           // �δ����� Ÿ������ �� ����Ǵ� ����
    [SerializeField]
    private MoleHitTextViewer moleHitTextViewer; // Ÿ���� �δ��� ���� Ÿ�� ���� �ؽ�Ʈ ���
    [SerializeField]
    private GameController   gameController;       // ���� ����
    [SerializeField]
    private ObjectDetector   objectDetector;       // ���콺 Ŭ������ ������Ʈ ����
    private Movement         movement;             // ��ġ ������Ʈ �̵�
    private AudioSource      audioSource;          // �δ����� Ÿ������ �� �Ҹ��� ����ϴ� AudioSource

    private void Awake()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();

        objectDetector.raycastEvent.AddListener(Onhit);
    }

    void Onhit(Transform target)
    {
        if (target.CompareTag("Mole"))
        {
            MoleFSM mole = target.GetComponent<MoleFSM>();

            // �δ����� Ȧ �ȿ� �������� ���� �Ұ�
            if (mole.MoleState == MoleState.UnderGround) return;

            // ��ġ�� ��ġ ����
            transform.position = new Vector3(target.position.x + 0.5f, minY, target.position.z);

            // ��ġ�� �¾ұ� ������ �δ����� ���¸� �ٷ� UnderGround�� ����
            mole.ChangeState(MoleState.UnderGround);

            // ī�޶� ����
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            // �δ��� Ÿ�� ȿ�� ����
            GameObject clone = Instantiate(moleHitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;

            //�δ��� ������ ���� ó��(����, ����, ���� ����� �ٸ�)
            if (mole.MoleType == 0) // �Ϲ�
            {
                gameController.NormalMoleHitCount ++;
                gameController.Combo ++;
                main.startColor = Color.yellow;

                //10�޺��� 0.5�� ���Ѵ�
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 50);

                gameController.Score += getScore;
                moleHitTextViewer.OnHit("Score +"+getScore, Color.white);

            }
            else if(mole.MoleType == 1)  //������(���ʽ�)
            {
                gameController.DogMoleHitCount++;
                gameController.Combo ++;
                main.startColor = Color.blue;

                //10�޺��� 0.5�� ���Ѵ�
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 150);

                gameController.Score += getScore;
                moleHitTextViewer.OnHit("Score +" + getScore, Color.blue);
            }
            else if (mole.MoleType == 2)  //���� (����)
            {
                gameController.RedMoleHitCount++;
                gameController.Combo = 0;
                main.startColor = Color.red;

                //10�޺��� 0.5�� ���Ѵ� ��� �� �� ������ �ȵǴ°���
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 300);

                gameController.Score -= getScore;
                moleHitTextViewer.OnHit("Score -" + getScore, Color.red);
            }

            // ���� ���()
            PlaySound((int)mole.MoleType);

            // ��ġ�� ���� �̵���Ű�� �ڷ�ƾ ���
            StartCoroutine("MoveUp");
        }
    }

    IEnumerator MoveUp()
    {
        //�̵����� (0,1,0) [��]
        movement.MoveTo(Vector3.up);

        while (true)
        {
            if(transform.position.y >= maxY)
            {
                movement.MoveTo(Vector3.zero);

                break;
            }

            yield return null;
        }
    }

    void PlaySound(int index)
    {
        audioSource.Stop(); //������ ����� ���� ����
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

}
