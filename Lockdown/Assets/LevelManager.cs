using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	LockdownGlobals globals = LockdownGlobals.Instance;

	// Handles to prefabs for objects that will be instantiated by the LevelManager
	// 		(these must be set in the GUI!)
	public GameObject Player1Prefab, Player2Prefab, Player3Prefab;
	public GameObject BlockadeManagerL1;

	private int lastLevelPrefix = 0;

	private GameObject player1 = null, player2 = null, player3 = null;

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
		case "Level 1": // old typos never die...but we can patch over them! :-D
		{
			lastLevelPrefix = 1;

			Network.SetSendingEnabled(0, false); // stop sending network messages for group 0
			Network.isMessageQueueRunning = false; // stop receiving network messages

			Network.SetLevelPrefix(1); // prefix keeps the new level from getting extraneous old updates
			Application.LoadLevel("Level I");

			Network.isMessageQueueRunning = true;
			Network.SetSendingEnabled(0, true);

			//// Set up the level

			// Initialize maze
			GameObject maze = GameObject.FindGameObjectWithTag("MazeManager");
			LOneMazeManager mazeScript = maze.GetComponent<LOneMazeManager>();
			mazeScript.Init();

			// Initialize blockade manager with blockades from maze + hallway
			GameObject blockadeMgrObj = (GameObject)Network.Instantiate (BlockadeManagerL1, Vector3.zero, Quaternion.identity, 0);
			BlockadeManager blockadeMgr = blockadeMgrObj.GetComponent<BlockadeManager>();
			blockadeMgr.MazeManager = maze; // manager will pull the maze's blockades from this
			blockadeMgr.Blockades = new GameObject[3]; // will hold the 3 hallway blockades
			GameObject dungeon = GameObject.FindGameObjectWithTag ("Dungeon");
			blockadeMgr.Blockades[0] = dungeon.transform.Find ("Blockades/Brute").gameObject;
			blockadeMgr.Blockades[1] = dungeon.transform.Find ("Blockades/Hacker").gameObject;
			blockadeMgr.Blockades[2] = dungeon.transform.Find ("Blockades/Thief").gameObject;
			blockadeMgr.Init();

			// Spawn players
			if(globals.networkingOn)
			{
				if(globals.isServer)
				{
					// Find player placeholders
					GameObject P1Placeholder = GameObject.Find ("Player 1 Placeholder");
					GameObject P2Placeholder = GameObject.Find ("Player 2 Placeholder");

					// Since this is the server, we will have control of players 1 and 2.
					player1 = (GameObject)Network.Instantiate(Player1Prefab, P1Placeholder.gameObject.transform.position, P1Placeholder.gameObject.transform.rotation, 0);
					player2 = (GameObject)Network.Instantiate(Player2Prefab, P2Placeholder.gameObject.transform.position, P2Placeholder.gameObject.transform.rotation, 0);
					// Activate P1 and P2's cameras
					player1.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
					player2.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);

					//TODO: get handle to player 3 so we can set DontDestroyOnLoad
					// (maybe this will be synchronized automatically, since this will be done on the
					// remote end? I suspect not...)
				}
				else // we are the client
				{
					// Find player placeholders
					GameObject P3Placeholder = GameObject.Find ("Player 3 Placeholder");

					// Since this is the client, we will have control of players 3 and 4.
					player3 = (GameObject)Network.Instantiate(Player3Prefab, P3Placeholder.gameObject.transform.position, P3Placeholder.gameObject.transform.rotation, 0);
					//player4 = (GameObject)Network.Instantiate(P4Controller, P4Placeholder.gameObject.transform.position, P4Placeholder.gameObject.transform.rotation, 0);
					// Activate P3 and P4's cameras
					player3.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);
					//player4.gameObject.transform.Find("Main Camera").gameObject.SetActive(true);

					//TODO: get handle to players 1 and 2 (as above)
				}
			}
			else // offline mode
			{
				// Find player placeholders
				GameObject P1Placeholder = GameObject.Find ("Player 1 Placeholder");
				GameObject P2Placeholder = GameObject.Find ("Player 2 Placeholder");

				player1 = (GameObject)Object.Instantiate(Player1Prefab, P1Placeholder.gameObject.transform.position, P1Placeholder.gameObject.transform.rotation);
				player2 = (GameObject)Object.Instantiate(Player2Prefab, P2Placeholder.gameObject.transform.position, P2Placeholder.gameObject.transform.rotation);
				//TODO: create player 3, set up 3-way split screen
			}

			// Now that we've instantiated all the players, we'll keep the same instances alive throughout
			// the game.
			if(player1 != null) DontDestroyOnLoad(player1);
			if(player2 != null) DontDestroyOnLoad(player2);
			if(player3 != null) DontDestroyOnLoad(player3);

			break;
		}
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
				string.Format ("Level '{0}' not recognized by the LevelManager. If you added a new level, did you forget to add it to LevelManager.cs?", newLevelName));
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
