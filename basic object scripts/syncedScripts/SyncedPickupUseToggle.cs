using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//exact same as syncedToggle, see that for full explained code
//this is just the pickubable-use script counterpart

public class SyncedPickupUseToggle : UdonSharpBehaviour
{
    public GameObject[] toggleables;
    [UdonSynced] bool[] activeStates;

    // Start is called before the first frame update
    void Start()
    {
        activeStates = new bool[toggleables.Length];//host's account of all object statuses
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.IsMaster)
        {
            for (int i = 0; i < toggleables.Length; i++)
            {
                activeStates[i] = toggleables[i].activeSelf;
            }
        }
        ToggleTargetsSpecified(activeStates);//this does nothing, except for the new player, who isn't synced up yet
    }

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ToggleTargets");//interact by owner is broadcasted
    }
    public void ToggleTargets()
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(!toggleables[i].activeSelf);
        }
    }
    public void ToggleTargetsSpecified(bool[] activeStates)
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(activeStates[i]);
        }
    }

}