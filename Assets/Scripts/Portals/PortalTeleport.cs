using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private CharacterController cc = null;
    [SerializeField] private PortalTeleport otherPortal = null;
    [SerializeField] private AudioClip goThroughSound = null;
    private Transform player;
    private PortalCam pc;
    public List<GameObject> toExit;

    private void Awake()
    {
        pc = transform.GetChild(0).GetComponent<PortalCam>();
        player = Camera.main.transform;
        toExit = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (!toExit.Contains(o))
        {
            if (o.CompareTag("Player"))
            {
                otherPortal.toExit.Add(o);
                Character_controller body = o.GetComponent<Character_controller>();
                o.GetComponent<Character_controller>().mYaw = Quaternion.LookRotation(pc.RelativeRotation(player.forward)).eulerAngles.y;
                body.mMomentum = Quaternion.LookRotation(pc.RelativeRotation(player.forward)) * (body.mMomentum);
                cc.enabled = false;
                o.transform.position = pc.RelativePosition(o.transform.position);
                cc.enabled = true;
            }
            else if (o.CompareTag("Pickable") || o.CompareTag("Turret"))
            {
                otherPortal.toExit.Add(o);
                Rigidbody body = o.GetComponent<Rigidbody>();
                o.transform.forward = pc.RelativeRotation(o.transform.forward);
                o.transform.localScale *= otherPortal.gameObject.transform.localScale.x / transform.localScale.x;
                body.velocity = Quaternion.LookRotation(pc.RelativeRotation(o.transform.forward)) * (body.velocity);
                o.transform.position = pc.RelativePosition(o.transform.position);
            }
            AudioSource.PlayClipAtPoint(goThroughSound, otherPortal.gameObject.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        toExit.Remove(other.gameObject);
    }
    public void DeactivatePortal() { otherPortal.GetComponent<Collider>().enabled = false; }
    public void ActivatePortal() { otherPortal.GetComponent<Collider>().enabled = true; }
}
