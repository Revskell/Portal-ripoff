using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cake : MonoBehaviour
{
    [SerializeField] private Text winText = null;
    [SerializeField] private Image orangePortal = null;
    [SerializeField] private Image bluePortal = null;

    private bool inRange;
    private bool won;

    private void Start()
    {
        inRange = false;
        won = false;
        winText.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(!won)
        {
            if (inRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<AudioSource>().Play();
                    winText.gameObject.SetActive(true);
                    orangePortal.gameObject.SetActive(false);
                    bluePortal.gameObject.SetActive(false);
                    won = true;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Application.Quit();
            }
        }
    }

    private void OnTriggerEnter(Collider other) { if (other.tag == "Player") inRange = true; }
    private void OnTriggerExit(Collider other) { if (other.tag == "Player") inRange = false; }
}
