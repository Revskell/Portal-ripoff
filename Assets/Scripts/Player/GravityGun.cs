using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [Header("Capacities")]
    [SerializeField] private float shootingPower = 5f;
    [SerializeField] private float range = 10f;
    private Transform oldParent;
    public GameObject hasPicked;

    [Header("Audio")]
    [SerializeField] private AudioClip pickUpSound = null;
    [SerializeField] private AudioClip shootSound = null;

    void Start()
    {
        hasPicked = null;
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(hasPicked == null)
            {
                int layerMask = 1 << 8;
                layerMask = ~layerMask;
                if (Physics.Raycast(transform.parent.position, transform.forward, out RaycastHit hit, range, layerMask))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.CompareTag("Pickable"))
                    {
                        if(target.GetComponent<Turret>())
                        {
                            target.GetComponent<Turret>().PickUp(true);
                            if(!target.GetComponent<Turret>().isDead()) target.GetComponent<Turret>().PlaySound("PickUp");
                        }
                        Adopt(target);
                    }
                }
            } else Release(hasPicked);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (hasPicked != null) Shoot(hasPicked);
            else
            {
                int layerMask = 1 << 8;
                layerMask = ~layerMask;
                if (Physics.Raycast(transform.parent.position, transform.forward, out RaycastHit hit, range, layerMask))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.CompareTag("Pickable") || target.CompareTag("Turret")) Shoot(target);
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && hasPicked != null) Release(hasPicked);
    }

    public void Adopt(GameObject o)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        o.GetComponent<Rigidbody>().isKinematic = true;
        oldParent = o.transform.parent;
        o.transform.position = transform.position;
        o.transform.parent = transform;
        hasPicked = o;
    }

    public void Release(GameObject o)
    {
        if(o.GetComponent<Turret>()) o.GetComponent<Turret>().PickUp(false);
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        o.GetComponent<Rigidbody>().isKinematic = false;
        o.transform.parent = oldParent;
        hasPicked = null;
    }

    public void Shoot(GameObject o)
    {
        if (o.GetComponent<Turret>()) o.GetComponent<Turret>().PickUp(false);
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
        Release(o);
        o.GetComponent<Rigidbody>().velocity = transform.forward * shootingPower;
    }
}
