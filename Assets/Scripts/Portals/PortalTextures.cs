using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextures : MonoBehaviour
{

    [SerializeField] private Camera otherCam = null;
    [SerializeField] private Material otherMat = null;

    void Start()
    {
        if (otherCam.targetTexture != null) otherCam.targetTexture.Release();
        otherCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        otherMat.mainTexture = otherCam.targetTexture;
    }
}
