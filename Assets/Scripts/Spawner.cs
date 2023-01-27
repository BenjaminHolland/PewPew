using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
public class Spawner : MonoBehaviour
{
    private VisualEffect booms;
    private GameObject shu;
    private GameObject roid;
    private GameObject crab;
    public void MakeBigBoom(Vector3 location)
    {
    }
    void Awake()
    {

        booms = GetComponent<VisualEffect>();
        shu = Resources.Load<GameObject>("Shu");
        roid = Resources.Load<GameObject>("Asteroid");
        crab = Resources.Load<GameObject>("Crab");
        Instantiate<GameObject>(shu, Vector3.zero, Quaternion.identity);
        SpawnStaticCircle();
        SpawnCrabLine();

    }
    IEnumerator SpawnCrabLineCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate<GameObject>(crab, new Vector3(0f, 3f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void SpawnCrabLine()
    {
        StartCoroutine(SpawnCrabLineCoroutine());
    }
    void SpawnStaticCircle()
    {
        for (int i = 0; i < 10; i++)
        {
            var x = 2 * Mathf.Sin((i * Mathf.PI * 2) / 10f);
            var y = 2 * Mathf.Cos((i * Mathf.PI * 2) / 10f);
            var r = Instantiate<GameObject>(roid, new Vector3(x, y, 0), Quaternion.identity);
            var rBody = r.GetComponent<Rigidbody2D>();
            rBody.mass = (float)Random.Range(10f, 100f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        var roids = GameObject.FindGameObjectsWithTag("asteroid");
        if (roids.Length == 0)
        {
            SpawnStaticCircle();
            SpawnCrabLine();
        }

    }
}
