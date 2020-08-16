using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingRayCast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
    }
}
