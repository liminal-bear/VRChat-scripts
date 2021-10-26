using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//used to teleport a player to a target position upon interact

public class buttonTeleportudon : UdonSharpBehaviour
{
    public Transform Target;
    private VRCPlayerApi Player;

    // Start is called before the first frame update
    void Start()
    {
	    Player = Networking.LocalPlayer;        
    }       

    public override void Interact()
    {
	    Player.TeleportTo(Target.position, Target.rotation);
    }
}