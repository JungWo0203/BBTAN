using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static int blockcount = 1;
    public Rigidbody2D ballRigidbody = null;
    bool isBallPlay = false;
    public static Ball BallMgr;
    public float power = 7f;
    public Vector3 addPos;

    private void Awake()
    {
        BallMgr = this;
        ballRigidbody = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        Debug.Log("공 속도 :"+ballRigidbody.velocity);
    } 

    public void ballstart()
    {
        addPos = GameManager.gameMgr.gap * power;
        ballRigidbody.velocity = addPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Col");
        if (collision.gameObject.tag == "Ground")
        {
            ballRigidbody.velocity = new Vector3(0, 0, 0);
            blockcount++;

        }
    }
}