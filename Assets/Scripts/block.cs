using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class block : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject score;

    public TextMeshPro scoreText;

    BlockGroup blockGroup;
    // Start is called before the first frame update
    void Start()
    {
        blockGroup = transform.parent.GetComponent<BlockGroup>();

        bool OnOff = (Random.value > 0.5f); // ON / OFF 
        gameObject.SetActive(OnOff);

        if (!OnOff && Random.value > 0.5f && blockGroup.enabled)
        {
            blockGroup.SpawnItem(transform.position);
            blockGroup.itemCount++;
        }

        blockGroup.count++;

        scoreText.text = Ball.blockcount.ToString();
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
