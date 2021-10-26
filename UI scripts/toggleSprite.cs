
using UdonSharp;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//used to swap between 2 sprites, 'active' and 'idle'

public class toggleSprite : UdonSharpBehaviour
{
    public Image buttonImage;
    public Sprite idleSprite;
    public Sprite ActiveSprite;
    private bool isActive = false;

    public override void Interact()
    {
        if (isActive)
        {
            isActive = false;
            buttonImage.sprite = idleSprite;
        }
        else
        {
            isActive = true;
            buttonImage.sprite = ActiveSprite;
        }
    }
}
