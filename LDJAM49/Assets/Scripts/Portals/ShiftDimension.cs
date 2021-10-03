using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftDimension : MonoBehaviour
{
    private DimensionManager dimensionManager => DimensionManager.instance;
    private AudioManager audioManager => AudioManager.instance;

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            dimensionManager.ToOtherDimension(col.gameObject);

            audioManager.PlayGlobalSound("DimensionShift");
            
        } else if (col.CompareTag("Projectile")){
            dimensionManager.ToOtherDimension(col.gameObject);
        }
    }
}
