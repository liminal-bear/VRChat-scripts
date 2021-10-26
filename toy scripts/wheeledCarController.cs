
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class wheeledCarController : UdonSharpBehaviour
{
    private VRCPlayerApi localPlayer;
    private bool inVR = false;
    private bool usePlayer = true;//debug parameter, change this to control in an editor setting, if false, the gameObject this script is placed on will be referenced as if it was a VR controller
    private Rigidbody carBody; //rigidbody of car object, used for actual movement
    private Transform carTransform;

    public GameObject carObject; //car to move
    public GameObject exaust; //exaust emitter

    public WheelCollider wheelFL;//wheels
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    /*public GameObject debugObject;
	public GameObject debugObject2;
	public GameObject debugObject3;*/

    public float maxSpeed = 1f;//input constraints, modify these to change overall maneuverability / sensitivity
    public float maxTurn = 1f;

    public float maxSpeedTorque = 20f;//wheelcollider constraints
    public float maxTurnAngle = 30f;//degrees
    public float brakeTorque = 100f;

    //speed and turn input values, obtained by doing calculations from user's arrow keys, or controller position
    private float speedInput = 0;
    private float turnInput = 0;

    private float steerAngle;
    private float torque;

    [UdonSynced] public bool driving;
    float curVelocity;


    //the 'zero position' of a user's VR controller
    //without this, the system would only work if the user was facing one direction and standing upright
    //   and would fail in cases where the user was facing backwards, or lying on their back
    private Quaternion joystickZeroPoint;

    //pitch and yaw of a VR user's controller (right hand)
    //this is joystickZeroPoint corrected
    public float pitch;
    public float yaw;

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        if (localPlayer == null) { usePlayer = false; }//fixedupdate runs before this happens and causes a crashin the editor until vrc fix it
        else if (localPlayer.IsUserInVR())
        { 
            inVR = true; 
        }

        carBody = carObject.GetComponent<Rigidbody>();
        carTransform = carObject.GetComponent<Transform>();
    }

	void FixedUpdate()
	{
        //you can see individual joystick stuff in action if you re enable this code and fill in debugObjects
/*        debugObject.transform.eulerAngles = transform.rotation.eulerAngles;
        debugObject2.transform.eulerAngles = joystickDifference.eulerAngles;
        debugObject3.transform.eulerAngles = joystickZeroPoint.eulerAngles;*/


        if (driving)//when the user is in drive mode, get the input, and move the car accordingly
        {
            GetInput();
            Drive();
        }
        else
        {
            Stop();//stops the car, allowing standard rigidbody physics to take over
        }

	}

    void Stop()
    {
        exaust.SetActive(false);

        //bring car to stop, as if was still driving, but all inputs stopped, (dont want people to abuse exiting drive mode for instant stop)
        //  "drift to a stop"
        turnInput /= 1.05f;// use /= to cut down turn bit by bit
        if (Mathf.Abs(turnInput) < .01f)//to avoid dumb small decimals like turnInput = 0.002041304132, just chop it at a threshold
        {
            turnInput = 0;
        }
        speedInput /= 1.05f;//cuts down speed the same way
        if (Mathf.Abs(speedInput) < .01f)//chop speedInput at threshold same as turnInput
        {
            speedInput = 0;
        }
        if (usePlayer)
        {
            joystickZeroPoint = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation;//rotation of the controller relative to the plane when it was pressed
        }
        else
        {
            joystickZeroPoint = transform.rotation;//in-editor way of controlling things
        }
        wheelFL.steerAngle = 0;
        wheelFR.steerAngle = 0;
    }
    
    void Drive()
    {
        //debugObject.transform.eulerAngles = transform.rotation.eulerAngles;

        curVelocity = carTransform.InverseTransformDirection(carBody.velocity).z;

        exaust.SetActive(true);

        steerAngle = turnInput * maxTurn;

        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;

        torque = maxSpeedTorque * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);

        if (Mathf.Sign(curVelocity) != Mathf.Sign(speedInput))//if you swap to the opposite direction, brake first
        {
            wheelRL.brakeTorque = brakeTorque;
            wheelRR.brakeTorque = brakeTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
            wheelRR.motorTorque = torque;
            wheelRL.motorTorque = torque;
        }
        

    }

    void GetInput()
    {
        if (usePlayer)
        {
            if (inVR)
            {
                Vector3 controllerForward = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation * Vector3.forward;
                Vector3 zeroDown = joystickZeroPoint * Vector3.down;
                Vector3 zeroRight = joystickZeroPoint * Vector3.right;
                //for some reason, vrchat makes the palm (left of controller, when held vertically) be considered down, rather than left (rather than like the conventional plane joystick)
                float absPitch = Vector3.Angle(zeroRight, controllerForward); //get angle of controller between it and facing straight right (between 0 and 180)
                float absYaw = Vector3.Angle(zeroDown, controllerForward); //same as before, but with the reference facing down
                pitch = absPitch - 90;
                yaw = absYaw - 90;

                pitch = -Mathf.Clamp(pitch, -45, 45); //clamp pitch/yaw to +/-45 degreess (because who da heck would want to rotate their wrist 180 degrees?)
                                                      //also negate pitch, because controller down means forward
                                                      //inverted controlls are not supported
                yaw = Mathf.Clamp(yaw, -45, 45);

                pitch /= 45; //convert values to decimal (a fraction between -45/45, 0/45, and 45/45 (-1 ~ 1))
                yaw /= 45;

                speedInput = maxSpeed * pitch;
                turnInput = yaw;
            }
            else
            {
                //Desktop controls are relativley simple compared to VR
                float left = Input.GetKey(KeyCode.LeftArrow) ? -.05f : 0;
                float right = Input.GetKey(KeyCode.RightArrow) ? .05f : 0;
                float forward = Input.GetKey(KeyCode.UpArrow) ? .05f : 0;
                float back = Input.GetKey(KeyCode.DownArrow) ? -.05f : 0;

                turnInput += left + right;
                turnInput = Mathf.Clamp(turnInput, -maxTurn, maxTurn);
                if(left + right == 0)
                {
                    turnInput /= 1.05f;
                    if (Mathf.Abs(turnInput) < .01f)
                    {
                        turnInput = 0;
                    }
                }

                speedInput += forward + back;
                speedInput = Mathf.Clamp(speedInput, -maxSpeed, maxSpeed);
                if (forward + back == 0)
                {
                    speedInput /= 1.05f;
                    if (Mathf.Abs(speedInput) < .01f)
                    {
                        speedInput = 0;
                    }
                }
            }
        }
        else //this is the case when we want to controll the cars in an editor setting (similar to VR controlls, but with 'this' being the controller referenced)
        {
            Vector3 controllerForward = transform.rotation * Vector3.forward;
            Vector3 zeroDown = joystickZeroPoint * Vector3.down;
            Vector3 zeroRight = joystickZeroPoint * Vector3.right;
            float absYaw = Vector3.Angle(zeroDown, controllerForward); //get angle of controller between it and facing straight down (between 0 and 180)
            float absPitch = Vector3.Angle(zeroRight, controllerForward); //same as before, but with the reference facing right
            pitch = absPitch - 90;
            yaw = absYaw - 90;

            pitch = -Mathf.Clamp(pitch, -45, 45); //clamp pitch/yaw to +/-45 degreess (because who da heck would want to rotate their wrist 180 degrees?)
                                                  //also negate pitch, because controller down means forward
            yaw = -Mathf.Clamp(yaw, -45, 45);

            pitch = pitch / 45; //convert values to decimal
            yaw = yaw / 45;

            speedInput = maxSpeed * pitch;
            turnInput = yaw;
        }
    }

	public override void OnPickupUseDown()
    {
        driving = true;
    }
    public override void OnPickupUseUp()
    {
        driving = false;
    }
}