private var motor : CharacterMotor;
private var playerInputSuffix : String;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

function Start()
{
	// Determine which controller we should be receiving input from
	playerInputSuffix = ""; // default to player 1
	if(gameObject.tag == "Player 1")
		playerInputSuffix = "";
	else if(gameObject.tag == "Player 2")
		playerInputSuffix = " P2";
	else if(gameObject.tag == "Player 3")
		playerInputSuffix = " P3";
	else if(gameObject.tag == "Player 4")
		playerInputSuffix = " P4";
}

// Update is called once per frame
function Update () {
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
