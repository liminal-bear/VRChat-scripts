using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedMatAssign : UdonSharpBehaviour
{
    public Material mat;
    public GameObject[] matAssignables;

    //not sure why this Start() method is used, but the code is preseved just in case

    //void Start()
    //{
    //    for (int i = 0; i < matAssignables.Length; i++)
    //    {
    //        matAssignables[i].GetComponent<Renderer>().material = mat;
    //    }
    //}

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "matAssignTargets");//interact by owner is broadcasted
    }
    public void matAssignTargets()
    {
        for (int i = 0; i < matAssignables.Length; i++)
        {
            matAssignables[i].GetComponent<Renderer>().material = mat;
        }
    }
}