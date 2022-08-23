using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMgr = null;
    public GameObject ball;
    public GameObject block;
    public Vector3 firstPos, secondPos, gap;

    public GameObject Blockgroup = null;
    
    // Start is called before the first frame update
    void Start()
    {
        gameMgr = this;

        Instantiate(Blockgroup);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            Debug.Log("firstPos" + firstPos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            Debug.Log("secondPos :"+ secondPos);
            if ((secondPos - firstPos).magnitude < 0.1) return;
            gap = (secondPos = firstPos).normalized;
            Debug.Log("gap:"+ gap);
            Cycle();
        }
    }



    void Cycle ()
    {
        // 발사
        Ball.BallMgr.ballstart();

        Instantiate(Blockgroup);
        // 블럭 생성 : 인스턴스를 하면서 y값을 줄여가며 내려가게하기

        // 블럭 더 쌔지게 하기 : 하나의 변수를 생성한뒤 드래그 & 드롭하면 1씩 더해지게 해준뒤 새롭게 블럭이 생성되면 이 변수를 이용하여 내구도와 안에 피를 채운다.

        // 아이템 [공 더해는 거] : 
    }
}