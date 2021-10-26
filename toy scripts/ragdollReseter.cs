
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//used to reset a ragdoll in a stable manner, without rubber banding/slingshotting around
//this is done by using a single bone to judge when to reset, and will properly handle physics
//the bone that judges the reset is 'this' object (the object this script is placed on)
//the other bones are supplied by the user

//NOTE: respawnPos must be higher than the world respawn position
//NOTE: 'this' must also be included in the bones array


public class ragdollReseter : UdonSharpBehaviour
{
    public GameObject[] bones;
    private Vector3[] initPos;
    private Quaternion[] initRot;

    public float respawnPos = -20f;

    void Start()
    {
        initPos = new Vector3[bones.Length];
        initRot = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            initPos[i] = bones[i].transform.position;
            initRot[i] = bones[i].transform.rotation;
        }
    }
    private void Update()
    {
        if (this.transform.position.y < respawnPos)
        {
            resetBones();
        }
    }
    private void resetBones()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].GetComponent<Rigidbody>().isKinematic = false;//disable bone physics before reset
        }

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].transform.position = initPos[i];
            bones[i].transform.eulerAngles = initRot[i].eulerAngles;
            if (bones[i].GetComponent<Rigidbody>() != null)
            {
                bones[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                bones[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].GetComponent<Rigidbody>().isKinematic = true;//enable bone physics after reset
        }
    }   
}

