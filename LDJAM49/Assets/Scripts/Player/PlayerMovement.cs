using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB;
    private CapsuleCollider2D cc;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private LayerMask lightGround;
    [SerializeField] private LayerMask darkGround;
    private LayerMask groundMask    { 
                                        get {
                                            if(gameObject.layer == LayerMask.NameToLayer("InLight")){
                                                return lightGround;
                                            } else {
                                                return darkGround;
                                            }
                                        }
                                    }
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private CinemachineVirtualCamera bigCam;
    [SerializeField] private float camLerp = 0.1f;

    public FallingHelp fallingHelp;
    


    // slopes
    private float slopeDownAngle;
    private float lastSlopeAngle;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerp;


    private static float fullCircle = 360f;
    private float camDutchOffset = 0f;


    // properties
    public bool isGrounded { get; private set; }
    public bool isInAir { get; private set; }
    public float moveDirection { get; private set; }
    public Vector2 ccSize { get {return cc.size;} }


    void Awake(){
        RB = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        
    }

    void Update(){

        Vector2 offset = SetMagnitude(-transform.up, ccSize.y/2);
        Debug.DrawLine(transform.position, transform.position+(Vector3)offset, Color.blue);
        Vector2 checkPos = transform.position + (Vector3)offset;

        CheckGround(checkPos);
        PushDown(checkPos);

        SlopeCheck(checkPos);
        MoveHorizontal();

        bigCam.m_Lens.Dutch = camDutchOffset;
    }

    void SlopeCheck(Vector2 checkPos){

        SlopeCheckVertical(checkPos);
        // SlopeCheckHorizontal(checkPos);

        // if (isOnSlope && RB.velocity.magnitude <= stopMoveSpeed && moveDirection == 0)
        // {
        //     RB.sharedMaterial = fullFriction;
        // }
        // else
        // {
        //     RB.sharedMaterial = noFriction;
        // }

        RB.SetRotation(-slopeDownAngle);
    }

    // private void SlopeCheckHorizontal(Vector2 checkPos){
    //     RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundMask);
    //     RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundMask);


    //     if (slopeHitFront)
    //     {
    //         isOnSlope = true;

    //         slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

    //     }
    //     else if (slopeHitBack)
    //     {
    //         isOnSlope = true;

    //         slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
    //     }
    //     else
    //     {
    //         slopeSideAngle = 0.0f;
    //         isOnSlope = false;
    //     }

    // }

    private void SlopeCheckVertical(Vector2 checkPos){
        RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, slopeCheckDistance, groundMask);

        if (hit){
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.SignedAngle(hit.normal, Vector2.up);

            // if (slopeDownAngle != lastSlopeAngle){
            //     isOnSlope = true;
            // }

            SetCameraDutch();

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }


    }

    private void SetCameraDutch(){
        float slopeSign = Mathf.Sign(-slopeDownAngle);
        float dutchSign = Mathf.Sign(-lastSlopeAngle);

        if (-slopeDownAngle <= -170f && -lastSlopeAngle >= 170f){
            camDutchOffset += fullCircle;
        } else if (-slopeDownAngle >= 170f && -lastSlopeAngle <= -170f){
            camDutchOffset -= fullCircle;

        }

        float lerpedDutch = Mathf.Lerp(cam.m_Lens.Dutch, -slopeDownAngle + camDutchOffset, camLerp);
        cam.m_Lens.Dutch = lerpedDutch;

    }

    void MoveHorizontal(){
        Vector2 move = Vector2.zero;

        if (isGrounded){
            move.Set(moveSpeed*-slopeNormalPerp.x*moveDirection, moveSpeed*-slopeNormalPerp.y*moveDirection);
        } else {
            Vector2 gravityVector = GetGravityVector();
            move.Set((moveSpeed*-slopeNormalPerp.x*moveDirection) + gravityVector.x, (moveSpeed*-slopeNormalPerp.y*moveDirection) + gravityVector.y);
        }

        RB.velocity = move;

    }

    Vector2 GetGravityVector(){
        float newMag = fallingHelp.magnitude;
        Vector2 direction = -transform.up;

        Vector2 gravityVector = SetMagnitude(direction, newMag);
        return gravityVector;
    }

    void CheckGround(Vector2 checkPos){
        Vector2 off = new Vector2(0.1f, 0.1f);
        Vector2 offset = SetMagnitude(slopeNormalPerp, 0.1f);

        Collider2D[] cols = Physics2D.OverlapAreaAll(checkPos - offset, checkPos + offset, groundMask);
        
        if (cols.Length > 0){
            isGrounded = true;
            fallingHelp.Reset();
        } else {
            isGrounded = false;
        }
    }

    void PushDown(Vector2 checkPos){
        // Vector2 checkPos = transform.position - new Vector3(0f, ccSize.y/2);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, 0.5f, groundMask);

        if (hit){
            // Vector3 wayDown = -transform.up;
            Vector3 wayDown = SetMagnitude(-transform.up, hit.distance-0.001f);
            transform.position = new Vector3(transform.position.x + wayDown.x, transform.position.y + wayDown.y);
            isInAir = true;
        } else {
            isInAir = false;
        }

    }

    Vector2 SetMagnitude(Vector2 v, float newMag){
        float mag = v.magnitude;
        // float newSqrMag = newMag*newMag;
        float new_vx = v.x * newMag / mag;
        float new_vy = v.y * newMag / mag;

        return new Vector2(new_vx, new_vy);
    }

    public void SetDirection(float value){
        moveDirection = value;
    }

}


