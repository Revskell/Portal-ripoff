using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.position = Vector3.zero + new Vector3(0, collision.transform.localScale.y*2);
    }
}
