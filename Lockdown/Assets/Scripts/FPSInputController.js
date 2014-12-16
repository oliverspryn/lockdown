private var motor : CharacterMotor;
private var playerInputSuffix : String;
private var networkingOn = false;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

function Start()
{
	// Determine which controller we should be receiving input from
	/*var netOnOffFoobarThing : GameObject = GameObject.Find ("Network OnOff Foobar Thing");
	// Yes, I'm using a dummy GameObject's active state as a boolean. It's the only way I
	// could think of to get a "truly global" boolean value that's accessible across
	// scripts in different assemblies, e.g., scripts on "Standard Assets" in the "firstpass"
	// assemblies, and NetworkManager in the main assembly. The purpose of this object is
	// to signal whether we're in "networked/online" or "offline" mode.
	if(netOnOffFoobarThing != null && netOnOffFoobarThing.activeInHierarchy)
		networkingOn = true;*/
	//if(LockdownGlobals.Instance.networkingOn)
		networkingOn = true;
	
	playerInputSuffix = " P1"; // default to player 1
	if(gameObject.tag == "Player 1")
	{
		// If we're a network client, P1 and P2 are over on the server, so we should ignore input
		// on their respective axes - those are being used for P3 and P4.
		if(networkingOn && Network.isClient)
			playerInputSuffix = "NULL AXIS THAT MOST ABSOLUTELY DOES NOT EXIST";
		else
			playerInputSuffix = " P1";
	}
	else if(gameObject.tag == "Player 2")
	{
		if(networkingOn && Network.isClient)
			playerInputSuffix = "NULL AXIS THAT MOST ABSOLUTELY DOES NOT EXIST";
		else
			playerInputSuffix = " P2";
	}
	else if(gameObject.tag == "Player 3")
	{
		// If we're a network client, then P1 and P2 are over on the server, so we want
		// P3 and P4 to be mapped to the first and second controllers.
		if(networkingOn && Network.isClient)
			playerInputSuffix = " P1";
		else
			playerInputSuffix = " P3";
	}
	else if(gameObject.tag == "Player 4")
	{
		if(networkingOn && Network.isClient)
			playerInputSuffix = " P2";
		else
			playerInputSuffix = " P4";
	}
}

// Update is called once per frame
function Update () {
	// Don't apply input if this is a remote player
	if(networkingOn && !networkView.isMine)
		return;

	// Get the input vector from keyboard or analog stick
	var directionVector = new Vector3(Input.GetAxis("Horizontal" + playerInputSuffix), 0, Input.GetAxis("Vertical" + playerInputSuffix));
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	motor.inputJump = Input.GetButton("Jump" + playerInputSuffix);
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")