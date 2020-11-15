using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    [SerializeField] public Transform otherCam = null;
    [SerializeField] private Transform otherPortal = null;
    [SerializeField] private Transform portal = null;
    private Transform player;
    private Camera thisCam;

    void Awake()
    {
        player = Camera.main.transform;
        thisCam = GetComponent<Camera>();
    }

    void Update()
    {
        thisCam.nearClipPlane = Vector3.Distance(transform.position, portal.position) + 0.6f;
        otherCam.position = RelativePosition(player.position);
        otherCam.forward = RelativeRotation(player.forward);
    }

    public Vector3 RelativePosition(Vector3 targetPos) { return otherPortal.TransformPoint(portal.InverseTransformPoint(targetPos)); }
    public Vector3 RelativeRotation(Vector3 targetForward) { return otherPortal.TransformDirection(portal.InverseTransformDirection(targetForward)); }
}
