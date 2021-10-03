using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHelp : MonoBehaviour
{
    Rigidbody2D RB;
    Vector3 originalPosition;

    public Vector2 velocity {get {return RB.velocity;} }

    public float magnitude {get {return RB.velocity.magnitude;} }

    void Awake(){
        RB = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
    }

    public void SetVelocity(Vector2 move){
        RB.velocity = move;
    }

    public void SetPosition(Vector3 position){
        transform.position = position;
    }

    public void Reset(){
        SetVelocity(Vector2.zero);
        SetPosition(originalPosition);
    }
}

