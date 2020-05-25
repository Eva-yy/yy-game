using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParachute : MonoBehaviour
{
    public string namedParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.Find(namedParent).gameObject != null)
            Destroy(transform.Find(namedParent).gameObject);
    }
}
