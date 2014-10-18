using UnityEngine;

/// <summary>
/// An abstract class which all blockade prefabs must inherit in order
/// to open, or grant passage through, the blockade.
/// </summary>
public abstract class Blockade : MonoBehaviour {
	#region Fields

/// <summary>
/// An array of objects to destroy when the blockade is opening.
/// </summary>
	public GameObject[] Dest;

/// <summary>
/// The door to open to allow passage.
/// </summary>
	public GameObject Door;

/// <summary>
/// Whether or not the blockade should open immediately when the
/// game initializes.
/// </summary>
	public bool OpenOnStart;

	#endregion

	#region Constructors

/// <summary>
/// Open the blockade on game initialization, if configured to do so
/// automatically.
/// </summary>
	public void Start() {
		if(OpenOnStart) Open();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Open, or grant passage through, the blockade. Use the keyword
/// "override" to implement custom opening functionality.
/// </summary>
	public virtual void Open() {
	//Play the door opening noise
		gameObject.audio.Play();

	//Destory stuff
		for(int i = 0; i < Dest.Length; ++i) {
			Destroy(Dest[i]);
		}
	}

	#endregion
}