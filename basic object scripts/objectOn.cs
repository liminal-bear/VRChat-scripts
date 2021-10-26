using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//toggles multiple objects on at once

public class objectOn : UdonSharpBehaviour
{
    public GameObject[] toggleables;

    public override void Interact()
    {
	    for(int i = 0; i < toggleables.Length; i++)
	    {
		    toggleables[i].SetActive(true);
	    }
    }
}