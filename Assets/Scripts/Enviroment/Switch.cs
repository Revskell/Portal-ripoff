using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    private bool InRange;
    private bool switched;
    private GameObject CubeSpawn;
    private GameObject ExplosionSpawn;

    [Header("Settings")]
    [SerializeField] public bool Destructible = true;
    [SerializeField] public GameObject Spawner = null;
    [SerializeField] public GameObject Cube = null;
    [SerializeField] public GameObject ExplosionEffect = null;

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
    
    void FixedUpdate()
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
            if(CubeSpawn != null)
            {
                AudioSource.PlayClipAtPoint(deleteCube, CubeSpawn.transform.position);
                StartParticleSystem();
                Destroy(CubeSpawn.gameObject);
                CubeSpawn = Instantiate(Cube, null);
                CubeSpawn.transform.position = Spawner.transform.position;
            }
            else
            {
                CubeSpawn = Instantiate(Cube, null);
                CubeSpawn.transform.position = Spawner.transform.position;
            }
        }
        AudioSource.PlayClipAtPoint(spawnCube, Spawner.transform.position);
        Invoke("PlayAudio", 1);
    }

    void PlayAudio() {
        AudioSource.PlayClipAtPoint(switchOff, transform.position);
        switched = false;
    }

    void StartParticleSystem()
    {
        ExplosionSpawn = Instantiate(ExplosionEffect, CubeSpawn.transform.position, CubeSpawn.transform.rotation);
        ExplosionSpawn.GetComponent<ParticleSystem>().Play();
        StartCoroutine(StopParticleSystem(ExplosionSpawn, 1));
    }

    IEnumerator StopParticleSystem(GameObject particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        ExplosionSpawn.GetComponent<ParticleSystem>().Stop();
        Destroy(ExplosionSpawn);
    }

    private void OnTriggerEnter(Collider other) { if (other.tag == "Player") InRange = true; }
    private void OnTriggerExit(Collider other) { if (other.tag == "Player") InRange = false; }
}
