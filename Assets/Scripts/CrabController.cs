using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    new private Renderer renderer;
    private int Health = 10;
    // Woogly is a state where the enemy has lost control of their vehicle. Concretely, they are now a dynamic physics object that can be manipulated by the player.
    // This could be a core game mechanic if we expand it: Shoot enemies untily they go woogly, then use some guns with physics properties (attraction, repulsion)
    // to whack enemies into eachother. Maybe add in some "elemental" aspects: "This enemy must be damanged by woogly enemies with some specific type". 
    private bool Woogly = false;
    private Rigidbody2D body;
    new private Collider2D collider;
    private float startTime;
    private Vector2 lastVelocity;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<Renderer>();
        body.isKinematic = true;
        collider.isTrigger = true;

        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        startTime = Time.time;
    }
    // Can/Should probably be extracted into an interface. Should also probably take an argument depending on the type of hit to support different
    // responses to different bullets. 
    public void TakeHit()
    {
        // Mentioned somewhere else, but this should probably be cached.
        var score = GameObject.Find("Score").GetComponent<ScoreController>();
        Health -= 1;
        if (Health < 5 && !Woogly) // Is Woogled!
        {
            Woogly = true;
            body.isKinematic = false;
            body.mass = 5;
            collider.isTrigger = false;
            body.constraints = RigidbodyConstraints2D.None;

            // Inherit ship's pre-woogly velocity and add random woogly torque
            body.velocity += -lastVelocity;
            float woogleTorque = UnityEngine.Random.Range(-10.0f,10.0f);
            body.AddTorque(woogleTorque);

            score.ModifyScoreBy(10);
        }
        if (Health <= 0) // Is Dead!
        {
            score.ModifyScoreBy(300);
            // do animation stuff instead;
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        var localTime = Time.time-startTime;
        if (!Woogly)
        {
            // Try to fly in a circle. The "forces" involved here are not quite right. The new position should be computed as the derivitive of a circle scaled by dT,
            // but we are being lazy here, so we're going to normal circle bits without time scaling, using the object-local time as a source instead of dT.
            var downAtSpeed = body.position + new Vector2(0, -0.02f);
            var circle = new Vector2(0.1f * Mathf.Cos(localTime * 4f), 0.1f * Mathf.Sin(localTime * 4f));
            var newPos = downAtSpeed + circle;
            body.MovePosition(newPos);
            
            // Cache the last velocity we measured for when the enemy goes woogly.
            lastVelocity = (body.position - newPos)/Time.fixedDeltaTime;
            
            // Figure out what direction we're flying in, and face that direction. Atan2 returns radians, body.rotation is in degrees, and everything is rotated 90 off
            // where we expect it to be. 
            body.rotation = (1+Mathf.Atan2(lastVelocity.y,lastVelocity.x)/Mathf.PI)/2*360f+90;
            
        }
        var boundarySize = renderer.bounds.size;
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1.1f, distance)).y - boundarySize.y / 2f;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, -0.1f, distance)).y + boundarySize.y / 2f;
        var topBorderSafe = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - boundarySize.y / 2f;
        var bottomBorderSafe = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + boundarySize.y / 2f;
        var leftBorder = -4 + boundarySize.x / 2f;
        var rightBorder = 4 - boundarySize.x / 2f;

        // What we do when an enemy leaves the screen depends on whether they're woogly or not. If they aren't, we probably want them to either be destroyed
        // or respawn at the top of the screen. If they are, we probably want them to bounce around pleasingly.
        void resetToTopOr(Action onWoogly)
        {
            if (!Woogly)
            {
                transform.position = new Vector3(transform.position.x, topBorderSafe, 0);
            }
            else
            {
                onWoogly();
            }
        }
        BoundaryUtils
           .ForBoundary(Rect.MinMaxRect(leftBorder, bottomBorder, rightBorder, topBorder))
           .OnBelowXMin(() =>
           {
               resetToTopOr(() =>
               {
                   body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                   body.position = new Vector2(leftBorder, transform.position.y);
               });
           })
           .OnAboveXMax(() =>
           {
               resetToTopOr(() =>
               {
                   body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                   body.position = new Vector2(rightBorder, transform.position.y);
               });
           })
           .OnAboveYMax(() =>
           {
               resetToTopOr(() =>
               {
                   body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                   body.position = new Vector2(transform.position.x, topBorder);
               });
           })
           .OnBelowYMin(() =>
           {
               resetToTopOr(() =>
               {
                   body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                   body.position = new Vector2(transform.position.x, bottomBorder);
               });
           })
           .Build()
           .Handle(transform.position);

    }
}
