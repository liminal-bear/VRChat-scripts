using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//toggles multiple objects off at once

public class objectOff : UdonSharpBehaviour
{
    public GameObject[] toggleables;

    public override void Interact()
    {
	    for(int i = 0; i < toggleables.Length; i++)
	    {
		    toggleables[i].SetActive(false);
	    }
    }
}