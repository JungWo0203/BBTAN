using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    Vector3 addPos;
    float power = 9000f;
    public LineRenderer line;
    Rigidbody2D Rig;
    public static Ball ball;

    public bool ismoving;
    public int itemballcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ball = this;
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OnCollisionEnter2D_Ball(Collision2D collision)
    {
        GameObject Col = collision.gameObject;
        Physics2D.IgnoreLayerCollision(2, 2);

        Vector2 pos = Rig.velocity.normalized;
        if (pos.magnitude != 0 && pos.y < 0.15f && pos.y > -0.15f)
        {
            Rig.velocity = Vector2.zero;
            Rig.AddForce(new Vector2(pos.x > 0 ? 1 : -1, -0.2f).normalized * power);
        }

        if (Col.gameObject.tag == "Ground")
        {
            Rig.velocity = Vector2.zero;
            transform.position = new Vector2(collision.contacts[0].point.x, GameManager.GameMgr.GroundY);
            GameManager.GameMgr.VeryFisrtPosSet(transform.position);
            GameManager.GameMgr.isShot = true;
            if (itemballcount > 0)
                for (int i=0; i<itemballcount; i++)
                {

                    Instantiate(GameManager.GameMgr.P_Ball, GameManager.GameMgr.veryfirstPos, Quaternion.identity).transform.SetParent(GameManager.GameMgr.BallGroup); 
                }
                itemballcount = 0;
            while (true)
            {
                yield return null;
                transform.position = Vector3.MoveTowards(transform.position, GameManager.GameMgr.veryfirstPos, 4);
                if (transform.position == GameManager.GameMgr.veryfirstPos)
                {
                    ismoving = false;
                    yield break;
                }
            }
        }

        if (Col.gameObject.tag == "Block")
        {
            Text BlockText = collision.transform.GetChild(0).GetComponentInChildren<Text>();
            int blockValue = int.Parse(BlockText.text) - 1;


            if (blockValue > 0)
            {
                BlockText.text = blockValue.ToString();
                Col.GetComponent<Animator>().SetTrigger("Shock");
            }
            else
            {
                Destroy(Col.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("OnCollisionEnter2D_Ball",collision);
    }


    public void GridLine(Vector3 vec)
    {
        print(transform.position - vec.normalized * 2);

        RaycastHit2D hit = Physics2D.Raycast(transform.position - vec.normalized * 2, -vec, 10);
        Debug.DrawRay(transform.position - vec.normalized * 2, -vec, Color.red, 0.3f);
        vec = (transform.position - (vec * 10));
        print(hit.transform.name);
        if (hit.collider.CompareTag("Wall"))
        {
            print("Wall!!");
            Vector3 vec2 = vec;
            vec = hit.point;
            line.positionCount = 3;
            print("1 " + vec2.x + " " + vec.x);
            float x = vec.x - vec2.x;
            vec2.x = vec.x + x;

            line.SetPosition(2, vec2);
            Debug.Log(vec);
        }
        else
        {
            line.positionCount = 2;
        }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, vec);
    }

    public void BallStart(Vector3 gap)
    {
        GameManager.GameMgr.shotTrigger = true;
        ismoving = true;
         addPos = -(gap * power);
        Rig.AddForce(addPos);
        print(gameObject.name + " : " + addPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ItemBall")
        {
            Destroy(collision.gameObject);
            itemballcount++;
            Debug.Log(collision.gameObject + "\n" + itemballcount);
        }
    }
}
