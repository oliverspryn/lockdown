using UnityEngine;
using System.Collections;
using System.Timers;

public class Destroyer : MonoBehaviour {

	public bool Activate {
		set {
			gameObject.audio.Stop();
			Timer = new Timer(60000);
			Timer.Elapsed += new ElapsedEventHandler(Tick);
			Timer.Enabled = true;

			MinutelyCountdown[Minutes].audio.Play();
			--Minutes;
		}
	}

	public int LevelIndex;

	public int Minutes;

	private bool Hands = false;

	public GameObject[] MinutelyCountdown;

	private bool Restart = false;

	private Timer Timer;

	private void Tick(object sender, ElapsedEventArgs e) {
		if(Minutes > 0) {
			Hands = true;
		} else {
			Restart = true;
		}
	}

	public void Update() {
		if(Hands) {
			MinutelyCountdown[Minutes].audio.Play();
			--Minutes;

			Timer = new Timer(60000);
			Timer.Elapsed += new ElapsedEventHandler(Tick);
			Timer.Enabled = true;
			Hands = false;
		}

		if(Restart) {
			Application.LoadLevel(LevelIndex);
			Restart = false;
		}
	}
}