using UnityEngine;

/// <summary>
/// This class will create a clock, allow the player to trigger the
/// clock, and cause the environment to start exploding if the level
/// is not finished in time.
/// </summary>
public class Destroyer : MonoBehaviour {
	#region Fields

/// <summary>
/// Activate or deactivate the destroyer and the countdown HUD.
/// </summary>
	public bool Active {
		get { return Clock.Active; }
		set {
		//Don't reactivate, if already active
			if(Active && value)
				return;

		//Display the HUD
			HUD.SetActive(value);

		//Set the time
			Clock.ElapsedTime = ElapsedTime;
			Clock.Active = value;
			Clock.TimeExpired += TimeExpired;

		//Don't start destroying anything, ever!
			Destroying = false;
		}
	}

/// <summary>
/// The elapsed time of the <c>Clock</c>, in seconds. Values may
/// be between 0 seconds, and 5939 seconds (99 minutes, 59 seconds).
/// </summary>
	public int ElapsedTime;

/// <summary>
/// A reference to an object which will display an exploding 
/// fireball.
/// </summary>
	public GameObject Explosion;

/// <summary>
/// The maximum number of explosions which may occur at any one time.
/// </summary>
	public int ExplosionCount = 10;

/// <summary>
/// The overall duration, in seconds, of the set of explosions.
/// </summary>
	public int ExplosionDuration = 10;

/// <summary>
/// How should the overall movement of the explosions progress as
/// time progresses?
/// </summary>
	public ExplosionProgression ExplosionProgression = ExplosionProgression.Everywhere;

/// <summary>
/// A reference to the object which will be used as the countdown
/// HUD.
/// </summary>
	public GameObject HUD;

/// <summary>
/// The level to return to, if the timer expires and the player
/// does not finish in time.
/// </summary>
	public int LevelIndex;

/// <summary>
/// This is an empty <c>GameObject</c> which is used as a marker
/// for the area in 3D space in which explosions may occur. Think
/// of the playable area as being roughly enclosed within a cube:
/// 
///         A
///         +---------------+
///        /|              /|
///       / |             / |
///      /  |            /  |
///     +---------------+   |
///     |   |           |   |
///     |   +-----------|---+
///     |  /            |  /
///     | /             | /
///     |/              |/
///     +---------------+
///                     B
///                     
/// By placing one marker in one corner, and another marker in 
/// the extreme opposite corner, the 3D cube can be mapped out, and
/// will indicate the region in which explosions may occur.
/// 
/// For example, if marker one was placed at point A, then marker
/// two will need to be placed at marker B. The order of the markers
/// do not matter.
/// </summary>
	public GameObject Marker1;

/// <summary>
/// This is an empty <c>GameObject</c> which is used as a marker
/// for the area in 3D space in which explosions may occur. Think
/// of the playable area as being roughly enclosed within a cube:
/// 
///         A
///         +---------------+
///        /|              /|
///       / |             / |
///      /  |            /  |
///     +---------------+   |
///     |   |           |   |
///     |   +-----------|---+
///     |  /            |  /
///     | /             | /
///     |/              |/
///     +---------------+
///                     B
///                     
/// By placing one marker in one corner, and another marker in 
/// the extreme opposite corner, the 3D cube can be mapped out, and
/// will indicate the region in which explosions may occur.
/// 
/// For example, if marker two was placed at point A, then marker
/// one will need to be placed at marker B. The order of the markers
/// do not matter.
/// </summary>
	public GameObject Marker2;

/// <summary>
/// How the location of the explosions should occur when they move
/// through space.
/// </summary>
	public ExplosionProgressionType ProgressionType = ExplosionProgressionType.Linear;

	#endregion

	#region Private Members

/// <summary>
/// A reference to the script powering the <c>Clock</c> in the
/// HUD.
/// </summary>
	private Clock Clock;

/// <summary>
/// Set the HUD to be destroyed and the explosions to begin when 
/// the timer expires.
/// </summary>
	private bool Destroying = false;

/// <summary>
/// An object which will manage all of the explosions in the scene.
/// </summary>
	private ExplosionManager ExplosionManager;

	#endregion

	#region Constructors

/// <summary>
/// Initialize the destroyer to inactive.
/// </summary>
	public void Start() {
	//Get the clock script
		foreach(Transform t in HUD.transform) {
			if(t.gameObject.name == "Clock") {
				Clock = t.GetComponent<Clock>();
			}
		}

	//Initialize all of the explosions
		ExplosionManager = new ExplosionManager(
			Explosion,
			ExplosionCount,
			ExplosionDuration,
			ExplosionProgression,
			ProgressionType,
			new ExplodingRegion(Marker1, Marker2)
		);

	//Deactivate the destroyer
		Active = false;
	}

	#endregion

	#region Public Methods

/// <summary>
/// Destroy the HUD, and start up the explosions.
/// </summary>
	public void Update() {
	//Should anything be destroyed?
		if(!Destroying)
			return;

	//Destory things!
		ExplosionManager.Trigger();
		ExplosionManager.Update();

	//Turn off the HUD
		HUD.SetActive(false);
	}

	#endregion

	#region Private Methods

/// <summary>
/// Set the HUD to be destroyed and the explosions to begin when 
/// the timer expires.
/// </summary>
	private void TimeExpired() {
		Destroying = true;
	}

	#endregion
}