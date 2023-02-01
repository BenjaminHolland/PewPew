using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFinishedAnim : MonoBehaviour
{
    public void DestroySelf(AnimationEvent e){
        GameObject.Destroy(this.gameObject,0.3f);
    }
}
