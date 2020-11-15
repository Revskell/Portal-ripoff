using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject orangePortal = null;
    [SerializeField] private GameObject bluePortal = null;
    private PortalTeleport orangePT;
    private PortalTeleport bluePT;

    private Transform player;
    private Animator anim;

    void Start()
    {
        orangePT = orangePortal.GetComponent<PortalTeleport>();
        bluePT = bluePortal.GetComponent<PortalTeleport>();
        player = Camera.main.transform;
        anim = GetComponent<Animator>();
        HidePortals();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) ShootPortal(true);
        else if (Input.GetMouseButtonDown(1)) ShootPortal(false);
    }

    private void ShootPortal(bool orangeOrBlue)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (Physics.Raycast(player.position, player.forward, out RaycastHit hit, 300f, layerMask))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Portable")) PlacePortal(orangeOrBlue, hit.point, hit.normal);
            else anim.SetTrigger("no");
        }
        else anim.SetTrigger("no");
    }

    private void HidePortals() { HidePortal(true); HidePortal(false); }
    private void HidePortal(bool orangeOrBlue)
    {
        (orangeOrBlue ? orangePortal : bluePortal).transform.position = new Vector3(0, -5, 0);
        orangePT.DeactivatePortal();
        bluePT.DeactivatePortal();
    }

    private void PlacePortal(bool orangeOrBlue, Vector3 newPos, Vector3 newForward)
    {
        (orangeOrBlue ? orangePortal : bluePortal).transform.position = newPos;
        (orangeOrBlue ? orangePortal : bluePortal).transform.forward = newForward;
        (orangeOrBlue ? bluePT : orangePT).ActivatePortal();
        anim.SetTrigger(orangeOrBlue ? "orange":"blue");
    }

    private void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

}
