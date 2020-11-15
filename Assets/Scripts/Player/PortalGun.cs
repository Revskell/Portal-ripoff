using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Crosshair))]
public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject orangePortal = null;
    [SerializeField] private GameObject bluePortal = null;
    [SerializeField] private GravityGun gravityGun = null;
    private PortalTeleport orangePT;
    private PortalTeleport bluePT;

    private Transform player;
    private Animator anim;
    private Crosshair ch;
    private int layerMask;
    private float scale;

    void Start()
    {
        orangePT = orangePortal.GetComponent<PortalTeleport>();
        bluePT = bluePortal.GetComponent<PortalTeleport>();
        ch = GetComponent<Crosshair>();
        anim = GetComponent<Animator>();
        player = Camera.main.transform;
        HidePortals();
        layerMask = 1 << 8;
        layerMask = ~layerMask;
        scale = 1f;
    }
    
    void Update()
    {
        if(gravityGun.hasPicked == null)
        {
            if (Input.GetMouseButtonDown(0)) ShootPortal(true);
            else if (Input.GetMouseButtonDown(1)) ShootPortal(false);
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                if (scroll < 0f) scale = Mathf.Max(scale - 0.25f, 0.5f);
                else scale = Mathf.Min(scale + 0.25f, 2f);
                ch.Scale(scale);
            }
        }
    }

    private void ShootPortal(bool orangeOrBlue)
    {
        if (Physics.Raycast(player.position, player.forward, out RaycastHit hit, 300f, layerMask))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Portable")) PlacePortal(orangeOrBlue, CorrectPlacement(hit.point, target, orangeOrBlue, hit.normal), hit.normal);
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
        ch.SetPortal(orangeOrBlue, false);
    }

    private void PlacePortal(bool orangeOrBlue, Vector3 newPos, Vector3 newForward)
    {
        (orangeOrBlue ? orangePortal : bluePortal).transform.position = newPos;
        (orangeOrBlue ? orangePortal : bluePortal).transform.forward = newForward;
        (orangeOrBlue ? orangePortal : bluePortal).transform.localScale = new Vector3(scale, scale, scale);
        (orangeOrBlue ? orangePT : bluePT).ActivatePortal();
        anim.SetTrigger(orangeOrBlue ? "orange":"blue");
        ch.SetPortal(orangeOrBlue, true);
    }

    private Vector3 CorrectPlacement(Vector3 point, GameObject other, bool orangeOrBlue, Vector3 normal)
    {
        Quaternion rotation = (orangeOrBlue ? orangePortal : bluePortal).transform.rotation;
        for (float f = 0f; f <= 1f * scale; f += 0.1f) //checks right
        {
            Debug.DrawRay(player.position, (point + rotation * new Vector3(1f * scale - f, 0) - player.position), Color.green, 2f);
            if (Physics.Raycast(player.position, (point + rotation * new Vector3(1f * scale - f, 0) - player.position), out RaycastHit hit, 300f, layerMask) && hit.transform.gameObject.Equals(other) && hit.normal.Equals(normal))
            {
                point += rotation * new Vector3(-f, 0);
                break;
            }
        }
        for (float f = 0f; f <= 1f * scale; f += 0.1f) //checks left
        {
            Debug.DrawRay(player.position, (point + rotation * new Vector3(-1f * scale + f, 0) - player.position), Color.blue, 2f);
            if (Physics.Raycast(player.position, (point + rotation * new Vector3(-1f * scale + f, 0) - player.position), out RaycastHit hit, 300f, layerMask) && hit.transform.gameObject.Equals(other) && hit.normal.Equals(normal))
            {
                point += rotation * new Vector3(f, 0);
                break;
            }
        }
        for (float f = 0f; f <= 2f * scale; f += 0.1f) //checks up
        {
            Debug.DrawRay(player.position, (point + rotation * new Vector3(0, 2f * scale - f) - player.position), Color.red, 2f);
            if (Physics.Raycast(player.position, (point + rotation * new Vector3(0, 2f * scale - f) - player.position), out RaycastHit hit, 300f, layerMask) && hit.transform.gameObject.Equals(other) && hit.normal.Equals(normal))
            {
                point += rotation * new Vector3(0, -f);
                break;
            }
        }
        for (float f = 0f; f <= 2f * scale; f += 0.1f) //checks down
        {
            Debug.DrawRay(player.position, (point + rotation * new Vector3(0, -2f * scale + f) - player.position), Color.magenta, 2f);
            if (Physics.Raycast(player.position, (point + rotation * new Vector3(0, -2f * scale + f) - player.position), out RaycastHit hit, 300f, layerMask) && hit.transform.gameObject.Equals(other) && hit.normal.Equals(normal))
            {
                point += rotation * new Vector3(0, f);
                break;
            }
        }
        return point;
    }

    private void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

}
