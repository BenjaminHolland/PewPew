using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float BackgroundSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {  

        transform.position += new Vector3(0, -BackgroundSpeed, 0f);
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        var backgroundSize = GetComponent<Renderer>().bounds.size;
        if (transform.position.y + (backgroundSize.y / 2f) < bottomBorder)
        {
            transform.position = new Vector3(transform.position.x, 16f, transform.position.z);
        }

    }
}
