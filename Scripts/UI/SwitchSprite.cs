using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite spriteStart;
    [SerializeField] private Sprite spriteSwitch;

    public void Switch(bool isSwitch)
    {
        image.sprite = isSwitch ? spriteSwitch : spriteStart;
    }
}
