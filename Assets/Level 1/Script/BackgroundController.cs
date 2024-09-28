using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallexEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float distance = cam.transform.position.x * parallexEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    
    }
}
