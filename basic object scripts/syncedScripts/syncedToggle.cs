using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//syncedToggle toggles multiple gameObjects on or off
//syncedToggle will sync interact events, and also has new player join sync

public class SyncedToggleObjects : UdonSharpBehaviour
{
    public GameObject[] toggleables;
    [UdonSynced] bool[] activeStates;//this has the host's account of all object statuses

    // Start is called before the first frame update
    void Start()
    {
        activeStates = new bool[toggleables.Length];//instantiates host's account of all object statuses
    }

    public override void OnPlayerJoined(VRCPlayerApi player)//used for new player sync to host's stuff
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

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ToggleTargets");//interact for owner is broadcasted
    }

    //the actual toggling of the objects is still pretty simple
    public void ToggleTargets()
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(!toggleables[i].activeSelf);
        }
    }

    //ToggleTargetsSpecified sets all gameObject's states to either on or off, specified by the bool[] parameter
    public void ToggleTargetsSpecified(bool[] activeStates)
    {
        for (int i = 0; i < toggleables.Length; i++)
        {
            toggleables[i].SetActive(activeStates[i]);
        }
    }

}