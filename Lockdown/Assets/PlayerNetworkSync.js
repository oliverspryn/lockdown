#pragma strict

private var charMotor : CharacterMotor;

function Start () {
	charMotor = gameObject.GetComponent("CharacterMotor") as CharacterMotor;
}

function Update () {

}

function OnSerializeNetworkView(stream : BitStream, info : NetworkMessageInfo) : void
{
	var myVel : Vector3 = Vector3.zero;
	if(stream.isWriting) // send data
	{
		myVel = charMotor.movement.velocity;
		stream.Serialize(myVel);
	}
	else // receive data
	{
		stream.Serialize(myVel);
		if(charMotor != null) charMotor.movement.velocity = myVel;
	}
}