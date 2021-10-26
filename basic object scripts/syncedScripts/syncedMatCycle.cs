using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedMatCycle : UdonSharpBehaviour
{

    public Material[] mats;
    [UdonSynced] public int matIndex = 0;
    public GameObject[] matAssignables;

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "cycleMats");//interact by owner is broadcasted
    }

    public void cycleMats()
    {
		for (int i = 0; i < matAssignables.Length; i++)
		{
			matAssignables[i].GetComponent<Renderer>().material = mats[matIndex];
		}
		matIndex++;
		if (matIndex >= mats.Length)//wrap material index
		{
			matIndex = 0;
		}
    }
}