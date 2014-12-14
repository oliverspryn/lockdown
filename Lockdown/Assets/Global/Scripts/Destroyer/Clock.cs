using System.Timers;
using UnityEngine;

#region Delegates

/// <summary>
/// A delegate which is used when the time on the <c>Clock</c>
/// has expired.
/// </summary>
public delegate void TimeExpired();

#endregion

/// <summary>
/// This class manages the countdown of the clock which is 
/// displayed as part of the HUD.
/// </summary>
public class Clock : MonoBehaviour {
	#region Events

/// <summary>
/// An event to dispatch whenever the time on the <c>Clock</c>
/// has expired.
/// </summary>
	public event TimeExpired TimeExpired;

	#endregion

	#region Fields

/// <summary>
/// Activate or deactivate the <c>Clock</c>.
/// </summary>
	public bool Active {
		get { return act; }
		set {
			act = value;

		//Deactivate the timer
			if(!value) {
				if(Timer != null) {
					Timer.Enabled = false;
					Timer.Stop();
					Timer.Dispose();
				}

				return;
			}

		//Check the range of the countdown
			if(ElapsedTime < 0)
				ElapsedTime = 0;
			else if(ElapsedTime > 5939) //99 minutes, 59 seconds
				ElapsedTime = 5939;

			InternalCounter = ElapsedTime;

		//Set the values of the digits
			int time = ElapsedTime;
			int tenM = time / 600;
			time = (time >= 600) ? time - tenM * 600 : time;

			int oneM = time / 60;
			time = (time >= 60) ? time - oneM * 60 : time;

			int tenS = time / 10;
			time = (time >= 10) ? time - tenS * 60 : time;

			int oneS = time;

			TenMinutesScript.Value = tenM;
			OneMinuteScript.Value  = oneM;
			TenSecondsScript.Value = tenS;
			OneSecondScript.Value  = oneS;

		//Activate the timer
			Timer = new Timer(1000);
			Timer.Elapsed += new ElapsedEventHandler(Tick);
			Timer.AutoReset = true;
			Timer.Enabled = true;
			Timer.Start();
		}
	}

	private bool act = false;

/// <summary>
/// The time, in seconds, at which to play a clock tick sound at 
/// each second. This is helpful when the player's time is running
/// out, and they need an audio cue to accelerate their progress.
/// </summary>
	public int ElapsedTicker = 20;

/// <summary>
/// The elapsed time of the <c>Clock</c>, in seconds. Values may
/// be between 0 seconds, and 5939 seconds (99 minutes, 59 seconds).
/// </summary>
	public int ElapsedTime;

/// <summary>
/// The <c>Digit</c> representing the 1-minute value.
/// </summary>
	public GameObject OneMinute;

/// <summary>
/// The <c>Digit</c> representing the 1-second value.
/// </summary>
	public GameObject OneSecond;

/// <summary>
/// The <c>Digit</c> representing the 10-minute value.
/// </summary>
	public GameObject TenMinutes;

/// <summary>
/// The <c>Digit</c> representing the 10-second value.
/// </summary>
	public GameObject TenSeconds;

	#endregion

	#region Private Members

/// <summary>
/// Internally count how many seconds remain after each tick, so
/// as not not mess the with user-modifiable <c>ElapsedTime</c> 
/// variable.
/// </summary>
	private int InternalCounter = 9999;

/// <summary>
/// The <c>Digit</c> script associated with the representing the
/// 1-minute digit.
/// </summary>
	private Digit OneMinuteScript;

/// <summary>
/// The <c>Digit</c> script associated with the representing the
/// 1-second digit.
/// </summary>
	private Digit OneSecondScript;

/// <summary>
/// The <c>Digit</c> script associated with the representing the
/// 10-minute digit.
/// </summary>
	private Digit TenMinutesScript;

/// <summary>
/// The <c>Digit</c> script associated with the representing the
/// 10-second digit.
/// </summary>
	private Digit TenSecondsScript;

/// <summary>
/// A reference to the C# timer, which will power this <c>Clock</c>.
/// </summary>
	private Timer Timer = null;

/// <summary>
/// Set to true after each second of the <c>Clock</c> tick, and is 
/// used to indicate when the UI should be updated.
/// </summary>
	private bool UpdateUI = false;

	#endregion

	#region Constructors

/// <summary>
/// Create the <c>Clock</c> and get all of the scripts from the
/// individual <c>Digit</c> objects.
/// </summary>
	public void Start() {
		OneMinuteScript = OneMinute.GetComponent<Digit>();
		OneSecondScript = OneSecond.GetComponent<Digit>();
		TenMinutesScript = TenMinutes.GetComponent<Digit>();
		TenSecondsScript = TenSeconds.GetComponent<Digit>();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Update the UI of the <c>Clock</c> only when told to do so by
/// the Timer.Elapsed event.
/// </summary>
	public void Update() {
		if(!UpdateUI)
			return;

	//Update the clock UI
		--OneSecondScript;

		if(OneSecondScript.Value == 9) {
			--TenSecondsScript;

			if(TenSecondsScript.Value == 9) {
				TenSecondsScript.Value = 5; // Only goes to 59 seconds, not 99!
				--OneMinuteScript;

				if(OneMinuteScript.Value == 9) {
					--TenMinutesScript;
				}
			}
		}

	//Count down, in seconds, and wait until the next second before changing the UI
		UpdateUI = false;
		--InternalCounter;

	//Play the clock tick sound
		if(InternalCounter <= ElapsedTicker)
			gameObject.audio.Play();
	}

	#endregion

	#region Private Methods

/// <summary>
/// Set the UI of the <c>Clock</c> to be updated after each second.
/// </summary>
/// 
/// <param name="sender">The Timer which sent the event</param>
/// <param name="e">The event arguments</param>
	private void Tick(object sender, ElapsedEventArgs e) {
		UpdateUI = true;

	//Stop the timer
		if(InternalCounter == 0) {
			Active = false;
			UpdateUI = false;

			if(TimeExpired != null)
				TimeExpired();
		}
	}

	#endregion
}