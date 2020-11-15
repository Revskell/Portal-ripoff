using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Player player = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) player.Die();
    }
}
