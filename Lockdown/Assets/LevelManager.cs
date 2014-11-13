using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	void Awake() {
		// Make sure the level manager itself survives all level transitions
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Use this function for any level transitions in the game.
	// It presents the same interface as Application.LoadLevel() (pass the name of the level to be loaded),
	// but performs any necessary setup/transition tasks needed to ensure game continuity.
	public void TransitionLevel(string newLevelName)
	{
		// Basic format for switch cases:
		// 		*Clean up old level (scoring, deal with stranded players, close elevator doors, etc.)
		//		*Save continuous state data (NetworkManager, etc.)
		//		*Call Application.LoadLevel() to perform the actual switch (removes all non-persistent
		//			objects, loads all objects from new level)
		//		*Restore state (NetworkManager, etc.)
		//		*Set up new level (open elevator doors, etc.)

		switch(newLevelName)
		{
		case "Video":
			Application.LoadLevel ("Video");
			break;
		case "Level I":
			Application.LoadLevel ("Level I");
			break;
		case "Level 2":
		{
			//NetMgrState netMgrState = SaveNetMgrState(GameObject.Find ("NetworkManager"));
			Application.LoadLevel ("Level 2");
			GameObject l2NetMgr = GameObject.Find("NetworkManager");
			//RestoreNetMgrState(l2NetMgr, netMgrState);
			//if(Network.isClient) // sleep to give the server time to make the transition
			//	System.Threading.Thread.Sleep (3000);
			l2NetMgr.SetActive (true);
			break;
		}
		case "Level 3":
		{
			NetMgrState netMgrState = SaveNetMgrState(GameObject.Find ("NetworkManager"));
			Application.LoadLevel ("Level 3");
			RestoreNetMgrState(GameObject.Find("NetworkManager"), netMgrState);
			break;
		}
		case "FialScene":
			Application.LoadLevel ("FinalScene");
			break;
		default:
			throw new Lockdown_LevelNotFoundException(
				string.Format ("Level '{0}' not recognized by the LevelManager.", newLevelName));
		}
	}

	// To store various properties of the NetworkManager for continuity between levels
	class NetMgrState
	{
		public bool networkingOn, isServer;
		public string gameName, gameComment;
		public bool useAWSserver;
		public string AWS_URL;
	}
	
	NetMgrState SaveNetMgrState(GameObject oldNetMgrGameObj)
	{
		NetworkManager oldNetMgr = oldNetMgrGameObj.GetComponent<NetworkManager>();

		NetMgrState state = new NetMgrState();
		state.networkingOn = oldNetMgr.networkingOn;
		state.isServer = oldNetMgr.isServer;
		state.gameName = oldNetMgr.gameName;
		state.gameComment = oldNetMgr.gameComment;
		state.useAWSserver = oldNetMgr.useAWSserver;
		state.AWS_URL = oldNetMgr.AWS_URL;

		return state;
	}

	void RestoreNetMgrState(GameObject newNetMgrGameObj, NetMgrState state)
	{
		NetworkManager newNetMgr = newNetMgrGameObj.GetComponent<NetworkManager>();

		newNetMgr.networkingOn = state.networkingOn;
		newNetMgr.isServer = state.isServer;
		newNetMgr.gameName = state.gameName;
		newNetMgr.gameComment = state.gameComment;
		newNetMgr.useAWSserver = state.useAWSserver;
		newNetMgr.AWS_URL = state.AWS_URL;
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
