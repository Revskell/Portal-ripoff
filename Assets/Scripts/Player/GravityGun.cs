using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [Header("Capacities")]
    [SerializeField] private float shootingPower = 5f;
    [SerializeField] private float range = 10f;
    private Transform oldParent;
    private GameObject hasPicked;

    [Header("Audio")]
    [SerializeField] private AudioClip pickUpSound = null;
    [SerializeField] private AudioClip holdSound = null;
    [SerializeField] private AudioClip shootSound = null;

    void Start()
    {
        hasPicked = null;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(hasPicked == null)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.parent.position, transform.forward, out hit, range))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.CompareTag("Pickable"))
                    {
                        if(target.GetComponent<Turret>())
                        {
                            target.GetComponent<Turret>().PickUp(true);
                            target.GetComponent<Turret>().PlaySound("PickUp");
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
                RaycastHit hit;

                if (Physics.Raycast(transform.parent.position, transform.forward, out hit, range))
                {
                    GameObject target = hit.transform.gameObject;
                    if (target.CompareTag("Pickable")||target.CompareTag("Turret")) Shoot(target);
                }
            }
        }
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
