using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaController : MonoBehaviour
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
    public AnimationCurve Acceleration; 
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
        if (Health < 5)
        {
            Woogly = true;
            body.isKinematic = false;
            collider.isTrigger = false;
             body.constraints = RigidbodyConstraints2D.None;
             body.velocity = lastVelocity;
            score.ModifyScoreBy(10);
        }
        if (Health <= 0)
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
            var localTimeSpec = 0f;
            if (localTime > 0.5f)
            {
                localTimeSpec = 1f;
            }
            else
            {
                localTimeSpec = localTime * 2f;
            }
            var downSpeed = Acceleration.Evaluate(localTimeSpec);
            var downAtSpeed = body.position + new Vector2(0, downSpeed*-0.05f);
            //var circle = new Vector2(0.1f * Mathf.Cos(localTime * 4f), 0.1f * Mathf.Sin(localTime * 4f));
            var newPos = downAtSpeed;
            var diff = body.position - newPos;
            lastVelocity = diff;
            body.rotation = (1+Mathf.Atan2(diff.y,diff.x)/Mathf.PI)/2*360f+90;
            body.MovePosition(newPos);
            
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
        void onOutOfBounds(Action onWoogly)
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
        if (transform.position.x < leftBorder)
        {
            onOutOfBounds(() =>
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                body.position = new Vector2(leftBorder, transform.position.y);
            });
        }
        if (transform.position.x > rightBorder)
        {


            onOutOfBounds(() =>
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                body.position = new Vector2(rightBorder, transform.position.y);
            });
        }
        if (transform.position.y > topBorder)
        {
            onOutOfBounds(() =>
            {
                body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                body.position = new Vector2(transform.position.x, topBorder);
            });
        }
        if (transform.position.y < bottomBorder)
        {


            onOutOfBounds(() =>
            {
                body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                body.position = new Vector2(transform.position.x, bottomBorder);
            });
        }

    }
}
