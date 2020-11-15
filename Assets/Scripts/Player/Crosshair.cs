using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Image orangeImage = null;
    [SerializeField] private Image blueImage = null;
    [SerializeField] private Sprite orangeOff = null;
    [SerializeField] private Sprite orangeOn = null;
    [SerializeField] private Sprite blueOff = null;
    [SerializeField] private Sprite blueOn = null;

    public void SetPortal(bool orangeOrBlue, bool on)
    {
        if (orangeOrBlue) orangeImage.sprite = on ? orangeOn : orangeOff;
        else blueImage.sprite = on ? blueOn : blueOff;
    }

    public void Scale(float val)
    {
        orangeImage.rectTransform.localScale = new Vector3(val, val, val);
        blueImage.rectTransform.localScale = new Vector3(val, val, val);
    }
}
