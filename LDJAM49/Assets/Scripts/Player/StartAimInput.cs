using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAimInput : MonoBehaviour
{
    PlayerControls inputActions;
    PlayerAim playerAim;

    Camera mainCam;

    Vector2 aimPosition { get {return inputActions.Play.AimPosition.ReadValue<Vector2>();}}
    Vector2 aimDirection { get {return inputActions.Play.AimDirection.ReadValue<Vector2>();}}

    void Awake(){
        playerAim = GetComponent<PlayerAim>();
        
        inputActions = new PlayerControls();

        inputActions.Play.Interact.performed += ctx => InteractInput(true);
        inputActions.Play.Interact.canceled += ctx => InteractInput(false);

        mainCam = Camera.main;
        //inputActions.Play.AimPosition.performed += ctx => Aim(ctx.ReadValue<Vector2>());
    }

    void Update(){
        if (aimPosition != Vector2.zero){
            Vector2 endPos = mainCam.ScreenToWorldPoint(aimPosition);
            Vector2 mouseAimDirection = endPos - (Vector2)transform.position;
            Aim(mouseAimDirection.normalized);
        } else {
            Aim(aimDirection.normalized);
        }
    }

    void Aim(Vector2 direction){
        playerAim.AimAt(direction);
    }

    void InteractInput(bool value){
        playerAim.Interact(value, true);
    }

    void OnEnable(){
        inputActions.Enable();
    }

    void OnDisable(){
        inputActions.Disable();
    }
}
