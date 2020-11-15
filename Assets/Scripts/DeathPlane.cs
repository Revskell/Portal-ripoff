using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        other.transform.position = Vector3.zero + new Vector3(0, other.transform.localScale.y * 2);
        if (cc != null) cc.enabled = true;
    }
}
