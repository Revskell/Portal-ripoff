using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{

    [Header("HUD")]
    [SerializeField] private Text checkpointText = null;

    [Header("Sounds")]
    [SerializeField] private AudioClip checkpointSound = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Checkpoints.AddPoint(transform.position, checkpointSound);
            // checkpointText.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
