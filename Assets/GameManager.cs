using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GameMgr;
    public float GroundY = -56.3f;
    public GameObject P_Ball, P_ItemBall, P_Block;
    public GameObject BallPreview, GameOverPanel;
    public Transform BlockGroup, BallGroup;
    public Text BestScoreText, ScoreText, FinalText, NewRecordText;
    public Color[] blockColor;
    public bool shotTrigger, shotable;
    public Vector3 veryfirstPos, addPos;

    public Vector3 firstPos, secondPos, gap;
    int score, timerCount, launchIndex=0;
    bool timerStart, isDie, isNewRecord, isBlockMoving;
    public bool isShot = true;
    float timeDelay;

    private void Awake()
    {
        // 9:16 고정해상도 카메라
        Camera camera = Camera.main;
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;


        // 시작
        BlockGenerator();
        BestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("BestScore").ToString();
    }



    private void FixedUpdate()
    {
        if (timerStart && ++timerCount == 3)
        {
            timerCount = 0;
            BallGroup.GetChild(launchIndex++).GetComponent<Ball>().BallStart(gap);
            Debug.Log("aaa");
            if (launchIndex == BallGroup.childCount)
            {
                timerStart = false;
                launchIndex = 0;
            }
        }
    }

    void Start()
    {
        GameMgr = this;
    }

    void Update()
    {
        if (isShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
                Debug.Log("firstPos" + firstPos);
            }

              shotable = true;
              for (int i=0; i< BallGroup.childCount; i++)
              {
                  if (BallGroup.GetChild(i).GetComponent<Ball>().ismoving)
                  {
                      shotable = false;
                  }
              }
              if (!shotable) return;

              if (shotTrigger && shotable)
              {
                  shotTrigger = false;
                  BlockGenerator();
                  timeDelay = 0;
              }
            

            

            if (Input.GetMouseButton(0))
            {
                Vector3 thisPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
                gap = (thisPos - firstPos);
                Ball.ball.GridLine(gap);
            }

            if (Input.GetMouseButtonUp(0))
            {
                secondPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
                if ((secondPos - firstPos).magnitude < 0.1) return;
                gap = (secondPos - firstPos).normalized;
                //Ball.ball.BallStart(gap);
                isShot = false;
                timerStart = true;
                veryfirstPos = Vector3.zero;
                firstPos = Vector3.zero;
                Ball.ball.line.positionCount = 0;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void VeryFisrtPosSet(Vector3 pos)
    {
        if (veryfirstPos == Vector3.zero)
        {
            veryfirstPos = pos;
        }
    }

    void BlockGenerator()
    {
        // 점수 
        ScoreText.text = "현재점수 : " + (++score).ToString();
        //BestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("BestScore").ToString();
        if (PlayerPrefs.GetInt("BestScore", 0) < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
            BestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("BestScore").ToString();
            print(PlayerPrefs.GetInt("BestScore").ToString());
            isNewRecord = true;
        }


        // 점수에 따른 블럭복사개수 정하기
        int count;
        int randBlock = Random.Range(0, 24);
        if (score <= 10) count = randBlock < 16 ? 1 : 2;
        else if (score <= 20) count = randBlock < 8 ? 1 : (randBlock < 16 ? 2 : 3);
        else if (score <= 40) count = randBlock < 9 ? 2 : (randBlock < 18 ? 3 : 4);
        else count = randBlock < 8 ? 2 : (randBlock < 16 ? 3 : (randBlock < 20 ? 4 : 5));


        // 스폰좌표에 블럭, 초록구 생성
        List<Vector3> SpawnList = new List<Vector3>();
        for (int i = 0; i < 6; i++) SpawnList.Add(new Vector3(-46.7f + i * 18.68f, 51.2f, 0));

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, SpawnList.Count);

            Transform TR = Instantiate(P_Block, SpawnList[rand], Quaternion.identity).transform;
            TR.SetParent(BlockGroup);
            TR.GetChild(0).GetComponentInChildren<Text>().text = score.ToString();

            SpawnList.RemoveAt(rand);
        }
        Instantiate(P_ItemBall, SpawnList[Random.Range(0, SpawnList.Count)], Quaternion.identity).transform.SetParent(BlockGroup);


        // 블럭 내리기
        isBlockMoving = true;
        for (int i = 0; i < BlockGroup.childCount; i++) StartCoroutine(BlockMoveDown(BlockGroup.GetChild(i)));
    }

    IEnumerator BlockMoveDown(Transform TR)
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 targetPos = TR.position + new Vector3(0, -12.8f, 0);

        // 막줄이면 게임오버 트리거, 콜라이더 비활성화
        if (targetPos.y < -50)
        {
            if (TR.CompareTag("Block")) isDie = true;
            for (int i = 0; i < BallGroup.childCount; i++)
                BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = false;
        }
        // 0.3초간 블럭 이동
        float TT = 1.5f;
        while (true)
        {
            yield return null; TT -= Time.deltaTime * 1.5f;
            TR.position = Vector3.MoveTowards(TR.position, targetPos + new Vector3(0, -6, 0), TT);
            if (TR.position == targetPos + new Vector3(0, -6, 0)) break;
        }
        TT = 0.9f;
        while (true)
        {
            yield return null; TT -= Time.deltaTime;
            TR.position = Vector3.MoveTowards(TR.position, targetPos, TT);
            if (TR.position == targetPos) break;
        }
        isBlockMoving = false;


        // 이동되고 난 후 막줄이면 블럭이면 게임오버, 초록구면 파괴
        if (targetPos.y < -50)
        {
            if (TR.CompareTag("Block"))
            {
                for (int i = 0; i < BallGroup.childCount; i++)
                    Destroy(BallGroup.GetChild(i).gameObject);


                BestScoreText.gameObject.SetActive(false);
                ScoreText.gameObject.SetActive(false);

                GameOverPanel.SetActive(true);
                enabled = false;
                FinalText.text = "최종점수 : " + score.ToString();
                if (isNewRecord)
                {
                    NewRecordText.gameObject.SetActive(true);
                }
            }

            else
            {
                Destroy(TR.gameObject);

                for (int i=0; i<BallGroup.childCount; i++)
                {
                    BallGroup.GetChild(i).GetComponent<CircleCollider2D>().enabled = true;
                }
            }
        }
    }
}