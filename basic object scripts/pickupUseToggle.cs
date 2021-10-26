
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//exact same as a regular toggle, but this script will be attached on a pickubale/usable gadget
public class pickupUseToggle : UdonSharpBehaviour
{
    public GameObject[] toggleables;
    public override void OnPickupUseDown()
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(!toggleables[i].activeSelf);
        }
    }
}
