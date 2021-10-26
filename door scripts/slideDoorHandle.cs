
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;
using System.Collections.Generic;

public class slideDoorHandle : UdonSharpBehaviour
{
    public GameObject door;

    public string direc = "forward";//user defined direction
    public float amount = 0.4f;
    public float speed = 5f;

    private Vector3 axis = Vector3.forward;//exactly the same as direc, but expressed as Vector3
    private Vector3 start;//open position
    private Vector3 target;//close postion
    private Vector3 curPos;//current valid position (either fully start, or fully target)
    private Vector3 destination;//the other position (for instance, if curPos is start, destination would be target)
    private float fractionToDestination = 0;//indicator to how close the door is to ending its move sequence

    [UdonSynced] public bool isOpen = false;

    void Start()
    {
        start = door.transform.position;
        switch (direc)
        {
            case "forward":
                axis = Vector3.forward;
                break;
            case "backward":
                axis = Vector3.back;
                break;
            case "left":
                axis = Vector3.left;
                break;
            case "right":
                axis = Vector3.right;
                break;
            case "up":
                axis = Vector3.up;
                break;
            case "down":
                axis = Vector3.down;
                break;
            default:

                break;
        }
        target = door.transform.position + axis * amount;
        destination = start;
        curPos = start; //the only time destination and curPos are going to be equal is if the game starts and the door hasn't gone through a lerp process yet
    }
	private void Update()
	{
        if (fractionToDestination < 1)//if still not close enough to final position
        {
            fractionToDestination += Time.deltaTime * speed;
            door.transform.position = Vector3.Lerp(curPos, destination, fractionToDestination);
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
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ToggleDoor");//interact by use is broadcasted
    }

    public void ToggleDoor()//interchanges the curPos and destination, based off of isOpen, this changes is acted upon by the lerp
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

