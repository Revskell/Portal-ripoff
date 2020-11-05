using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float rotationX = 0f;
    private bool dead = false;
    private bool hasSpawned = false;
    private bool hasBeenPicked = false;
    private GameObject ExplosionSpawn;

    [SerializeField] public GameObject ExplosionEffect = null;

    [Header("Sounds")]
    [SerializeField] public List<AudioClip> DeathSounds = new List<AudioClip>();
    [SerializeField] public List<AudioClip> PickUpSounds = new List<AudioClip>();

    void Start()
    {
        Invoke("ActivateTurret", 1);
    }

    void Update()
    {
        if(hasSpawned)
        {
            if (!dead && !hasBeenPicked)
            {
                if (transform.rotation.x != rotationX)
                {
                    PlaySound("Death");
                    StartParticleSystem();
                    dead = true;
                }
            }
        }
    }

    private void ActivateTurret() {
        hasSpawned = true;
        rotationX = transform.rotation.x;
    }

    public void PickUp(bool pickUp) { hasBeenPicked = pickUp; }

    public void PlaySound(string type)
    {
        switch(type)
        {
            case "PickUp": AudioSource.PlayClipAtPoint(PickUpSounds[Random.Range(0, 10)], transform.position); break;
            default: AudioSource.PlayClipAtPoint(DeathSounds[Random.Range(0, 7)], transform.position); break;
        }
    }

    void StartParticleSystem()
    {
        ExplosionSpawn = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        ExplosionSpawn.GetComponent<ParticleSystem>().Play();
        StartCoroutine(StopParticleSystem(ExplosionSpawn, 1));
    }

    IEnumerator StopParticleSystem(GameObject particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
        ExplosionSpawn.GetComponent<ParticleSystem>().Stop();
    }
}
