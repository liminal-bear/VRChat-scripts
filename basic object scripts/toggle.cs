using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//unsynced toggling of multiple objects, very, *very* simple

public class toggleObjects: UdonSharpBehaviour
{
    public GameObject[] toggleables;
    public override void Interact()
    {
	    for(int i = 0; i < toggleables.Length; i++)
	    {
		    toggleables[i].SetActive(!toggleables[i].activeSelf);
	    }
    }
}