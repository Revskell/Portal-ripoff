using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{

    [SerializeField] private Animator anim = null;
    [SerializeField] private GameObject Door = null;
    [SerializeField] private AudioClip openDoorSound = null;
    [SerializeField] private AudioClip closeDoorSound = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Pickable"))
        {
            if(collision.gameObject.name.Contains("Cube"))
            {
                anim.SetBool("Open", true);
                AudioSource.PlayClipAtPoint(openDoorSound, Door.transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Pickable"))
        {
            if (collision.gameObject.name.Contains("Cube"))
            {
                anim.SetBool("Open", false);
                AudioSource.PlayClipAtPoint(closeDoorSound, Door.transform.position);
            }
        }
    }
}
