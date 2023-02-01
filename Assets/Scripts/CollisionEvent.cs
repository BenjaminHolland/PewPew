using UnityEngine;

public class CollisionEvent{

    // We do lazy-load and cache interactions.
    // Todo : A proper service locator type access for interactions
    private static Interactions interactions = null;

    /// Process kinematic collisions or trigger based collisions/OnTriggerEnter2D(...)
    public static bool ProcessUnitKinematicCollision( GameObject caller, Collider2D collider ){

        // Process Damage and physics effects
        if(!CollisionEvent.ProcessUnitCollision(caller, collider.gameObject)){
            return false;
        }

        // Show explosion
        var boom = GameObject.Instantiate( Resources.Load<GameObject>("BoomSmall") , caller.transform.position, Quaternion.identity );

        return true;
    }

    /// Process dynamic collisions or collision/rigidbody based collisions via OnCollisionEnter2D(...)
    public static bool ProcessUnitDynamicCollision( GameObject caller, Collision2D collision ){

        // Process Damage and physics effects
        if(!CollisionEvent.ProcessUnitCollision(caller, collision.gameObject)){
            return false;
        }

        // Show explosion
        var contact = collision.GetContact(0).point;
        var pos = new Vector3( contact.x , contact.y , caller.transform.position.z );
        var boom = GameObject.Instantiate( Resources.Load<GameObject>("BoomSmall") , pos, Quaternion.identity );
        return true;
    }

    /// Process non collision-type specific collisions. Called by ProcessUnit*Collision methods in CollisionEvent.
    public static bool ProcessUnitCollision(GameObject caller, GameObject callee){

        // Todo : Interactions needs a real accessor via scriptedobject, servicelocator or singleton
        if(CollisionEvent.interactions == null){
            CollisionEvent.interactions = GameObject.Find("Interactions").GetComponent<Interactions>();
        }

        // We only care about collisions with other IUnits
        if (!caller.CompareTag("unit") || !callee.CompareTag("unit"))
        {
            return false;
        }

        // Process as units
        var ucaller = caller.GetComponent<IUnit>();
        var ucallee = callee.GetComponent<IUnit>();

        // Non woogly collisions should not occur
        if(!ucaller.Woogly && !ucallee.Woogly){
            return false;
        }

        // Check for effects, if there are none, return
        var effects = CollisionEvent.interactions.GetEffects(ucaller.GetType(), ucallee.GetType());
        if(effects == null){
            return false;
        }

        // Deal hit damage to caller and callee
        foreach (var obj in effects)
        {
            ucaller.HitWith(obj);
            ucallee.HitWith(obj);
        }

        // We hit a woogly as a non woogly, so toss it away
        if(!ucaller.Woogly && ucallee.Woogly){
            var rb = callee.GetComponent<Rigidbody2D>();
            var direction = caller.transform.position-callee.transform.position;
            rb.AddForce(-direction*100,ForceMode2D.Impulse);
        }

        return true;
    }


}