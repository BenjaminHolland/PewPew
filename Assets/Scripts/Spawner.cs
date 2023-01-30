using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
public class Spawner : MonoBehaviour
{
    private VisualEffect booms;
    private GameObject shu;
    private GameObject roid;
    private GameObject crab;
    private GameObject bana;
    private GameObject backgroundPatch;
    public void MakeBigBoom(Vector3 location)
    {
    }
    void Awake()
    {
        backgroundPatch = Resources.Load<GameObject>("Background");
        booms = GetComponent<VisualEffect>();
        shu = Resources.Load<GameObject>("Shu");
        roid = Resources.Load<GameObject>("Asteroid");
        crab = Resources.Load<GameObject>("Crab");
        bana = Resources.Load<GameObject>("Banana");
        Instantiate<GameObject>(shu, Vector3.zero, Quaternion.identity);
       // SpawnStaticCircle();
        //SpawnBananaLine();
        SpawnCrabLine();
    }
    public Rect ViewportWorldRect(){
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance)).x;
    
        return Rect.MinMaxRect(bottomBorder,topBorder,leftBorder,rightBorder);
    }
    void OnGUI(){
        GUI.Box(new Rect(0,0,800,100),ViewportWorldRect().ToString());
    }
    public void SpawnBackgroundPatch(){
        // I don't think we should have to do this with an orthographic camera, since distance in an orthographic projection
        // doesn't effect size.
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;
        // 0.1 is the background speed. The idea is to spawn it one frame of motion ahead. Otherwise there's a gap.
        Instantiate(backgroundPatch,new Vector3(0,topBorder+backgroundPatch.GetComponent<Renderer>().bounds.size.y-0.005f,0f),Quaternion.identity);
    }
    IEnumerator SpawnCrabLineCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate<GameObject>(crab, new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void SpawnCrabLine()
    {
        StartCoroutine(SpawnCrabLineCoroutine());
    }
    IEnumerator SpawnBananaLineAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate<GameObject>(bana, new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
    void SpawnBananaLine()
    {
        StartCoroutine(SpawnBananaLineAsync());
    }
    void SpawnStaticCircle()
    {
        for (int i = 0; i < 10; i++)
        {
            var x = 0.3f * Mathf.Sin((i * Mathf.PI * 2) / 10f);
            var y = 0.3f * Mathf.Cos((i * Mathf.PI * 2) / 10f);
            var r = Instantiate<GameObject>(roid, new Vector3(x, y, 0), Quaternion.identity);
            var rBody = r.GetComponent<Rigidbody2D>();
            rBody.mass = (float)Random.Range(10f, 100f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("unit").Count(obj => obj.GetComponent<AsteroidController>() != null)==0)
        {
            SpawnStaticCircle();
            SpawnCrabLine();
            SpawnBananaLine();
        }

    }
}
