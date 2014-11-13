using UnityEngine;

#region Delegates

/// <summary>
/// A delegate which is used during a <c>Blockade</c> opened event,
/// </summary>
/// 
/// <param name="sender">The sending blockade object</param>
/// <param name="e">Information about the blockade network ID</param>
public delegate void EventHandler(object sender, int e);

#endregion

/// <summary>
/// An abstract class which all blockade prefabs must inherit in order
/// to open, or grant passage through, the blockade.
/// </summary>
public abstract class Blockade : MonoBehaviour {
	#region Events

/// <summary>
/// An event to dispatch whenever a <c>Blockade</c> has been opened.
/// </summary>
	public event EventHandler Opened;

	#endregion

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
/// The height of the blockade prefab.
/// </summary>
	public float Height {
		get {
			float height = 0.0f;

			for(int i = 0; i < HeightObjects.Length; ++i) {
				height += HeightObjects[i].transform.localScale.y;
			}

			return height;
		}
	}

/// <summary>
/// The object(s) which, when their heights are added together, make
/// up the overall height of the blockade prefab.
/// </summary>
	public GameObject[] HeightObjects;

/// <summary>
/// Whether or not the <c>Blockade</c> is open.
/// </summary>
	public bool IsOpen { get; private set; }

/// <summary>
/// The network ID assigned to the <c>Blockade</c> by a network manager
/// object.
/// </summary>
	public int NetID { get; set; }

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
		IsOpen = false;
		if(OpenOnStart) Open();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Open, or grant passage through, the blockade. Use the keyword
/// "override" to implement custom opening functionality.
/// </summary>
	public virtual void Open() {
		if(IsOpen) {
			return;
		}

	//Play the door opening noise
		gameObject.audio.Play();

	//Destory stuff
		for(int i = 0; i < Dest.Length; ++i) {
			Destroy(Dest[i]);
		}

		IsOpen = true;
		Opened(this, NetID);
	}

	#endregion
}