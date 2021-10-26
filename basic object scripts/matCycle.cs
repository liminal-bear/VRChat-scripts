using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//this is the exact same as matAssign, but assigns from an array of possible materials, cycled through by the user
//there is a pickup-use script, there is no Interact() based script yet

public class matCycle : UdonSharpBehaviour
{

    public Material[] mats;
    public int matIndex = 0;
    public GameObject[] matAssignables;

    public override void OnPickupUseDown()
    {
        for (int i = 0; i < matAssignables.Length; i++)
        {
            matAssignables[i].GetComponent<Renderer>().material = mats[matIndex];
        }
        matIndex++;
        if (matIndex >= mats.Length)//overflows mat index back to start of the array
        {
            matIndex = 0;
        }
    }
}