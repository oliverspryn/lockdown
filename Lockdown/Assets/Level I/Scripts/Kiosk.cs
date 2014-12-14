using UnityEngine;

/// <summary>
/// Manages the kiosk to open the prison doors.
/// </summary>
public class Kiosk : MonoBehaviour {
	#region Fields

/// <summary>
/// The name of the Unity button to use to activate the
/// kiosk.
/// </summary>
	public string ActivateButton;

/// <summary>
/// The button to display when showing the user how to interact with
/// this kiosk.
/// </summary>
	public Buttons ButtonDisplay;

/// <summary>
/// A reference to the button HUD, to display directinos to the user
/// when he or she comes near this kiosk.
/// </summary>
	public GameObject ButtonHUD;

/// <summary>
/// The object with which the user will collide in order to
/// activate a close enough range within the kiosk.
/// </summary>
	public Handheld Collider;

/// <summary>
/// The text to display when the user approaches the kiosk.
/// </summary>
	public string Directions;

/// <summary>
/// The jail doors which need to be opened when the kiosk is
/// activated.
/// </summary>
	public GameObject[] JailDoors;

/// <summary>
/// The screen to display when the kiosk is activated.
/// </summary>
	public GameObject ScreenActivated;

/// <summary>
/// The screen to display when the kiosk is deactivated.
/// </summary>
	public GameObject ScreenDeactivated;

	#endregion

	#region Private Members

/// <summary>
/// Whether or not the kiosk has been activated.
/// </summary>
	private bool Activated = false;

/// <summary>
/// The script attached to the button HUD.
/// </summary>
	private ButtonHUD HUDScript;

/// <summary>
/// Whether or not the player is close enough to the kiosk to
/// activate it.
/// </summary>
	private bool Near = false;

	#endregion

	#region Constructors

/// <summary>
/// Get the script attached to the button HUD.
/// </summary>
	public void Start() {
		HUDScript = ButtonHUD.GetComponent<ButtonHUD>();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Listen for button presses to activate the kiosk and open the
/// doors.
/// </summary>
	public void Update() {
		if(!Activated && Near && Input.GetButtonDown(ActivateButton)) {
			foreach(GameObject door in JailDoors) {
				door.GetComponent<JailDoor>().Open();
			}

			ScreenActivated.SetActive(true);
			ScreenDeactivated.SetActive(false);
			HUDScript.Active = false;
			Activated = true;
		}
	}

	#endregion

	#region Private Methods

/// <summary>
/// Triggered by Unity whenever a player has walked near the kiosk.
/// </summary>
/// 
/// <param name="c">The collider object which triggered the event</param>
	private void OnTriggerEnter(Collider c) {
		if(!Activated) {
			HUDScript.Active = true;
			HUDScript.Button = ButtonDisplay;
			HUDScript.Text = Directions;
			Near = true;
		}
	}

/// <summary>
/// Triggered by Unity whenever a player has walked away from the
/// kiosk.
/// </summary>
/// 
/// <param name="c">The collider object which triggered the event</param>
	private void OnTriggerExit(Collider c) {
		if(!Activated) {
			Near = false;
		}
	}

	#endregion
}