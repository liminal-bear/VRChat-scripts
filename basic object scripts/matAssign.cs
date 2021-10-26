using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//assigns a new material onto an object on interaction
//if an object has multiple material slots, Material Reference[0] will be assigned

public class toggle : UdonSharpBehaviour
{
    public Material mat;
    public GameObject[] matAssignables;

    public override void Interact()
    {
        for (int i = 0; i < matAssignables.Length; i++)
        {
            matAssignables[i].GetComponent<Renderer>().material = mat;
        }
    }
}