using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Transform respawnPoint;

    void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void Respawn(){
        transform.position = respawnPoint.position;
        // transform.rotation = respawnPoint.rotation;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -respawnPoint.rotation.y, transform.rotation.z));
        int newLayer = respawnPoint.gameObject.layer;
        ChangeLayerRecursively(gameObject, newLayer);
    }

    void ChangeLayerRecursively(GameObject obj, int newLayer){
        obj.layer = newLayer;

        foreach (Transform ob in obj.transform)
        {
            ChangeLayerRecursively(ob.gameObject, newLayer);
        }
    }
}
