using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{

    [SerializeField] private Transform portal = null;
    [SerializeField] private Transform otherPortal = null;
    private Transform player;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        transform.position = portal.position + (player.position - otherPortal.position);
        transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(Quaternion.Angle(portal.rotation, otherPortal.rotation), Vector3.up) * player.forward, Vector3.up);
    }
}
