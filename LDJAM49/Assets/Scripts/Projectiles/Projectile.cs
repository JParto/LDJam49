using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Drawables;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Ground")){

            Destroy(gameObject);
        } else if (col.CompareTag("Player")){
            Debug.Log("TODO; Hurt player");

            Destroy(gameObject);
        } else if (col.CompareTag("Drawable")){
            // var platform = col.GetComponent<DrawablePlatform>();
            // platform.RemovePath();

            Destroy(gameObject);
        }
    }
}
