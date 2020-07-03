using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
    
    public float Velocity;

    public float BarrierMaxDistanceMoving;

    public Sprite triggeredSprite;

    public string barrierMsg;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", BarrierMaxDistanceMoving, "time", 0.75f, "easeType", "easeInOutExpo", "loopType", "pingPong"));
    }

    public void OnProjectileCollision() {
        _spriteRenderer.sprite = triggeredSprite;
    }


}
