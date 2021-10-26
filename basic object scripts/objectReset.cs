using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//resets the position, rotation (and velocity, if necessary) of multiple gameObjects
//this script has no dedicated synced counterpart, as the individual object's VRC Object sync components already handle position, rotation, and velocity

public class objectReset : UdonSharpBehaviour
{
    public GameObject[] objects;
    private Vector3[] initPos;//initial position to reset back to
    private Quaternion[] initRot;//initial rotation to reset back to
    // Start is called before the first frame update
    void Start()
    {
        initPos = new Vector3[objects.Length];
        initRot = new Quaternion[objects.Length];

        for (int i = 0; i < objects.Length; i++)//records the initial positions and rotations at the starting frame
        {
            initPos[i] = objects[i].transform.position;
            initRot[i] = objects[i].transform.rotation;
        }
    }

	public override void Interact()
    {
	    for(int i = 0; i < objects.Length; i++)
	    {
            objects[i].transform.position = initPos[i];
            objects[i].transform.eulerAngles = initRot[i].eulerAngles;
            if (objects[i].GetComponent<Rigidbody>() != null)//if the object is a rigid body, the velocity needs to be reset. (resetting an object at 100m/s if futile without this)
            {
                objects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                objects[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}