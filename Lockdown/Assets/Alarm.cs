using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour {
	#region Fields

/// <summary>
/// Whether or not the alarm is enabled, and should be alarming.
/// </summary>
	public bool Activated {
		get { return activated; }
		set {
			activated = value;
			AlarmLight.light.intensity = MinBright;

			if(activated) {
				AlarmStation.audio.Play();
			} else {
				AlarmStation.audio.Stop();
			}
		}
	}

	private bool activated = false;

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

	#region Private Fields

/// <summary>
/// An internal value used for blinking the alarm.
/// </summary>
	private int BrightnessToggle = 1;

	#endregion

	// Use this for initialization
	public void Awake () {
		Activated = false;
		AlarmStation.renderer.material.color = new Color(255.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	public void Update () {
		if(!Activated) return;

	//Animate the flashing of the light
		if(AlarmLight.light.intensity >= MaxBright) {
			BrightnessToggle = -1;
		} else if(AlarmLight.light.intensity <= MinBright) {
			BrightnessToggle = 1;
		}

		AlarmLight.light.intensity += (BrightnessStep * BrightnessToggle);
	}
}