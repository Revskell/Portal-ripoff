using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Cube")) Debug.Log("Switched");
    }
}
