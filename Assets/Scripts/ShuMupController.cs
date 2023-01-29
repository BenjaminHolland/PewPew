using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShuMupController : MonoBehaviour
{
    public GameObject bullet;
    public float HorizontalSpeed = 0.1f;
    public float VerticalSpeed = 0.05f;
    private float HorizontalImpulse = 0f;
    private float VerticalImpulse = 0f;
    private bool firing = false;
    private Animator animator;
    new private Renderer renderer;
    private float nextBullet = 0f;
    private PointEffector2D attractor;
    // We really should keep the charged state somewhere other than the main GUI.
    private MainGui mainGui;
    private Vector3 HorizontalVelocity
    {
        get { return new Vector3(HorizontalImpulse * HorizontalSpeed, 0f, 0f); }
    }
    private Vector3 VerticalVelocity
    {
        get { return new Vector3(0, VerticalImpulse * VerticalSpeed, 0f); }
    }

    // Start is called before the first frame update
    void Start()
    {
        nextBullet = 0;
        mainGui = GameObject.Find("Canvas").GetComponent<MainGui>();
        attractor = GameObject.Find("Attractor").GetComponent<PointEffector2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }
    IEnumerator FireAttractor()
    {
        if (!attractor.enabled)
        {
            attractor.enabled = true;
            yield return new WaitForSeconds(5f);
            attractor.enabled = false;
        }
    }
    public void OnPower(InputValue value)
    {
        if (mainGui.IsCharged)
        {
            mainGui.SetCharged(false);
            StartCoroutine(FireAttractor());
        }
    }
    public void OnFire(InputValue value)
    {
        firing = value.Get<float>() > 0;
       
    } 
    public void OnMove(InputValue action)
    {
  
        var impulse = action.Get<Vector2>();
        HorizontalImpulse = impulse.x;
        VerticalImpulse = impulse.y;
        animator.SetFloat("HorizontalDirection", impulse.x);
    }
    // Update is called once per frame
    void Update()
    { 
        if (nextBullet <= 0&&firing)
        {
            Instantiate(bullet,transform.position,new Quaternion());
            nextBullet = 0.1f;

        }
        else
        {
            nextBullet -= Time.deltaTime;
            if (nextBullet < 0) nextBullet = 0;
        }
        // Update position based on current inputs. 
        transform.position += HorizontalVelocity + VerticalVelocity;

        // Resolve boundary clamping
        var boundarySize = renderer.bounds.size;
        var distance = (transform.position - Camera.main.transform.position).z;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y-boundarySize.y/2f;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y+boundarySize.y/2f;
        var leftBorder = -4+boundarySize.x/2f;
        var rightBorder = 4-boundarySize.x/2f;

        if (transform.position.x < leftBorder)
        {
            transform.position = new Vector3(leftBorder, transform.position.y, transform.position.z);
        }
        if(transform.position.x > rightBorder)
        {
            transform.position = new Vector3(rightBorder, transform.position.y, transform.position.z);
        }
        if(transform.position.y > topBorder)
        {
            transform.position = new Vector3(transform.position.x, topBorder, transform.position.z);
        }
        if(transform.position.y < bottomBorder)
        {
            transform.position = new Vector3(transform.position.x, bottomBorder, transform.position.z);
        }


       


    }
}
