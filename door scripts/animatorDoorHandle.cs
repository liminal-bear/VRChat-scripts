
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//an animatorDoorHandle is best used for more complex doors with very fancy animation sequences

//NOT SPAM PROOF!!!!!!!!!
//you will probably need to handle spam protection in your individual door's animator controller

public class animatorDoorHandle : UdonSharpBehaviour
{
    public Animator doorAnimator;
    [UdonSynced] public bool isOpen = false;

	public override void OnPlayerJoined(VRCPlayerApi player)
	{
		if (Networking.IsMaster)
		{
			if (doorAnimator.GetBool("isOpen"))
			{
				isOpen = true;
				SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "doorOpen");
			}
			else
			{
				isOpen = false;
				SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "doorClose");
			}
		}
	}

	public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ToggleDoor");//interact by owner is broadcasted
    }

	public void ToggleDoor()
	{
		//we are not using the following line, because it doesn't utilize our synced isOpen variable
		//doorAnimator.SetBool("isOpen", !doorAnimator.GetBool("isOpen"));

		if (isOpen)
		{
			doorAnimator.SetBool("isOpen", false);
			isOpen = false;
		}
		else
		{
			doorAnimator.SetBool("isOpen", true);
			isOpen = true;
		}
	}

	public void doorOpen()
	{
		doorAnimator.SetBool("isOpen", true);
	}

	public void doorClose()
	{
		doorAnimator.SetBool("isOpen", false);
	}
}
