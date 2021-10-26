using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedObjectOn : UdonSharpBehaviour
{
    //syncedObjectOn does not have new player join sync

    public GameObject[] toggleables;
    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "activateTargets");//interact by owner is broadcasted
    }
    public void activateTargets()
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(true);
        }
    }
}