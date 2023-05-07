using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class InteractTeleport : UdonSharpBehaviour
{
	public GameObject Target;
	private VRCPlayerApi localPlayer;
	public float teleportOffset = 0f;
	public float rotationOffset = 0f;

	public float rotationDiff;

	// Start is called before the first frame update
	void Start()
	{

	}
	public override void Interact()
	{
        localPlayer = Networking.LocalPlayer;

		Vector3 portalToPlayer = localPlayer.GetPosition() - transform.position;
		rotationDiff = -Quaternion.Angle(this.gameObject.transform.rotation, Target.transform.rotation);
		rotationDiff += rotationOffset;

		Vector3 curVelocity = localPlayer.GetVelocity();
		curVelocity = Quaternion.AngleAxis(rotationDiff, Vector3.up) * curVelocity;

        float yOffset = (localPlayer.GetPosition() - transform.position).y;

        //teleport
        localPlayer.TeleportTo(Target.gameObject.transform.position + new Vector3(0, yOffset, 0), Target.transform.rotation);
        localPlayer.SetVelocity(curVelocity);
    }
}