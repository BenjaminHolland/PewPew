using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    new private Renderer renderer;
    private Rigidbody2D body;
    new private Collider2D collider;
    private Interactions interactions;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        interactions = GameObject.Find("Interactions").GetComponent<Interactions>();
    }
    private void OnCollisionEnter2D(Collision2D collision2D){
        if (collision2D.gameObject.CompareTag("unit"))
        {

            var unit = collision2D.gameObject.GetComponent<IUnit>();
            foreach(var obj in interactions.GetEffects(this.GetType(),unit.GetType())){
                unit.HitWith(obj);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.CompareTag("unit"))
        {
            var unit = collider2D.gameObject.GetComponent<IUnit>();
            foreach (var obj in interactions.GetEffects(this.GetType(), unit.GetType()))
            {
                unit.HitWith(obj);
            }
   
            Destroy(gameObject);
        }

    }

    void FixedUpdate(){
     
    }
    // Update is called once per frame
    void Update()
    {
        body.MovePosition(transform.position+new Vector3(0, 0.01f, 0));
        // Resolve boundary clamping
        var boundarySize = renderer.bounds.size;
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - boundarySize.y / 2f;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + boundarySize.y / 2f;
        var leftBorder = -1 + boundarySize.x / 2f;
        var rightBorder = 1 - boundarySize.x / 2f;

        BoundaryUtils
            .ForBoundary(new Rect(leftBorder, bottomBorder, rightBorder - leftBorder, topBorder - bottomBorder))
            .OnOutOfBoundary(() =>
            {
                Destroy(gameObject);
            })
            .Build()
            .Handle(transform.position);

    }
}
