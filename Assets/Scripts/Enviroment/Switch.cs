using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    private bool InRange;
    private bool switched;
    private GameObject spawn;

    [SerializeField] public bool Destructible = true;
    [SerializeField] public GameObject Spawner = null;
    [SerializeField] public GameObject Cube = null;

    [Header("Sounds")]
    [SerializeField] private AudioClip spawnCube = null;
    [SerializeField] private AudioClip deleteCube = null;
    [SerializeField] private AudioClip switchOn = null;
    [SerializeField] private AudioClip switchOff = null;

    void Start()
    {
        InRange = false;
        switched = false;
    }
    
    void Update()
    {
        if(InRange)
        {
            if(!switched)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switched = true;
                    AudioSource.PlayClipAtPoint(switchOn, transform.position);
                    Invoke("SpawnCube", 2);
                }
            }
        }
    }

    void SpawnCube()
    {
        if (!Destructible) Instantiate(Cube, null).transform.position = Spawner.transform.position;
        else
        {
            if(spawn != null)
            {
                AudioSource.PlayClipAtPoint(deleteCube, spawn.transform.position);
                Destroy(spawn.gameObject);
                spawn = Instantiate(Cube, null);
                spawn.transform.position = Spawner.transform.position;
            }
            else
            {
                spawn = Instantiate(Cube, null);
                spawn.transform.position = Spawner.transform.position;
            }
        }
        AudioSource.PlayClipAtPoint(spawnCube, Spawner.transform.position);
        Invoke("PlayAudio", 1);
    }

    void PlayAudio() {
        AudioSource.PlayClipAtPoint(switchOff, transform.position);
        switched = false;
    }

    private void OnTriggerEnter(Collider other) { if (other.tag == "Player") InRange = true; }
    private void OnTriggerExit(Collider other) { if (other.tag == "Player") InRange = false; }
}
