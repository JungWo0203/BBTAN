using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public GameObject itemPrefab;
    public int count;
    public int itemCount;
    void Start()
    {
        count = 0;
    }

    void Update()
    {
        if(count >= 5 && itemCount == 0)
        {
            foreach(Transform children in transform) 
            {
                if (!children.gameObject.activeInHierarchy)
                {
                    SpawnItem(children.position);
                    return;
                }
            }
        }
    }

    public void SpawnItem(Vector3 vec)
    {
        GameObject go = Instantiate(itemPrefab, vec, Quaternion.identity);
        go.transform.parent = transform;
        enabled = false;
    }
}
