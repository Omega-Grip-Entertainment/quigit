using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grappling : MonoBehaviour
{
    [Header("References")]
    public PhysicsUpdater pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    private Vector3 grapplePoint;
    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;
    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    private bool grappling;
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();
        if (grapplingCdTimer > 0)
        grapplingCdTimer -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        // if (grappling)
        // lr.SetPosition(0, gunTip.position);
    }
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;
        grappling = true;
        pm.frozen = true;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
        }
    private void ExecuteGrapple()
    {
        pm.frozen = false;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }
    public void StopGrapple()
    {
        pm.frozen = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        //lr.enabled = false;
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