using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{

    [SerializeField] private float shootingPower = 5f;
    [SerializeField] private float range = 10f;
    private Transform oldParent;
    private GameObject hasPicked;

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
                    if (target.CompareTag("Pickable")) Adopt(target);
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
                    if (target.CompareTag("Pickable")) Shoot(target);
                }
            }
        }
    }

    public void Adopt(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = true;
        oldParent = o.transform.parent;
        o.transform.position = transform.position;
        o.transform.parent = transform;
        hasPicked = o;
    }

    public void Release(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = false;
        o.transform.parent = oldParent;
        hasPicked = null;
    }

    public void Shoot(GameObject o)
    {
        Release(o);
        o.GetComponent<Rigidbody>().velocity = transform.forward * shootingPower;
    }
}
