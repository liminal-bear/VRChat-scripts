using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedObjectOff : UdonSharpBehaviour
{
    public GameObject[] toggleables;

    //syncedObjectOff does not have new player join sync

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "deActivateTargets");//interact by owner is broadcasted
    }
    public void deActivateTargets()
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(false);
        }
    }
}