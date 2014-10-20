using UnityEngine;

/// <summary>
/// This script manages an alarm by creating a deactivated by
/// default, and providing a function to enabled the alarm lights
/// and sound when provoked.
/// </summary>
public class Alarm : MonoBehaviour {
	#region Fields

/// <summary>
/// The light emitted from the alarm station.
/// </summary>
	public GameObject AlarmLight;

/// <summary>
/// The GameObject which represents the alarm station.
/// </summary>
	public GameObject AlarmStation;

/// <summary>
/// How quickly to increase or decrease the <c>AlarmLight</c> brightness
/// value.
/// </summary>
	public float BrightnessStep = 1.0f;

/// <summary>
/// The maximum brightness of <c>AlarmLight</c>.
/// </summary>
	public float MaxBright = 8.0f;

/// <summary>
/// The minimum brightness setting when flashing the <c>AlarmLight</c>
/// or when it is in the off deactivated state.
/// deactivated.
/// </summary>
	public float MinBright = 2.0f;

	#endregion

	#region Private Members

/// <summary>
/// Whether or not the alarm is activated.
/// </summary>
	private bool Activated;

/// <summary>
/// An internal value used for blinking the alarm light.
/// </summary>
	private float BrightnessToggle = 0.0f;

	#endregion

	#region Constructors

/// <summary>
/// Create an alarm in a de-activated state.
/// </summary>
	public void Awake () {
		Activated = false;
		AlarmStation.renderer.material.color = new Color(255.0f, 0.0f, 0.0f);
	}

	#endregion

	#region Public Methods

/// <summary>
/// Activate the alarm.
/// </summary>
	public void Activate(bool soundEnabled = true) {
		Activated = true;
		AlarmLight.light.intensity = MinBright;

		if(Activated && soundEnabled) {
			AlarmStation.audio.Play();
		} else {
			AlarmStation.audio.Stop();
		}
	}

/// <summary>
/// Flash the light whenever the alarm is activated.
/// </summary>
	public void Update() {
		if(!Activated) return;

	//Animate the flashing of the light
		if(AlarmLight.light.intensity >= MaxBright) {
			BrightnessToggle = -0.5f;
		} else if(AlarmLight.light.intensity <= MinBright) {
			BrightnessToggle = 0.5f;
		}

		AlarmLight.light.intensity += (BrightnessStep * BrightnessToggle);
	}

	#endregion
}