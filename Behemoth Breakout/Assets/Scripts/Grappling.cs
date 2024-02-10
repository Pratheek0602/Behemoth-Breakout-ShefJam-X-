using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    public Rigidbody rb;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime = 0;
    public float overshootYAxis;
    // public bool holding;

    private Vector3 grapplePoint;
    float highestPointOnArc;
    private SpringJoint joint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode leaveGrappleKey = KeyCode.Mouse0;

    private bool grappling;
    private bool reachedGrapplePoint;
    

    private void Start()
    {
        pm = GetComponent<PlayerMovementGrappling>();
    }

    private void Update()
    {
        Debug.Log(rb.position);
        // if (Input.GetKeyDown(grappleKey) && !grappling) StartGrapple();
        if (Input.GetKeyDown(grappleKey) && !grappling) {
            Debug.Log("idk idk");
            // StopGrapple();
            StartGrapple(); 
        }

        if (Input.GetKeyUp(grappleKey)) {
            
            StopGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;

        SubStateMachine();
        
    }

    private void LateUpdate()
    {
        if (grappling)
           lr.SetPosition(0, gunTip.position);
            // SubStateMachine();
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        reachedGrapplePoint = false;

        pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;            
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
            // holding = true
            // rb.useGravity = false;
            // rb.velocity = Vector3.zero;

        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        // lr.SetPosition(1, grapplePoint);

        
    }

    private void SubStateMachine(){
        // float horizontalInput = Input.GetAxisRaw("Horizontal");
        // float verticalInput = Input.GetAxisRaw("Vertical");
        // bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;
        // lr.SetPosition(1, grapplePoint);

        if (Vector3.Distance(rb.position, grapplePoint) <= 0.25f && grappling)
        {
            // Player has reached the point
            // Debug.Log("Player has reached the target point.");
            if (rb.position.y != -0.23){
                FreezeRigidbodyOnLedge();
            }
            
            
            // if (Input.GetKeyDown(grappleKey)) StopGrapple();          
        }
        rb.useGravity = true;
        
             
            // pm.freeze = true;
            // if (Input.GetKeyDown(leaveGrappleKey)) StopGrapple(); 
        
    }

    
    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        // reachedGrapplePoint = true;

        // Debug.Log("Executed grapple");

        // if (Vector3.Distance(rb.position, grapplePoint) <= 0.3f) {
        //     SubStateMachine();
        // }
        

        // // stick to location
        // rb.useGravity = false;

        // if (Input.GetKeyDown(leaveGrappleKey)){
        //     Invoke(nameof(StopGrapple), 1f);
        // }
        
    }

    private void FreezeRigidbodyOnLedge(){
        rb.useGravity = false;
        pm.freeze = true;
         
    }

    public void StopGrapple()
    {
        // Debug.Log("Should fall down");

        rb.useGravity = true;

        pm.freeze = false;

        grappling = false;

        reachedGrapplePoint = false;

        // grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }


    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
