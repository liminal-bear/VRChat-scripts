
using UdonSharp;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//TabButton is a wrapper that contains references to the button's image, it's parent TabManager, the page it will open, and what index it is in the entire tab setup

//NOTE: tabIndex is not auto organizing, you must fill this in yourself
//individual buttons have no idea where they are in the manager's tabButton array, hence why it must be user supplied

//code inspired by https://www.youtube.com/watch?v=211t6r12XPQ

[RequireComponent(typeof(Image))]
public class TabButton : UdonSharpBehaviour
{
    public Image image;
    public TabManager manager;
    public GameObject page;//this will show the gameObject that is the actual content desired
    public int tabIndex;                    

    private bool isActive = false;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public override void Interact()//on interacted, notify the manager that this particular tab has been selected
    {
        manager.OnTabSelected(this);
    }
}
