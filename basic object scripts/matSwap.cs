using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//assigns a new material onto an object on interaction
//however, it will also swap back to the original material upon re-interaction
//if an object has multiple material slots, Material Reference[0] will be assigned
//  if wholeSwap = true, all material slots will be assigned

public class matSwap : UdonSharpBehaviour
{
    public Material mat;
    public GameObject[] matAssignables;
    public bool wholeSwap = true;
    private Material[][] originalMats;
    public bool isOn = false;//keeps track regarding whether or not the new material is assigned or not

    private void Start()
    {
        originalMats = new Material[matAssignables.Length][];

        for (int i = 0; i < matAssignables.Length; i++)
        {
            if (!wholeSwap)
            {
                originalMats[i][0] = matAssignables[i].GetComponent<Renderer>().material;
            }
            else
            {
                Material[] subMats = matAssignables[i].GetComponent<Renderer>().sharedMaterials;
                originalMats[i] = new Material[subMats.Length];
                for (int j = 0; j < subMats.Length; j++)
                {
                    originalMats[i][j] = subMats[j];
                }
            }
        }
    }
    public override void Interact()
    {
        if (!isOn)
        {
            for (int i = 0; i < matAssignables.Length; i++)
            {
                if (!wholeSwap)
                {
                    matAssignables[i].GetComponent<Renderer>().material = mat;
                }
                else
                {
                    Material[] newSubMats = matAssignables[i].GetComponent<Renderer>().sharedMaterials;
                    for (int j = 0; j < newSubMats.Length; j++)
                    {
                        newSubMats[j] = mat;
                    }
                    matAssignables[i].GetComponent<Renderer>().sharedMaterials = newSubMats;
                }
            }
            isOn = true;
        }
        else
        {
            for (int i = 0; i < matAssignables.Length; i++)
            {
                if (!wholeSwap)
                {
                    matAssignables[i].GetComponent<Renderer>().material = originalMats[i][0];
                }
                else
                {
                    matAssignables[i].GetComponent<Renderer>().sharedMaterials = originalMats[i];
                }
            }
            isOn = false;
        }
    }
}