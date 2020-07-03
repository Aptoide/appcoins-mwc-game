using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticle : MonoBehaviour {

    public GameObject effect;
    private bool _isColliding;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (_isColliding)
            return;

        _isColliding = true;

        // Spawn Effect, then remove Script
        GameObject particle = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
        Destroy(particle,1f);
    }

    //void OnCollisionExit2D(Collision2D coll) {
    //    _isColliding = false;
    //}
}
