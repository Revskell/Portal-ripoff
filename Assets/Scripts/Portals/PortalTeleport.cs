using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalTeleport : MonoBehaviour
{
    [SerializeField] private CharacterController cc = null;
    [SerializeField] private Collider otherCollider = null;
    private Transform player;
    private PortalCam pc;

    private void Awake()
    {
        pc = transform.GetChild(0).GetComponent<PortalCam>();
        player = Camera.main.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.CompareTag("Player"))
        {
            DeactivatePortal();
            cc.enabled = false;
            o.transform.position = pc.RelativePosition(player.position);
            o.GetComponent<Character_controller>().mYaw = pc.transform.localEulerAngles.y;
            Character_controller body = o.GetComponent<Character_controller>();
            body.mMomentum = Quaternion.AngleAxis(Quaternion.Angle(player.rotation, pc.transform.rotation), Vector3.up) * (body.mMomentum);
            cc.enabled = true;
            Invoke("ActivatePortal", 0.5f);
        }
        else if (o.CompareTag("Pickable"))
        {
            DeactivatePortal();
            o.GetComponent<Collider>().enabled = false;
            o.transform.position = pc.RelativePosition(o.transform.position);
            o.transform.forward = pc.RelativeRotation(o.transform.forward);
            Rigidbody body = o.GetComponent<Rigidbody>();
            body.velocity = o.transform.forward.normalized * body.velocity.magnitude;
            StartCoroutine(ActivateObject(o.GetComponent<Collider>()));
            Invoke("ActivatePortal", 0.5f);
        }
    }
    public void DeactivatePortal() { otherCollider.enabled = false; }
    public void ActivatePortal() { otherCollider.enabled = true; }

    private IEnumerator ActivateObject(Collider c)
    {
        yield return new WaitForSeconds(0.25f);
        c.enabled = true;
    }
}
