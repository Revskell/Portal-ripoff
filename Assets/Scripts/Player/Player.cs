using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private Text deathText = null;
    [SerializeField] private Image orangePortal = null;
    [SerializeField] private Image bluePortal = null;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound = null;

    public static Player instance;
    private bool dead;

    private void Awake()
    {
        if(instance != null && instance != this) Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        dead = false;
        deathText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if(dead)
        {
            if (Input.GetKeyDown(KeyCode.F)) Respawn(Checkpoints.GetLastCheckpoint());
        }
    }

    public void Die()
    {
        if (!dead)
        {
            // Time.timeScale = 0f;
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            deathText.gameObject.SetActive(true);
            orangePortal.gameObject.SetActive(false);
            bluePortal.gameObject.SetActive(false);
            dead = true;
        }
    }

    private void Respawn(Vector3 newPos)
    {
        CharacterController cc = GetComponentInChildren<CharacterController>();
        cc.enabled = false;
        transform.GetChild(0).position = newPos;
        transform.parent = null;
        deathText.gameObject.SetActive(false);
        orangePortal.gameObject.SetActive(true);
        bluePortal.gameObject.SetActive(true);
        dead = false;
        cc.enabled = true;
        // Time.timeScale = 1f;
    }

    public bool isDead() { return dead; }
}
