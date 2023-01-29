using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    new private Renderer renderer;
    private Rigidbody2D body;
    new private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision2D){
        if(collision2D.gameObject.CompareTag("asteroid")){
         var controller = collision2D.gameObject.GetComponent<AsteroidController>();
         controller.TakeHit();
         Destroy(gameObject);
        }
        // Tags don't seem to be the best mechanism for this. You can only have one tag per oject. It's probably better to make some kind of "damageable" component, or at least
        // a shared component type? Not sure how to structure this.
        if(collision2D.gameObject.CompareTag("crab")){
            var controller = collision2D.gameObject.GetComponent<CrabController>();
            controller.TakeHit();
            Destroy(gameObject);
        }
        if (collision2D.gameObject.CompareTag("damageable"))
        {
            var controller = collision2D.gameObject.GetComponent<BananaController>();
            controller.TakeHit();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider2D){
      
        if(collider2D.CompareTag("crab")){
            var controller = collider2D.gameObject.GetComponent<CrabController>();
            controller.TakeHit();
            Destroy(gameObject);
        }
        if (collider2D.gameObject.CompareTag("damageable"))
        {
            var controller = collider2D.gameObject.GetComponent<BananaController>();
            controller.TakeHit();
            Destroy(gameObject);
        }

    }

    void FixedUpdate(){
     
    }
    // Update is called once per frame
    void Update()
    {
        body.MovePosition(transform.position+new Vector3(0, 0.1f, 0));
        // Resolve boundary clamping
        var boundarySize = renderer.bounds.size;
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - boundarySize.y / 2f;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + boundarySize.y / 2f;
        var leftBorder = -4 + boundarySize.x / 2f;
        var rightBorder = 4 - boundarySize.x / 2f;

        if (transform.position.x < leftBorder)
        {

            Destroy(gameObject);
        }
        if (transform.position.x > rightBorder)
        {


            Destroy(gameObject);
        }
        if (transform.position.y > topBorder)
        {


            Destroy(gameObject);
        }
        if (transform.position.y < bottomBorder)
        {

            Destroy(gameObject);
        }

    }
}
