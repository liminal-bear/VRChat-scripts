
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//this rotates a gameobject to be directly pointed toward a player's (specifically yours) head bone (effectively staring into your eyes)
//it's very creepy, trust me
//if the rotation is offset by some amount, just place it under a dummy parent object

public class FacePlayerHead : UdonSharpBehaviour
{
	public float speed = 1.0f;

	private VRCPlayerApi localPlayer;
	private Vector3 targetDirection;
	void Start()
    {
		localPlayer = Networking.LocalPlayer;
	}	
	private void FixedUpdate()
	{
		if (localPlayer != null)
		{
			targetDirection = localPlayer.GetBonePosition(HumanBodyBones.Head) - transform.position;//gets the direction that needs to be faced in order to point to head bone

			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);//gradually rotate to that direction
			transform.rotation = Quaternion.LookRotation(newDirection);
		}
	}
}
