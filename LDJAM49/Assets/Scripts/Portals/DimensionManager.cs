using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager instance;
    // [SerializeField] private string lightScene = "Light";
    // [SerializeField] private string darkScene  = "Dark";

    [SerializeField] private LayerMask lightLayer;
    [SerializeField] private LayerMask darkLayer;
    [SerializeField] private LayerMask lightCameraCullingMask;
    [SerializeField] private LayerMask darkCameraCullingMask;

    void Awake(){
        if(instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        lightLayer = LayerMask.NameToLayer("InLight");
        darkLayer = LayerMask.NameToLayer("InDark");

        ChangeCameraDimension();
    }

    void ChangeCameraDimension(){
        Camera.main.cullingMask = lightCameraCullingMask;
    }

    public void ToOtherDimension(GameObject ob){
        if (ob.layer == lightLayer){
            // ob.layer = darkLayer;
            if (ob.CompareTag("Player")){
                Camera.main.cullingMask = darkCameraCullingMask;
            }
            ChangeLayerRecursively(ob, darkLayer);
        } else {
            // ob.layer = lightLayer;
            if (ob.CompareTag("Player")){
                Camera.main.cullingMask = lightCameraCullingMask;
            }
            ChangeLayerRecursively(ob, lightLayer);

        }
    }

    void ChangeLayerRecursively(GameObject obj, int newLayer){
        obj.layer = newLayer;

        foreach (Transform ob in obj.transform)
        {
            ChangeLayerRecursively(ob.gameObject, newLayer);
        }
    }
}
