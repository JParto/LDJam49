using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour 
{
    public delegate void ParallaxCameraDelegate(float deltaXMovement, float deltaYMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float oldPosition;
    private Vector2 oldPos;
    void Start()
    {
        oldPosition = transform.position.x;
        oldPos = transform.position;
    }
    void Update()
    {
        // if (transform.position.x != oldPosition)
        // {
        //     if (onCameraTranslate != null)
        //     {
        //         float delta = oldPosition - transform.position.x;
        //         onCameraTranslate(delta);
        //     }
        //     oldPosition = transform.position.x;
        // }

        if((Vector2)transform.position != oldPos){
            if (onCameraTranslate != null){
                Vector2 delta = oldPos - (Vector2)transform.position;
                onCameraTranslate(delta.x, delta.y);
            }
            oldPos = transform.position;
        }
    }
}
