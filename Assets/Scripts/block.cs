using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class block : MonoBehaviour
{
    
    public GameObject score;
    
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        bool OnOff = (Random.value > 0.5f); // ON / OFF 
        gameObject.SetActive(OnOff);
        GameObject conv = transform.GetChild(0).gameObject;
        score = conv.transform.GetChild(0).gameObject;
        Debug.Log(score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
