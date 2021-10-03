using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerControls inputActions;
    PlayerMovement playerMovement;
    PlayerAim playerAim;

    Camera mainCam;

    Vector2 aimPosition { get {return inputActions.Play.AimPosition.ReadValue<Vector2>();}}
    Vector2 aimDirection { get {return inputActions.Play.AimDirection.ReadValue<Vector2>();}}

    void Awake(){
        playerMovement = GetComponent<PlayerMovement>();
        playerAim = GetComponent<PlayerAim>();
        
        inputActions = new PlayerControls();

        inputActions.Play.Move.performed += ctx => Move(ctx.ReadValue<float>());
        inputActions.Play.Move.canceled += ctx => Move(ctx.ReadValue<float>());

        //inputActions.Play.Jump.performed += ctx => JumpInput();

        inputActions.Play.Interact.performed += ctx => InteractInput(true);
        inputActions.Play.Interact.canceled += ctx => InteractInput(false);

        inputActions.Play.Destroy.performed += ctx => DestroyInput(true);
        inputActions.Play.Destroy.canceled += ctx => DestroyInput(false);

        inputActions.Play.Pause.performed += ctx => PauseGame();

        inputActions.Play.Respawn.performed += ctx => Respawn();
        

        mainCam = Camera.main;
        //inputActions.Play.AimPosition.performed += ctx => Aim(ctx.ReadValue<Vector2>());
    }

    void Update(){
        if (aimDirection != Vector2.zero){
            Debug.Log(aimDirection);
        }
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

    void DestroyInput(bool value){
        // playerAim.Remove();
        playerAim.Interact(value, false);
    }
    
    void Move(float direction){
        playerMovement.SetDirection(direction);
    }

    // void JumpInput(){
    //     AudioManager.instance.PlayGlobalSound("Step");
    //     playerMovement.Jump();
    // }

    void PauseGame(){
        // Debug.LogWarning("TODO: Pause the game");
        UIManager.instance.PauseGame();
    }

    void Respawn(){
        GameManager.instance.RespawnPlayer();
    }

    void OnEnable(){
        inputActions.Enable();
    }

    void OnDisable(){
        inputActions.Disable();
    }
}
