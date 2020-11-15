using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private float rotationX = 0f;
    private Vector3 forward;
    private bool dead = false;
    private bool hasSpawned = false;
    private bool hasBeenPicked = false;
    private GameObject ExplosionSpawn;
    private LineRenderer lineRenderer = null;

    [SerializeField] public GameObject ExplosionEffect = null;
    [SerializeField] public GameObject Light = null;
    [SerializeField] public GameObject Line = null;
    [SerializeField] public Transform LinePoint = null;
    [SerializeField] public Player player = null;

    [Header("Sounds")]
    [SerializeField] public List<AudioClip> DeathSounds = new List<AudioClip>();
    [SerializeField] public List<AudioClip> PickUpSounds = new List<AudioClip>();
    [SerializeField] public List<AudioClip> ActiveSounds = new List<AudioClip>();
    [SerializeField] public List<AudioClip> ShootSounds = new List<AudioClip>();

    void Start()
    {
        Invoke("ActivateTurret", 1);
        lineRenderer = Line.GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        lineRenderer.SetPosition(0, LinePoint.position - transform.position - new Vector3(0, 0.9f));
        if(hasSpawned)
        {
            if (hasBeenPicked) Line.SetActive(false);
            else if (!dead)
            {
                if (transform.rotation.x != rotationX)
                {
                    PlaySound("Death");
                    StartParticleSystem();
                    Light.SetActive(false);
                    dead = true;
                    Line.SetActive(false);
                }
                else if (Physics.Raycast(LinePoint.position, LinePoint.forward, out RaycastHit hit))
                {
                    if (hit.collider) lineRenderer.SetPosition(1, hit.point - transform.position - new Vector3(0, 0.9f));
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        if(!player.isDead())
                        {
                            PlaySound("Shoot");
                            player.Die();
                        }
                    }
                }
                else lineRenderer.SetPosition(1, (LinePoint.position - transform.position - new Vector3(0, 0.9f)) + (LinePoint.forward * 100000));
            }
        }
    }

    private void ActivateTurret() {
        hasSpawned = true;
        rotationX = transform.rotation.x;
    }

    public void PickUp(bool pickUp) { hasBeenPicked = pickUp; }

    public bool isDead() { return dead; }

    public void PlaySound(string type)
    {
        switch(type)
        {
            case "PickUp": AudioSource.PlayClipAtPoint(PickUpSounds[Random.Range(0, 10)], transform.position); break;
            case "Active": AudioSource.PlayClipAtPoint(ActiveSounds[Random.Range(0, 8)], transform.position); break;
            case "Shoot": AudioSource.PlayClipAtPoint(ShootSounds[Random.Range(0, 3)], transform.position); break;
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
        Destroy(ExplosionSpawn);
    }
}
