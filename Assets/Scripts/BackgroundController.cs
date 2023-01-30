using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private bool triggeredSpawn = false;
    private Spawner spawner;
    public float BackgroundSpeed = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }
    
    // Update is called once per frame
    void Update()
    {  

        transform.position += new Vector3(0, -BackgroundSpeed, 0f);
        var distance = (transform.position - Camera.main.transform.position).z;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        var backgroundSize = GetComponent<Renderer>().bounds.size;
        if(transform.position.y-backgroundSize.y<bottomBorder&&!triggeredSpawn){
            triggeredSpawn = true;
            spawner.SpawnBackgroundPatch();
        }
        if (transform.position.y < bottomBorder)
        {
            Destroy(gameObject);
        }

    }
}
