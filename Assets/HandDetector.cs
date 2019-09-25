using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        transform.parent.GetComponent<OHH>().Collided(collider);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        transform.parent.GetComponent<OHH>().UnCollided(collider);
    }
}
