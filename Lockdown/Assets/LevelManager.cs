using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	private bool Transable = false;
	private string Next = string.Empty;

	void Awake() {
		// Make sure the level manager itself survives all level transitions
		DontDestroyOnLoad(gameObject);
	}

/// <summary>
/// Initialize the Network Manager on the first level.
/// </summary>
	public void Start() {
		RestoreNetMgrState(GameObject.FindGameObjectWithTag("Network Manager"));
	}

	[RPC]
	public void Trans() {
		Transable = true;
		TransitionLevel(Next);
		Transable = false;
	}

	// Use this function for any level transitions in the game.
	// It presents the same interface as Application.LoadLevel() (pass the name of the level to be loaded),
	// but performs any necessary setup/transition tasks needed to ensure game continuity.
	public void TransitionLevel(string newLevelName)
	{
		// Basic format for switch cases:
		// 		*Clean up old level (scoring, deal with stranded players, close elevator doors, etc.)
		//		*Call Application.LoadLevel() to perform the actual switch (removes all non-persistent
		//			objects, loads all objects from new level)
		//		*Restore state (NetworkManager, etc.)
		//		*Set up new level

		if(Network.isClient && !Transable) {
			Next = newLevelName;
			return;
		} else if(Network.isServer) {
			networkView.RPC("Trans", RPCMode.OthersBuffered);
		} else {
			return;
		}

		switch(newLevelName)
		{
		case "Level 0":
			Application.LoadLevel ("Level 0");
			break;

		case "Level I":
		case "Level 1": // old typos never die...but we can patch over them! :-D
			Application.LoadLevel ("Level I");
			break;

		case "Level 2":
			Application.LoadLevel ("Level 2");
			break;

		case "Level 3":
			Application.LoadLevel ("Level 3");
			break;

		case "FinalScene":
			Application.LoadLevel ("FinalScene");
			break;

		default:
			throw new Lockdown_LevelNotFoundException(
				string.Format ("Level '{0}' not recognized by the LevelManager. If you added a new level, did you forget to add it to LevelManager.cs?", newLevelName));
		}
	}

	void RestoreNetMgrState(GameObject newNetMgrGameObj) {
		NetworkManager newNetMgr = newNetMgrGameObj.GetComponent<NetworkManager>();

		newNetMgr.AWS_URL = LockdownGlobals.Instance.AWSServer;
		newNetMgr.gameName = LockdownGlobals.Instance.GameName;
		newNetMgr.isServer = LockdownGlobals.Instance.Host == Host.Server;
		newNetMgr.networkingOn = LockdownGlobals.Instance.NetworkingEnabled;
		newNetMgr.useAWSserver = LockdownGlobals.Instance.AWSServerEnabled;
		newNetMgr.Init();
	}

	public void OnLevelWasLoaded() {
		RestoreNetMgrState(GameObject.FindGameObjectWithTag("Network Manager"));
	}

	#region Exception Types

	public class Lockdown_LevelNotFoundException : System.Exception
	{
		// Constructor - pass the error message associated with this exception
		public Lockdown_LevelNotFoundException(string msg)
		{
			Message = msg;
		}

		public new string Message
		{ get; private set; }
	}

	#endregion
}