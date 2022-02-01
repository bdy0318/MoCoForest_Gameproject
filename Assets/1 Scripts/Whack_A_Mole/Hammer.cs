using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private float            maxY;                 // 망치의 최대 y위치
    [SerializeField]
    private float            minY;                 // 망치의 최소 y위치
    [SerializeField]
    private GameObject       moleHitEffectPrefab;  // 두더지 타격 효과 프리팹
    [SerializeField]
    private AudioClip[]      audioClips;           // 두더지를 타격했을 때 재생되는 사운드
    [SerializeField]
    private MoleHitTextViewer moleHitTextViewer; // 타격한 두더지 위에 타격 정보 텍스트 출력
    [SerializeField]
    private GameController   gameController;       // 점수 증가
    [SerializeField]
    private ObjectDetector   objectDetector;       // 마우스 클릭으로 오브젝트 선택
    private Movement         movement;             // 망치 오브젝트 이동
    private AudioSource      audioSource;          // 두더지를 타격했을 때 소리를 재생하는 AudioSource

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

            // 두더지가 홀 안에 있을때는 공격 불가
            if (mole.MoleState == MoleState.UnderGround) return;

            // 망치의 위치 설정
            transform.position = new Vector3(target.position.x + 0.5f, minY, target.position.z);

            // 망치에 맞았기 때문에 두더지의 상태를 바로 UnderGround로 설정
            mole.ChangeState(MoleState.UnderGround);

            // 카메라 흔들기
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            // 두더지 타격 효과 생성
            GameObject clone = Instantiate(moleHitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;

            //두더지 종류에 따라 처리(색상, 점수, 사운드 재생이 다름)
            if (mole.MoleType == 0) // 일반
            {
                gameController.NormalMoleHitCount ++;
                gameController.Combo ++;
                main.startColor = Color.yellow;

                //10콤보당 0.5씩 더한다
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 50);

                gameController.Score += getScore;
                moleHitTextViewer.OnHit("Score +"+getScore, Color.white);

            }
            else if(mole.MoleType == 1)  //강아지(보너스)
            {
                gameController.DogMoleHitCount++;
                gameController.Combo ++;
                main.startColor = Color.blue;

                //10콤보당 0.5씩 더한다
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 150);

                gameController.Score += getScore;
                moleHitTextViewer.OnHit("Score +" + getScore, Color.blue);
            }
            else if (mole.MoleType == 2)  //레드 (감점)
            {
                gameController.RedMoleHitCount++;
                gameController.Combo = 0;
                main.startColor = Color.red;

                //10콤보당 0.5씩 더한다 얘는 왜 더 감점이 안되는거지
                float scoreMultiple = 1 + gameController.Combo / 10 * 0.5f;
                int getScore = (int)(scoreMultiple * 300);

                gameController.Score -= getScore;
                moleHitTextViewer.OnHit("Score -" + getScore, Color.red);
            }

            // 사운드 재생()
            PlaySound((int)mole.MoleType);

            // 망치를 위로 이동시키는 코루틴 재생
            StartCoroutine("MoveUp");
        }
    }

    IEnumerator MoveUp()
    {
        //이동방향 (0,1,0) [위]
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
        audioSource.Stop(); //기존에 재생된 음악 멈춤
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

}
