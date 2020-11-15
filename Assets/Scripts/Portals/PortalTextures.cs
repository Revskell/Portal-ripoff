using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextures : MonoBehaviour
{

    [SerializeField] private Camera thisCam = null;
    [SerializeField] private Material thisMat = null;

    void Awake()
    {
        if (thisCam.targetTexture != null) thisCam.targetTexture.Release();
        thisCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        thisMat.mainTexture = thisCam.targetTexture;
    }
}
