using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	string playerInputSuffix;

	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		// Determine which controller we should be receiving input from
		GameObject netOnOffFoobarThing = GameObject.Find ("Network OnOff Foobar Thing");
		// Yes, I'm using a dummy GameObject's active state as a boolean. It's the only way I
		// could think of to get a "truly global" boolean value that's accessible across
		// scripts in different assemblies, e.g., scripts on "Standard Assets" in the "firstpass"
		// assemblies, and NetworkManager in the main assembly. The purpose of this object is
		// to signal whether we're in "networked/online" or "offline" mode.
		bool networkingOn = false;
		if(netOnOffFoobarThing.activeInHierarchy)
			networkingOn = true;

		playerInputSuffix = " P1"; // default to player 1
		if(gameObject.tag == "Player 1")
			playerInputSuffix = " P1";
		else if(gameObject.tag == "Player 2")
			playerInputSuffix = " P2";
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

	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X" + playerInputSuffix) * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y" + playerInputSuffix) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X" + playerInputSuffix) * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y" + playerInputSuffix) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
}