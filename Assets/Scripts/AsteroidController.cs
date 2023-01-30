using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour,IUnit
{
    public int Health;
    new private Renderer renderer;
    private Rigidbody2D body;
    new private Collider2D collider;
    private Animator animator;

    int IUnit.Health => Health;
    bool IUnit.Woogly => false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        Health = 2;
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        // Resolve boundary clamping
        var boundarySize = renderer.bounds.size;
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - boundarySize.y / 2f;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + boundarySize.y / 2f;
        var leftBorder = -4 + boundarySize.x / 2f;
        var rightBorder = 4 - boundarySize.x / 2f;

        BoundaryUtils
            .ForBoundary(new Rect(leftBorder, bottomBorder, rightBorder - leftBorder, topBorder - bottomBorder))
            .OnAboveXMax(() =>
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                body.position = new Vector2(rightBorder, transform.position.y);
            })
            .OnBelowXMin(() =>
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
                body.position = new Vector2(leftBorder, transform.position.y);
            })
            .OnAboveYMax(() =>
            {
                body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                body.position = new Vector2(transform.position.x, topBorder);
            })
            .OnBelowYMin(() =>
            {
                body.velocity = new Vector2(body.velocity.x, -body.velocity.y);
                body.position = new Vector2(transform.position.x, bottomBorder);
            })
            .Build()
            .Handle(transform.position);
    }

    public void HitWith(object other)
    {
        if (other is DamageEffect damageEffect)
        {
            // Can I cache this?
            var score = GameObject.Find("Score").GetComponent<ScoreController>();

            Health -= damageEffect.NormalDamage;
            if (Health <= 0)
            {
                collider.enabled = false;
                body.velocity = Vector2.zero;
                //TODO better saturate.
                if (body.angularVelocity > 0)
                {
                    body.angularVelocity = 10;
                }
                else
                {
                    body.angularVelocity = -10;
                }
                GameObject.FindGameObjectWithTag("spawner").GetComponent<Spawner>().MakeBigBoom(body.position);
                animator.SetTrigger("Explode");
                score.ModifyScoreBy(100);
            }
        }
    }
}
