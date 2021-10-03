using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private float shootSpeed = 1.5f;

    void Start(){
        InvokeRepeating("Shoot", 0f, shootSpeed);
    }

    void Shoot(){
        Instantiate(bullet, shootPoint.position, transform.rotation);
    }
}
