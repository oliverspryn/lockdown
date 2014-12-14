using UnityEngine;

/// <summary>
/// This class is used to show a HUD when the user approaches a
/// specific jail door, and allow that user to open it with a 
/// button press.
/// </summary>
public class RustedJailDoor : MonoBehaviour {
	#region Fields

/// <summary>
/// The button to display when showing the user how to interact with
/// this door.
/// </summary>
	public Buttons ButtonDisplay;

/// <summary>
/// A reference to the button HUD, to display directinos to the user
/// when he or she comes near this door.
/// </summary>
	public GameObject ButtonHUD;

/// <summary>
/// The text to display when the user approaches the door.
/// </summary>
	public string Directions;

/// <summary>
/// The name of the Unity button to use to open the door.
/// </summary>
	public string OpenButton;

	#endregion

	#region Private Members

/// <summary>
/// The script attached to the button HUD.
/// </summary>
	private ButtonHUD HUDScript;

/// <summary>
/// Whether or not the player is close enough to the door to punch
/// it open.
/// </summary>
	private bool Near = false;

/// <summary>
/// The other script attached to this door.
/// </summary>
	private JailDoor JailDoorScript;

	#endregion

	#region Constructors

/// <summary>
/// Get the script attached to the button HUD and the other script
/// attached to this game object.
/// </summary>
	public void Start() {
		HUDScript = ButtonHUD.GetComponent<ButtonHUD>();
		JailDoorScript = gameObject.GetComponent<JailDoor>();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Listen for button presses to open the door.
/// </summary>
	public void Update() {
		if(!JailDoorScript.IsOpen && Near && Input.GetButtonDown(OpenButton)) {
			JailDoorScript.Open();
			HUDScript.Active = false;
		}
	}

	#endregion

	#region Private Methods

/// <summary>
/// Triggered by Unity whenever a player has walked near the door.
/// </summary>
/// 
/// <param name="c">The collider object which triggered the event</param>
	private void OnTriggerEnter(Collider c) {
		if(!JailDoorScript.IsOpen) {
			HUDScript.Active = true;
			HUDScript.Button = ButtonDisplay;
			HUDScript.Text = Directions;
			Near = true;
		}
	}

/// <summary>
/// Triggered by Unity whenever a player has walked away from the
/// door.
/// </summary>
/// 
/// <param name="c">The collider object which triggered the event</param>
	private void OnTriggerExit(Collider c) {
		if(!JailDoorScript.IsOpen) {
			HUDScript.Active = false;
			Near = false;
		}
	}

	#endregion
}