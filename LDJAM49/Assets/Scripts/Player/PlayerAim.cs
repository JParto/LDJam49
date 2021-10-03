using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Drawables;
using Cinemachine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;

    [SerializeField] private float raycastDistance;
    [SerializeField] private float raycastSelectedDistance;
    [SerializeField] private LayerMask whatIsInteractable;
    // [SerializeField] private LayerMask whatIsDrawable;
    [SerializeField] private string whatIsInitiateTag = "Initiate";
    [SerializeField] private AudioPlayer audioPlayer;
    private float audioCoolDown = 1f;
    private float currentAudioCoolDown;
    private bool playedAudio;
    private Vector2 aimDirection = Vector2.right;
    private bool canInteract;
    private bool interacting;
    private bool removing;
    
    private float currentRaycastDistance { get {return interacting ? raycastSelectedDistance : raycastDistance;} }
    private Vector2 reverseHitPosition { get {return (Vector2)transform.position + (aimDirection * currentRaycastDistance);} }

    DrawablePlatform drawablePlatform;
    DrawablePlatform indicatorPlatform;
    Transform platformTransform;

    void Start(){
        currentAudioCoolDown = audioCoolDown;
    }

    void Update(){
        RaycastHit2D[] hits;

        if (removing){
            hits = Physics2D.RaycastAll(reverseHitPosition, -aimDirection, currentRaycastDistance, whatIsInteractable);
        } else {
            hits = Physics2D.RaycastAll(transform.position, aimDirection, currentRaycastDistance, whatIsInteractable);
        }

        if (hits.Length > 0){
            RaycastHit2D hit;
            hit = FurthestHit(hits);

            if (interacting && hit){
                DrawablePlatform platform = hit.collider.GetComponentInParent<DrawablePlatform>();


                if (drawablePlatform == null){ // register the platform to interact with
                    drawablePlatform = platform;
                    drawablePlatform.reachedEndEvent.AddListener(Release);
                    drawablePlatform.Interact(true);
                }

                if (drawablePlatform == platform){
                    if (removing){
                        platform.ErasePath(hit.point);
                    } else {
                        platform.MakePath(hit.point);
                    }
                    if (hit.transform != platformTransform && interacting){ // idk why interacting needs to be here, but it does
                        platformTransform = hit.transform;
                        cameraTargetGroup.AddMember(platformTransform, 1, 0);
                    }
                }
            }

            if (hit){
                if (hit.collider.CompareTag(whatIsInitiateTag)){
                    DrawablePlatform platform = hit.collider.GetComponentInParent<DrawablePlatform>();
                    
                    if (indicatorPlatform == null){
                        indicatorPlatform = platform;
                        platform.ShowIndicator(true);
                        if (!playedAudio && !interacting){
                            audioPlayer.PlaySound("Indicator");
                            playedAudio = true;
                            currentAudioCoolDown = audioCoolDown;
                        }
                    }

                    canInteract = true;
                } else {
                    if (indicatorPlatform){
                        indicatorPlatform.ShowIndicator(false);
                        indicatorPlatform = null;
                    }
                    canInteract = false;
                }
            } else {
                if (indicatorPlatform){
                    indicatorPlatform.ShowIndicator(false);
                    indicatorPlatform = null;
                }
                canInteract = false;
            }
        } else {
            if (indicatorPlatform){
                indicatorPlatform.ShowIndicator(false);
                indicatorPlatform = null;
            }
            canInteract = false;
        }

        if (playedAudio){
            currentAudioCoolDown -= Time.deltaTime;
            if (currentAudioCoolDown < 0f){
                playedAudio = false;
            }
        }
    }   

    RaycastHit2D FurthestHit(RaycastHit2D[] hits){
        RaycastHit2D currentHit = hits[0];
        float currentDistance = 0f;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.GetComponentInParent<DrawablePlatform>() == drawablePlatform){
                float d;
                if (removing){
                    d = Vector2.Distance(reverseHitPosition, hit.point);
                } else {
                    d = Vector2.Distance(transform.position, hit.point);
                }
                
                if (d > currentDistance){
                    currentDistance = d;
                    currentHit = hit;
                }
            }
        }

        return currentHit;
    }

    public void AimAt(Vector2 direction){
        aimDirection = direction;
    }

    public void Interact(bool value, bool drawing){
        if (!canInteract){
            interacting = false;
            removing = false;
        } else {
            interacting = value;
            removing = !drawing;
            if (interacting){
                audioPlayer.PlaySound("DrawLine", true);
            }
        }

        if (!interacting){
            Release();
            // drawablePlatform = null;
            // cameraTargetGroup.RemoveMember(platformTransform);
            // platformTransform = null;
            // removing = false;
        }
    }

    private void Release(){
        if (platformTransform == null){
            return;
        }
        cameraTargetGroup.RemoveMember(platformTransform);
        platformTransform = null;
        removing = false;
        interacting = false;
        if (drawablePlatform){
            drawablePlatform.reachedEndEvent.RemoveListener(Release);
            drawablePlatform.Interact(false);
        }
        drawablePlatform = null;
        audioPlayer.StopLoopSound();
        audioPlayer.PlaySound("DrawLineDone");
    }

    // public void Remove(){
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, raycastDistance, whatIsDrawable);

    //     if (hit){
    //         DrawablePlatform platform = hit.collider.GetComponentInParent<DrawablePlatform>();

    //         if(platform){
    //             platform.RemovePath();
    //         }
    //     }
    // }

    // public void RemoveSimilar(){
    //     Vector2 reverseHitStartPosition = (Vector2)transform.position + (aimDirection * raycastDistance);

    //     RaycastHit2D[] reverseHit = Physics2D.RaycastAll(reverseHitStartPosition, -aimDirection, raycastDistance, whatIsInteractable);
    // }

    void OnDrawGizmos(){
        if(!removing){
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (aimDirection*currentRaycastDistance)+(Vector2)transform.position);
        } else {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(reverseHitPosition, transform.position);
        }
    }
}
