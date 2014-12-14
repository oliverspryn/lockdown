using UnityEngine;
using System.Collections;

#region Delegates

/// <summary>
/// A delegate which is used when an <c>Explosion</c> has completed.
/// </summary>
public delegate void ExplosionCompleted();

/// <summary>
/// A delegate which is used when an <c>Explosion</c> has started.
/// </summary>
public delegate void ExplosionStarted();

#endregion

/// <summary>
/// This class manages the appearance of the individual explosion
/// fireballs, such as how this item will appear when beginning to
/// explode and when it finishes.
/// </summary>
public class Explosion : MonoBehaviour {
	#region Events

/// <summary>
/// An event to dispatch whenever an <c>Explosion</c> has completed.
/// </summary>
	public event ExplosionCompleted Completed;

/// <summary>
/// An event to dispatch whenever an <c>Explosion</c> has started.
/// </summary>
	public event ExplosionCompleted Started;

	#endregion

	#region Fields

/// <summary>
/// Whether or not this fireball is actively exploding.
/// </summary>
	public bool Exploding {
		get;
		private set;
	}

/// <summary>
/// The maximum size of the fire, in X, Y, and Z directions, at the
/// height of the explosion.
/// </summary>
	public float MaxScale = 20.0f;

/// <summary>
/// A Lerp multiplier.
/// </summary>
	public float Smoothing = 1.0f;

	#endregion

	#region Private Members

/// <summary>
/// The script which controls the core action of the fireball.
/// </summary>
	private ExplosionMat FireballScript;

	#endregion

	#region Constructors

/// <summary>
/// Initialize the <c>Explosion</c> to inactive and hidden.
/// </summary>
	public void Start() {
		Exploding = false;
		FireballScript = gameObject.GetComponent<ExplosionMat>();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Trigger the explosion.
/// </summary>
	public void Trigger() {
		if(Started != null)
			Started();

		Exploding = true;
		gameObject.audio.Play();
		gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}

/// <summary>
/// Play the fireball explosion animation.
/// </summary>
	public void Update() {
	//Is the explosion active?
		if(!Exploding)
			return;

	//When should the explosion stop?
		if(FireballScript._alpha < 0.01f) {
			if(Completed != null)
				Completed();

			Exploding = false;
			FireballScript._alpha = 1.0f;
			gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
			return;
		}

	//Grow the explosion
		gameObject.transform.localScale = Vector3.Lerp(
			gameObject.transform.localScale,
			new Vector3(MaxScale, MaxScale, MaxScale),
			Time.deltaTime / Smoothing
		);

	//Fade the explosion
		FireballScript._alpha = Mathf.Lerp(FireballScript._alpha, 0.0f, Time.deltaTime / Smoothing);
	}

	#endregion
}