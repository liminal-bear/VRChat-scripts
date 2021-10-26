
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;
using System.Collections.Generic;

//this script is used to rotate a door based off of user interaction

//a door is made up of 3 components, the hinge, the doorObject, and the handle.
//hinge & object are explained below
//handle is 'this', the object you will place the script upon. It is best to have handle and the doorObject be seperate, because you will probably want a dedicated small collider for the handle to interact upon

//a slideDoor does not need a hinge, btw

public class rotatorDoorHandle : UdonSharpBehaviour
{
    public GameObject doorPivot;//the 'hinge' of the door. this is seperate from the actual doorObject, because if they were the same, the door would only rotate about its center, not a hinge
    public GameObject doorObject;//the thing that gets rotated around the pivot

    public float angle = 90f;
    public float speed = 3f;
    public char axis = 'y';
    public bool disableColliderWhileRotating = true;//ussually set to true. don't wanna get slapped in the face with the door, better for it to just phase through you

    private Quaternion target;//open rotation
    private Quaternion start;//original rotation
    private Quaternion curPos;//most recent rotation
    private Quaternion destination;//the other rotation to go towards (if curPos is start, then destination will be target)
    private float fractionToDestination = 0;//indicator to how close the door is to ending its move sequence

    [UdonSynced] public bool isOpen = false;

    void Start()
    {
        start = doorPivot.transform.rotation;

        Quaternion offset;

        switch(axis)
        {
            case 'x':
                offset = Quaternion.Euler(angle, 0, 0);
                break;
            case 'y':
                offset = Quaternion.Euler(0, angle, 0);
                break;
            case 'z':
                offset = Quaternion.Euler(0, 0, angle);
                break;
            default://if invalid char is given, just rotate around y (most reasonable door behavior)
                offset = Quaternion.Euler(0, angle, 0);
                break;
        }
        target = doorPivot.transform.rotation * offset;//calculates the open rotation
        destination = start;
        curPos = start; //the only time destination and curPos are going to be equal is if the game starts and the door hasn't gone through a lerp process yet
    }
	private void Update()
	{

        if (fractionToDestination < 1)//if still not close enough to final rotation
        {
            if (disableColliderWhileRotating && (doorObject.GetComponent<Collider>() != null))//disables door collider when rotating
            {
                doorObject.GetComponent<Collider>().enabled = false;
            }
            fractionToDestination += Time.deltaTime * speed;
            doorPivot.transform.rotation = Quaternion.Slerp(curPos, destination, fractionToDestination);
        }
        else
        {
            if (doorObject.GetComponent<Collider>() != null)//enables door collider when it's not rotating, this doesn't do anything if there is no collider
            {
                doorObject.GetComponent<Collider>().enabled = true;
            }
        }
    }
	public override void OnPlayerJoined(VRCPlayerApi player)//used to sync new user with host's door state
	{
		if (Networking.IsMaster)
		{
			if (isOpen)
			{
				SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "doorOpen");
			}
			else
			{
				SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "doorClose");
			}
		}
	}

	public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ToggleDoor");//interact by user is broadcasted
    }

    public void ToggleDoor()//interchanges the curPos and destination, based off of isOpen, this changes is acted upon by the slerp
    {
        if (isOpen)
        {
            fractionToDestination = 0;//begin lerping again
            curPos = target;
            destination = start;
            isOpen = false;
        }
        else
        {
            fractionToDestination = 0;
            curPos = start;
            destination = target;
            isOpen = true;
        }
    }

	public void doorOpen()
	{
		fractionToDestination = 0;
		curPos = start;
		destination = target;
	}

	public void doorClose()
	{
		fractionToDestination = 0;//begin lerping again
		curPos = target;
		destination = start;
	}

}

